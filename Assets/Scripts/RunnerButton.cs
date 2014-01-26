using UnityEngine;
using System;

    
public class RunnerButton : MonoBehaviour {

    public enum ButtonChar { Z, M, P, Q , nil}
    public ButtonChar buttonChar = ButtonChar.nil;
    public C.Colors buttonColor;

    private bool isActive = false;

    public SpriteRenderer EnabledSprite;

    public RunnerController runner; 
    void Awake()
    {
        DisableButton();
    }




    public void OnMouseUpAsButton()
    {
        if (!isActive && GameManager.i.state == GameManager.State.Menu)
        {
            GameManager.i.NewPlayerAvailable(this);

			//pick a random enum val from Colors
			var values = Enum.GetValues(typeof(C.Colors));
			int rand = UnityEngine.Random.Range(0,3); 
			//Don't blame me, blame Unity
			switch (rand)
			{
				case 0: { buttonColor = C.Colors.Blue; break; } 
				case 1: { buttonColor = C.Colors.Green; break; } 
				case 2: { buttonColor = C.Colors.Red; break; } 
				case 3: { buttonColor = C.Colors.White; break; } 
				case 4: { buttonColor = C.Colors.Yellow; break; } 
			}
			//Debug.Log("random: "+ rand+" color: "+ buttonColor);
            EnableButton();
        }
            

        if (isActive && GameManager.i.state == GameManager.State.Game)
        {
            if(runner != null)
                runner.RunnerAction();
        }
            
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (KeyCode.Q.ToString() == buttonChar.ToString())
                OnMouseUpAsButton();
        }
        
        if ( Input.GetKeyDown( KeyCode.Z ) )
        {
            if ( KeyCode.Z.ToString() == buttonChar.ToString() )
                OnMouseUpAsButton();
        }
        
        if ( Input.GetKeyDown( KeyCode.P ) )
        {
            if ( KeyCode.P.ToString() == buttonChar.ToString() )
                OnMouseUpAsButton();
        } 
        
        if ( Input.GetKeyDown( KeyCode.M ) )
        {
            if ( KeyCode.M.ToString() == buttonChar.ToString() )
                OnMouseUpAsButton();
        }
    }

    void OnEnable()
    {
        EventManager.ResetGame += DisableButton;
        EventManager.RunnerChangedColor += SyncRunnerColor;
    }

    void OnDisable()
    {
        EventManager.ResetGame -= DisableButton;
        EventManager.RunnerChangedColor -= SyncRunnerColor;
    }



    void DisableButton()
    {
        EnabledSprite.color = Color.white;
        isActive = false;
        runner = null;
    }

    void EnableButton()
    {
		EnabledSprite.color = C.GetRealColor(buttonColor);
        isActive = true;
    }

    private void SyncRunnerColor( RunnerController rc )
    {
        if (runner != null && runner == rc)
        {
            buttonColor = rc.CurrentColor;
            EnabledSprite.color = C.GetRealColor(buttonColor);
        }
    }
}
