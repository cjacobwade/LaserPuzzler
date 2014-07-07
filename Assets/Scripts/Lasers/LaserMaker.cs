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
		Vector3 laserDir = Vector3.zero;

		#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
			laserDir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
		#elif UNITY_ANDROID || UNITY_IPHONE
		if(Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)
			laserDir = Camera.main.ScreenPointToRay(Input.GetTouch(1).position).direction;
		#endif

		CreateLaserWithColor(transform.position, laserDir, 10, true);
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
