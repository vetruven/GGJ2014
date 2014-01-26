using UnityEngine;
using System.Collections;

public class PlayerAnchor : MonoBehaviour {

	public float rotateForce = -2000f;



	void Update () {
		if (Input.GetKey(KeyCode.R))
		{
			RotateForward(rotateForce);
		}
	
	}

	public void RotateForward(float rForce)
	{
		transform.Rotate(new Vector3(0,0,rForce*Time.deltaTime));
	}
}
