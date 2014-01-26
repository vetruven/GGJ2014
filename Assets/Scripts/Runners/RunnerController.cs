using UnityEngine;

public class RunnerController : MonoBehaviour {

    const float FORWARD_FORCE = 5f;
    const float DOWN_FORCE = -10f;

    float JumpForce = 15f;
    float ForwardForce = 5f;
    float DownForce = -10f;

    bool Grounded = true;
    bool NotBurrowed = true;
    bool Burrowing = false;

    private string RunnerKeyString;

    public SpriteRenderer ColorOverlay;
    public SpriteRenderer PlayerSprite;

    public string RunnerKey 
    {
        get 
        {
            return RunnerKeyString;
        }
        set
        {
            RunnerKeyString = value;
            GetComponentInChildren<TextMesh>().text = value;
        }
    }

    Transform ground;
    GameObject burrowSign;

    private C.Colors currColor;

    public C.Colors CurrentColor 
    {
        get { return currColor; }
        set { currColor = value; }
    }

    void Awake()
    {
        if (transform.Find("GroundCheck") != null)
        {
            ground = transform.Find("GroundCheck");
        }



        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Players"), LayerMask.NameToLayer("Players"));
    }

    void OnEnable()
    {
        EventManager.GameEnd += RunnerGameEnd;
        EventManager.ResetContinueStage += UnBurrow;
        EventManager.Wave += WaveHit; 
    }

    void OnDisable()
    {
        EventManager.GameEnd -= RunnerGameEnd;
        EventManager.ResetContinueStage -= UnBurrow;
        EventManager.Wave -= WaveHit; 
    }


	void Update () 
    {
        Grounded = Physics2D.Linecast(transform.position, ground.position, 1 << LayerMask.NameToLayer("Ground"));

        //Debug.Log("" + currColor.ToString());
        ColorOverlay.GetComponent<SpriteRenderer>().color = C.GetRealColor( currColor );

        foreach (TextMesh txt in GetComponentsInChildren<TextMesh>())
        {
            txt.color = C.GetRealColor(currColor);
        }

        if (NotBurrowed)
        {
            UpdateVelocity();
        }
        else
        {
            rigidbody2D.velocity = Vector2.zero;
        }

        if (Burrowing)
        {
            Burrow();
        }

        if (Input.GetKeyUp(KeyCode.J)) { RunnerAction(); }

        if (Input.GetKeyUp(KeyCode.U)) { UnBurrow(); }

        #region REMOVED MOVEMENT
        /*
        if (Input.GetKey(KeyCode.J))
        {
            if (Grounded)
            {
                if (CanBurrow)
                {
                    Burrow();
                }
                else
                {
                    ForceJump(JumpForce);
                }
            }
            else
            {
                if (CanBurrow)
                {
                    Burrow();
                }
 
            }
        }
        if (Input.GetKeyUp(KeyCode.J) && !Grounded)
        {
            if (CanBurrow)
            {
                ForwardForce = FORWARD_FORCE;
                DownForce = DOWN_FORCE;
            }
            else
            {
                CanBurrow = true;
            }
        }

        if (Grounded)
        {
            CanBurrow = false; 
        }

        */
        #endregion

    }

    private float jumpVel = 0;

    public void RunnerAction()
    {
        if (Grounded)
        {
            ForceJump(JumpForce);
        }
        else
        {
            Burrow();
        }
    }

    public void UpdateVelocity()
    {
        RotateToCenter();
        float horVel = ForwardForce;
        float vertVel = (DownForce+jumpVel);

        if (jumpVel > 0)
        {
            jumpVel += DownForce*Time.deltaTime;
            jumpVel = Mathf.Max( jumpVel, 0 );
        }

        Vector3 v;
        v = transform.right*horVel;
        v += transform.up*vertVel;

        rigidbody2D.velocity = new Vector2(v.x, v.y);
    }


    public void ForceJump(float fForce)
    {
        jumpVel = JumpForce;
    }

    public void RotateToCenter()
    {
        if (C.i != null && C.i.WorldCenter == null) { return; }
        transform.up = -(C.i.WorldCenter.position - transform.position);
    }

    public void Burrow()
    {
        Burrowing = true;
        ForwardForce = 0f;
        DownForce = -30F;
        if (Grounded && NotBurrowed)
        {
            // Dead, remove any movement, remove renderer and collider, instantiate burrow and sign
            DownForce = 0f;
            NotBurrowed = false;
            Burrowing = false;
            ColorOverlay.renderer.enabled = false;
            PlayerSprite.renderer.enabled = false;

            rigidbody2D.isKinematic = true;
            collider2D.enabled = false;

            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }
            burrowSign = Instantiate(C.i.BurrowSign, transform.position, transform.rotation) as GameObject;
            burrowSign.transform.Rotate(new Vector3(0f,0f,33.4f));
            burrowSign.transform.parent = transform;
            burrowSign.transform.localPosition = new Vector2(-0.670f,-4.491f);

            burrowSign.transform.GetComponentInChildren<TextMesh>().text = RunnerKey;

            foreach (Renderer rend in burrowSign.transform.GetComponentsInChildren<Renderer>())
            {
                if (rend.gameObject.name == "Burrow_Sign_Base")
                    (rend as SpriteRenderer).color = C.GetRealColor(currColor);
            }





            EventManager.RaiseRunnerBorrowed(this);
        }   
    }

    public void UnBurrow()
    {
        NotBurrowed = true;
        Destroy(burrowSign);

        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        {
            rend.enabled = true;
        }

        rigidbody2D.isKinematic = false;
        collider2D.enabled = true;
        ColorOverlay.renderer.enabled = true;
        DownForce = DOWN_FORCE;
        ForwardForce = FORWARD_FORCE;

        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Players"), LayerMask.NameToLayer("Players"));
    }

    public void RunnerGameEnd()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("Before collision Color is : " + currColor.ToString());
        currColor = collider.gameObject.GetComponent<GemController>().MyColor;
        EventManager.RaiseRunnerChangedColor(this);
        //Debug.Log("After collision Color is : " + currColor.ToString());
    }

    public void WaveHit(C.Colors WaveColor)
    {
        // NOT (Burrowed and Colors are same.) (not burrowed or color is different)
        if (!(!NotBurrowed && WaveColor == currColor))
        {
            EventManager.RaiseRunnerDead(this);
            Destroy(gameObject);
        }
    }
}
