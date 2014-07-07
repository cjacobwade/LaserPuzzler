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
		transform.rotation = transform.parent.rotation;
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
			Vector3 acceleration = Input.acceleration;
			acceleration.y = (acceleration.y + 0.5f) * 2;

			transform.rotation *= Quaternion.Euler (rotSpeed.y * acceleration.y, rotSpeed.x * acceleration.x, 0);
			transform.eulerAngles = Vector3.Scale (transform.eulerAngles, new Vector3 (1, 1, 0));
		}
	}
}
