using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class WinScreenCont : MonoBehaviour
{
    public static WinScreenCont i;

    public SpriteRenderer gopher;
    public List<SpriteRenderer> colorThings;

    public SpriteRenderer Q;
    public SpriteRenderer Z;
    public SpriteRenderer M;
    public SpriteRenderer P;


    void Awake()
    {
        i = this;
        colorThings.Add(Q);
        colorThings.Add(Z);
        colorThings.Add(M);
        colorThings.Add(P);
        HideWin();
    }

    public void ShowWin( C.Colors color, string runnerButton )
    {
        foreach (var sr in colorThings)
        {
            sr.color = C.GetRealColor(color);
        }

        gopher.enabled = true;
        colorThings[0].enabled = true;

        switch ( runnerButton )
        {
            case "Q":
                Q.enabled = true;
                break;
            case "Z":
                Z.enabled = true;
                break;
            case "M":
                M.enabled = true;
                break;
            case "P":
                P.enabled = true;
                break;
        }
    }


    public void HideWin()
    {
        foreach (var sr in colorThings)
        {
            sr.enabled = false;
        }

        gopher.enabled = false;
    }
}
