using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNav : MonoBehaviour 
{
	public static SingletonBehaviour<PlayerNav> singleton = new SingletonBehaviour<PlayerNav>();

	[SerializeField] Transform player;
	NavMeshAgent nmAgent;

	// Use this for initialization
	void Awake () 
	{
		singleton.DontDestroyElseKill (this);
		nmAgent = player.GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector3 targetPos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;

			RaycastHit hit;
			if(Physics.Raycast(Camera.main.transform.position, targetPos, out hit))
			{
				nmAgent.SetDestination(hit.point);
			}
		}
	}

	public void ClearPath()
	{
		nmAgent.Stop ();
		nmAgent.SetDestination (transform.position);
	}
}
