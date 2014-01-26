using UnityEngine;
using System.Collections;

public class OpeningScene : MonoBehaviour {

    void OnMouseDown()
    {
        Application.LoadLevel("MainScene");
    }
}
