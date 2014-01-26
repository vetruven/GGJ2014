using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public static GameManager i;
    public enum State { Menu, Game, Wave, End }
    public State state = State.Menu;

    private List<RunnerController> runners;
    private List<RunnerController> borrowedPlayers;

    private List<RunnerButton> controls;
 
    private int alivePlayers;

    private float waveTime;

    void Awake()
    {
        i = this;
        runners = new List<RunnerController>();
        borrowedPlayers = new List<RunnerController>();
        controls = new List<RunnerButton>();
        C.i.GlobalTimer.renderer.enabled = false;

        HardReset();
    }


    void OnEnable()
    {
        EventManager.StartGame += StartGame;
        EventManager.ResetGame += HardReset;
        EventManager.ResetContinueStage += ResetAndStartStage;
        EventManager.RunnerDead += RunnerDeath;
        EventManager.RunnerBorrowed += RunnerBorrowed;
    }



    void OnDisable()
    {
        EventManager.StartGame -= StartGame;
        EventManager.ResetGame -= HardReset;
        EventManager.ResetContinueStage -= ResetAndStartStage;
        EventManager.RunnerDead -= RunnerDeath;
        EventManager.RunnerBorrowed -= RunnerBorrowed;

    }

    void HardReset()
    {
        runners.Clear();
        borrowedPlayers.Clear();
        controls.Clear();
        state = State.Menu;
        C.i.GlobalTimer.renderer.enabled = false;
        Debug.Log("Clearing all logs on hard reset");
        C.i.DeathText.text = "";
        C.i.GlobalTimer.text = "";

        WinScreenCont.i.HideWin();
    }

    public void StartGame()
    {
        state = State.Game;
        C.i.GlobalTimer.renderer.enabled = true;

        foreach (var control in controls)
        {
            control.runner = GeneratePlayer();
            control.runner.name = "player " + control.buttonChar.ToString();
            control.runner.RunnerKey = control.buttonChar.ToString();
            control.runner.CurrentColor = control.buttonColor;
            Debug.Log("created player for control ");
        }

        EventManager.RaiseResetContinueStage();
    }

    RunnerController GeneratePlayer()
    {
        GameObject go = (GameObject)Instantiate(C.i.RunnerPrefab);
        RunnerController rc = go.GetComponent<RunnerController>();

        rc.CurrentColor = (C.Colors)Random.Range(1, Enum.GetNames(typeof (C.Colors)).Length + 1);

        Vector3 offset = Random.insideUnitCircle.normalized*C.RunnerDistance;
        rc.transform.position = C.i.WorldCenter.position + offset;
        rc.RotateToCenter();

        runners.Add(rc);
        return rc;
    }

    void ResetAndStartStage()
    {
        waveTime = Time.time +  C.TimeUntilWave;
        borrowedPlayers.Clear();
        Sound.Play( Sound.i.music );
        Debug.Log("reset and start stage");
    }

    private void RunnerBorrowed( RunnerController runnerController )
    {
        borrowedPlayers.Add(runnerController);
        Debug.Log("borrowed  "+runnerController.name);
    }

    private void RunnerDeath(RunnerController rc)
    {
        runners.Remove(rc);
        Debug.Log("death"+rc.name);
    }

    void Update()
    {
        CheckForWinConditions();
        CheckforWaveConditions();

        UpdateTimer();
        
    }

    private int lastTimer;
    private void UpdateTimer()
    {
        if (state == State.Game)
        {
            int timer = Mathf.FloorToInt(Math.Max((waveTime - Time.time), 0));

            if (timer < 4 && timer > 0 && timer != lastTimer)
                Sound.Play(Sound.i.ping);

            if ( timer == 0 && timer != lastTimer )
                Sound.Play(Sound.i.lastPing);

            lastTimer = timer;

            C.i.GlobalTimer.text = timer.ToString("0");
            C.i.DeathText.text = "";
        }
            
        else if (state == State.Wave)
        {
            C.i.GlobalTimer.text = "";
            C.i.DeathText.text = "GopherMageddon";
        }  
    }

    private void CheckForWinConditions()
    {
        if (state == State.Game && runners.Count <= 1)
        {
            EventManager.RaiseGameEnd();
            state = State.End;
            Debug.Log("raise game end");

            if (runners.Count > 0)
            {
                WinScreenCont.i.ShowWin( runners[0].CurrentColor, runners[0].RunnerKey );
                C.i.DeathText.text = "";
            }
                
            else
                C.i.DeathText.text = "ALL ARE DEAD!!!";

            StartCoroutine(ResetGameDelayed(C.TimeAfterEndToHardReset));
        }
            
    }


    IEnumerator ResetGameDelayed(float delay)
    {
        yield return new WaitForSeconds( delay );
        EventManager.RaiseResetGame();
        Debug.Log( "Continue stage" );
    }

    private void CheckforWaveConditions()
    {
        if (state == State.Game && (waveTime < Time.time || borrowedPlayers.Count == runners.Count))
        {
            C.Colors waveColor;

            if (borrowedPlayers.Count > 0)
            {/*
                WaveController.i.SendWave(borrowedPlayers[0].transform.position,
                    borrowedPlayers[borrowedPlayers.Count - 1].transform.position);
              * */
                waveColor = borrowedPlayers[borrowedPlayers.Count - 1].CurrentColor;
            }
            else
            {
                waveColor = C.Colors.White;
            }

            Debug.Log( "WAVE COLOR = " + waveColor.ToString());
            EventManager.RaiseWave(waveColor);
            WaveController.WaveColor = waveColor;
            WaveController.i.SendWave();
            Sound.MuteAll();
            Sound.Play(Sound.i.wave, 0.75f);

            state = State.Wave;

            StartCoroutine(ContinueStageDelayed(C.TimeAfterWave));
        }
    }

    IEnumerator ContinueStageDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        state = State.Game;
        EventManager.RaiseResetContinueStage();
        Sound.Play(Sound.i.music);
        Debug.Log("Continue stage");
    }

    public void NewPlayerAvailable(RunnerButton RunnerCont)
    {
        controls.Add(RunnerCont);
        Debug.Log("control added => total controls#"+controls.Count);
    }
}
