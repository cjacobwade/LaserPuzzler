using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverheadController : MonoBehaviour 
{
	Camera overheadCam;
	Vector3 initOffset;

	[SerializeField] Transform player;
	[SerializeField] float lerpSpeed;
	[SerializeField] LayerMask navLayer;

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
			#if UNITY_EDITOR
			if(Input.GetMouseButtonDown(0))
			#elif UNITY_ANDROID || UNITY_IPHONE
			if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			#endif
			{
				Vector3 targetPos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
				
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.transform.position, targetPos, out hit, navLayer))
					PlayerNav.singleton.instance.SetDestination(hit.point);
			}

			transform.position = Vector3.Lerp(transform.position, player.position - initOffset, Time.deltaTime * lerpSpeed );
		}
	}
}
