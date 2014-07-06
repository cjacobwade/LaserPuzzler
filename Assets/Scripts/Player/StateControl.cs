using UnityEngine;
using System.Collections;

public class StateControl : MonoBehaviour 
{
	public static SingletonBehaviour<StateControl> singleton = new SingletonBehaviour<StateControl>();

	[SerializeField] LookController lookController;
	[SerializeField] OverheadController overheadController;

	bool lookMode = false;

	// Use this for initialization
	void Awake () 
	{
		singleton.DontDestroyElseKill (this);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnGUI()
	{
		lookMode = GUI.Toggle (new Rect (Screen.width * 7/10, Screen.height * 7/10, Screen.width * 2.5f/10, Screen.height * 2.5f/10), lookMode, "LOOK");
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
