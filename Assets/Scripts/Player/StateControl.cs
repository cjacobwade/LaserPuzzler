using UnityEngine;
using System.Collections;

public class StateControl : MonoBehaviour 
{
	public static SingletonBehaviour<StateControl> singleton = new SingletonBehaviour<StateControl>();

	[SerializeField] LookController lookController;
	[SerializeField] OverheadController overheadController;

	[SerializeField] Camera uiCamera;
	[SerializeField] LayerMask uiLayer;

	bool lookMode = false;

	// Use this for initialization
	void Awake () 
	{
		singleton.DontDestroyElseKill (this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
		#elif UNITY_ANDROID || UNITY_IPHONE
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		#endif
		{
			RaycastHit hit;
			if(Physics.Raycast(uiCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, uiLayer))
			{
				if(hit.transform.tag == "LookButton")
					lookMode = true;
			}
		}

		#if UNITY_EDITOR
		if(Input.GetMouseButtonUp(0))
		#elif UNITY_ANDROID || UNITY_IPHONE
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		#endif
		{
			lookMode = false;
		}

		if(lookMode)
		{
			lookController.enabled = true;
			overheadController.enabled = false;
			PlayerNav.singleton.instance.ClearPath ();
		}
		else
		{
			overheadController.enabled = true;
			lookController.enabled = false;
		}
	}
}
