using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverheadController : MonoBehaviour 
{
	Camera overheadCam;
	Vector3 initOffset;

	[SerializeField] Transform player;
	[SerializeField] float lerpSpeed;

	void Awake()
	{
		if(!overheadCam)
			overheadCam = GetComponent<Camera>();

		initOffset = player.position - transform.position;
	}
	
	// Use this for initialization
	void OnEnable () 
	{
		overheadCam.enabled = true;
	}
	
	void OnDisable()
	{
		overheadCam.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(overheadCam)
		{
			transform.position = Vector3.Lerp(transform.position, player.position - initOffset, Time.deltaTime * lerpSpeed );
		}
	}
}
