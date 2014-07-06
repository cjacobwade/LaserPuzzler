using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserMaker : MonoBehaviour 
{	
	public static SingletonBehaviour<LaserMaker> singleton = new SingletonBehaviour<LaserMaker>();
	
	public GameObject laserPrefab;
	List<GameObject> lasers = new List<GameObject>();
	
	void Awake()
	{
		singleton.DontDestroyElseKill (this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
			CreateLaserWithColor(transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, 10, true);
	}
	
	public GameObject CreateLaser(Vector3 pos, Vector3 dir, int maxHits, bool fromCam)
	{
		GameObject laserObj = (GameObject)Instantiate (laserPrefab, pos, Quaternion.identity);
		Laser laser = laserObj.GetComponent<Laser> ();
		
		laser.initDir = dir;
		laser.maxHits = maxHits;
		laser.fromCam = fromCam;
		lasers.Add (laserObj);
		
		return laserObj;
	}
	
	public GameObject CreateLaserWithColor(Vector3 pos, Vector3 dir, int maxHits, bool fromCam)
	{
		GameObject laserObj = CreateLaser(pos, dir, maxHits, fromCam);
		laserObj.GetComponent<Laser>().RandomColor ();

		return laserObj;
	}
	
	public GameObject ExtendLaser(Vector3 pos, Vector3 dir, int maxHits, bool fromCam, Color startColor, Color endColor)
	{
		GameObject laserObj = CreateLaser (pos, dir, maxHits, fromCam);
		Laser laser = laserObj.GetComponent<Laser> ();
		laser.startColor = endColor;
		laser.endColor = startColor;
		laser.SetColors();

		return laserObj;
	}
}
