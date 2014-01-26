using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static Action MainMenu;
    public static void RaiseMainMenu() { FireAction(MainMenu);}

    public static Action StartGame;
    public static void RaiseStartGame() { FireAction( StartGame ); }

    public static Action<RunnerController> RunnerChangedColor;
    public static void RaiseRunnerChangedColor( RunnerController runner ) { FireAction( RunnerChangedColor, runner ); }

    public static Action<RunnerController> RunnerDead;
    public static void RaiseRunnerDead( RunnerController runner ) { FireAction( RunnerDead, runner ); }

    public static Action<RunnerController> RunnerBorrowed;
    public static void RaiseRunnerBorrowed( RunnerController runner ) { FireAction( RunnerBorrowed, runner ); }

    public static Action<C.Colors> Wave;
    public static void RaiseWave(C.Colors color) { FireAction( Wave, color ); }

    public static Action ResetContinueStage;
    public static void RaiseResetContinueStage() { FireAction( ResetContinueStage ); }

    public static Action GameEnd;
    public static void RaiseGameEnd() { FireAction( GameEnd ); }

    public static Action ResetGame;
    public static void RaiseResetGame() { FireAction( ResetGame ); }

    private static void FireAction(Action a)
    {
        if (a != null)
            a();
    }

    private static void FireAction<T>( Action<T> a, T t )
    {
        if ( a != null )
            a(t);
    }

}
