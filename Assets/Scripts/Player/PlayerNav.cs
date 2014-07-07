using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNav : MonoBehaviour 
{
	public static SingletonBehaviour<PlayerNav> singleton = new SingletonBehaviour<PlayerNav>();
	NavMeshAgent nmAgent;

	// Use this for initialization
	void Awake () 
	{
		singleton.DontDestroyElseKill (this);
		nmAgent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void SetDestination(Vector3 pos)
	{
		nmAgent.SetDestination (pos);
	}

	public void ClearPath()
	{
		nmAgent.Stop ();
		nmAgent.SetDestination (transform.position);
	}
}
