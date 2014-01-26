using UnityEngine;
using System.Collections;

public class WaveSegmentKiller : MonoBehaviour {

    void OnEnable()
    {
        EventManager.ResetContinueStage += DestroyMe;
    }

    void OnDestroy()
    {
        EventManager.ResetContinueStage -= DestroyMe;
    }



    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
