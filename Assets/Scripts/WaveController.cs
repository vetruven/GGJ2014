using UnityEngine;
using System.Collections;

public class WaveController : MonoBehaviour {


    public static WaveController i;


    void Awake()
    {
        i = this;
    }

    private static Vector3 axis;
	private static Vector3 start_point;
    private static Vector3 current_point;
    private static int segments_left = 0;
	private static bool around_the_world = false;
    public static C.Colors WaveColor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
			Vector3 p_start = new Vector3 (0, 1, 0);
			Vector3 p_end = new Vector3 (-1f, 0, 0);
			SendWave(p_start, p_end);
            //StartCoroutine(Test_PositionOnWave());
        }
    }

    public void SendWave()
    {
		Vector3 p_start = new Vector3 (0, 1, 0);
		Vector3 p_end = new Vector3 (-1.0f,0 , 0);
		around_the_world = true;
        StartWave(p_start, p_end);
        StartCoroutine(createDelayed(C.WAVE_DELAY));
    }

    public void SendWave( Vector3 p_start, Vector3 p_end )
    {
        StartWave( p_start, p_end );
        StartCoroutine( createDelayed( C.WAVE_DELAY ) );
    }

    bool isPositionInWave(Vector3 pos)
    {

        bool res = false;
        float ang1 = GetAngleFrom2Points(start_point, pos);
        float ang2 = GetAngleFrom2Points(pos, current_point);
        //Debug.Log("Angles:" + ang1 + ", " + ang2);
        if (ang1 < ang2)
        {
            res = true;
        }
        //Debug.Log("res:" + res.ToString());
        return res;
    }

    private static void StartWave( Vector3 p_start, Vector3 p_end )
    {
		axis = Vector3.forward;

		float angle = 360;
		if (!around_the_world) 
		{
			angle = GetAngleFrom2Points (p_start, p_end);
		}

		if (C.WAVE_DIRECTION == 1) 
		{
			axis = axis * -1;
		}

		//Debug.Log ("angle: " + angle);
		segments_left = Mathf.Abs(Mathf.FloorToInt(angle / C.WaveSegmentSizeAngle));

        current_point = p_start.normalized * C.WORLD_RADIUS;
		start_point = current_point;

    }

    private static void DrawSegment()
    {
        int reps = 0;

		if (segments_left > C.WAVE_MOVE_RATE)
		{
			segments_left = segments_left - C.WAVE_MOVE_RATE;
			reps = C.WAVE_MOVE_RATE;
		}
		else
        {
            reps = segments_left;
            segments_left = 0;
        }
		float angle_offset = C.WaveSegmentSizeAngle;
        for (int i = 0; i < reps; i++)
        {
            //Debug.Log ("angle: " + (C.WaveSegmentSizeAngle * i) + " pos- x: " + pos.x + " y:" + pos.y);
            GameObject wave_segment = (GameObject)Instantiate(C.i.WavePrefab, current_point, Quaternion.identity);
			wave_segment.transform.RotateAround(C.i.WorldCenter.position, axis, angle_offset);
			current_point = wave_segment.transform.position;
			//angle_offset += C.WaveSegmentSizeAngle;
            wave_segment.transform.up = (C.i.WorldCenter.transform.position - wave_segment.transform.position) * -1;
            //current_point = wave_segment.transform.position;

            SpriteRenderer s = wave_segment.GetComponent<SpriteRenderer>();
            s.color = C.GetRealColor(WaveColor);

        }
    }

    private static float GetAngleFrom2Points( Vector3 p_start, Vector3 p_end )
    {
		float deg = Vector3.Angle (p_start, p_end);
        float ang_dir = -p_start.x * p_end.y + p_start.y * p_end.x;
        if (ang_dir == -1)
        {
            deg = 360 - deg;
        }
        //Debug.Log("deg ang:" + deg);
		//deg = 360 - deg;
		return deg;
    }

	private static Vector3 GetDestPointFromAngleAndPoint(float angle, Vector3 p_start){
       
		return new Vector3 (
			C.i.WorldCenter.position.x + (float)(C.WORLD_RADIUS * Mathf.Cos( angle)),
			C.i.WorldCenter.position.y + (float)(C.WORLD_RADIUS * Mathf.Sin( angle)),
			0.0f
				);
	}

    private IEnumerator createDelayed(float delay)
    {
		while ( WaveController.segments_left  > 0)
        {
			yield return new WaitForSeconds(delay);
			//Debug.Log("running delayed");
			DrawSegment();
		}
    }

    private IEnumerator Test_PositionOnWave()
    {
        while (segments_left > 0)
        {
            yield return new WaitForSeconds(0.1f);
            Test_PointsOnArc();
        }
    }

    private void Test_PointsOnArc()
    {
        bool pos1 = isPositionInWave(new Vector3(-1, -1, 0));
        //Debug.Log("pos1:" + pos1.ToString());
        //bool pos2 = isPositionInWave(new Vector3(1, 0, 0));
        //bool pos3 = isPositionInWave(new Vector3(0, -1, 0));
        //bool pos4 = isPositionInWave(new Vector3(-1, 0, 0));

        //Debug.Log("pos1:" + pos1.ToString() + " pos2:" + pos2.ToString() + " pos3:" + pos3.ToString() + " pos4:" + pos4.ToString());
    }


    private void Test_Angles()
    {
        float ang1 = GetAngleFrom2Points(new Vector3(0, 1, 0), new Vector3(1, 0, 0));
        Debug.Log("ang1:" + ang1);

        float ang2 = GetAngleFrom2Points(new Vector3(0, 1, 0), new Vector3(0, -1, 0));
        Debug.Log("ang2:" + ang2);

        float ang3 = GetAngleFrom2Points(new Vector3(0, 1, 0), new Vector3(-1, 0, 0));
        Debug.Log("ang3:" + ang3);

        float ang4 = GetAngleFrom2Points(new Vector3(1, 0, 0), new Vector3(-1, 0, 0));
        Debug.Log("ang4:" + ang4);
    }

}
