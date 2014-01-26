using UnityEngine;

public class StartButton : MonoBehaviour {

    public  SpriteRenderer sr;

    void Awake()
    {
        renderer.enabled = true;
        sr.enabled = true;
    }

    void OnEnable()
    {
        EventManager.StartGame += HideButton;
        EventManager.MainMenu += Showbutton;
        EventManager.ResetGame += Showbutton;
    }

    void OnDisable()
    {
        EventManager.StartGame -= HideButton;
        EventManager.MainMenu -= Showbutton;
        EventManager.ResetGame -= Showbutton;
    }
    
    
    void OnMouseUpAsButton()
    {
        EventManager.RaiseStartGame();
        Debug.Log("Start Button pushed!");
    }


    void Showbutton()
    {
        renderer.enabled = true;
        sr.enabled = true;
        collider2D.enabled = true;
    }

    void HideButton()
    {
        renderer.enabled = false;
        sr.enabled = false;
        collider2D.enabled = false;
    }

}
