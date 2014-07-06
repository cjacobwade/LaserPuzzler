using UnityEngine;
using System.Collections;

public class LookController : MonoBehaviour 
{
	[SerializeField] Vector2 rotSpeed;
	LaserMaker lm;
	Camera lookCam;

	void Awake()
	{
		if(!lookCam)
			lookCam = GetComponent<Camera>();
		if(!lm)
			lm = GetComponent<LaserMaker>();
	}

	// Use this for initialization
	void OnEnable () 
	{
		lookCam.enabled = true;
		lm.enabled = true;
	}

	void OnDisable()
	{
		lookCam.enabled = false;
		lm.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(lookCam)
		{
			transform.rotation *= Quaternion.Euler (rotSpeed.y * Input.acceleration.y, rotSpeed.x * Input.acceleration.x, 0);
			transform.eulerAngles = Vector3.Scale (transform.eulerAngles, new Vector3 (1, 1, 0));
		}
	}
}
