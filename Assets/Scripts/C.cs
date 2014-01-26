using System;
using UnityEngine;

public class C : MonoBehaviour
{
    public static C i;
    public static int MAX_RUNNER_HP = 5;

    void Awake()
    {
        i = this;
    }

    public enum Colors
    {
        Red = 1,
        Green = 2,
        Blue = 3,
        Yellow = 4,
        White = 5
    };

    public Transform WorldCenter;
    public GameObject WavePrefab;
    public TextMesh    GlobalTimer ;
    public TextMesh    DeathText ;

    public GameObject RunnerPrefab;
    public GameObject BurrowParticlePrefab;
    public GameObject BurrowSign;

    public Color Red;
    public Color Green;
    public Color Blue;
    public Color Yellow;

    public static Color GetRealColor(Colors c)
    {
        switch (c)
        {
            case Colors.Red:
                return i.Red;
            case Colors.Green:
                return i.Green;
            case Colors.Blue:
                return i.Blue;
            case Colors.Yellow:
                return i.Yellow;
        }
        return Color.white;
    }

    public static float WORLD_RADIUS = 3.8f;

    public static float TimeUntilWave = 17f;//seconds
	public static float WaveSegmentSizeAngle = 3f;
	public static int WAVE_DIRECTION = 1; // 1 - CW , -1 - CCW
	public static float WAVE_DELAY = 0.001f; //seconds
    public static int WAVE_MOVE_RATE = 10;
    
    public static int RunnerDistance = 4;
    public static int RunnerAmount = 4;

	public static float TimeAfterWave = 5f;

    public static float TimeAfterEndToHardReset = 5f;
}
