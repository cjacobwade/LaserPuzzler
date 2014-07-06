using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CamLookAt : MonoBehaviour 
{
	[SerializeField] Transform target;

	// Update is called once per frame
	void Update () 
	{
		transform.LookAt (target);
	}
}
