using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{
	[HideInInspector] public Vector3 initDir = Vector3.zero;
	
	struct WallHit
	{
		public Vector3 point;
		public Vector3 normal;
		
		public WallHit(Vector3 p, Vector3 n)
		{
			point = p;
			normal = n;
		}
	};
	
	List<WallHit> wallHits = new List<WallHit> ();
	
	int numHits = 0;
	[HideInInspector] public int maxHits = 10;
	
	public bool fromCam = false;
	
	[SerializeField] float lineDrawTimeFactor = 0.05f;
	
	LineRenderer lr;
	AudioSource hitSounder;
	ParticleSystem hitEffect;
	
	Transform hitEffectObj;
	
	[HideInInspector]
	public Color 	startColor,
					endColor;
	
	// Use this for initialization
	void Start ()
	{
		if (!lr)
			lr = GetComponent<LineRenderer> ();
		
		hitEffectObj = transform.GetChild(0);
		hitSounder = hitEffectObj.GetComponent<AudioSource> ();
		hitEffect = hitEffectObj.GetComponent<ParticleSystem> ();
		
		CalculateLaser ();

		StartCoroutine (DrawLaser ());
	}
	
	public void RandomColor()
	{
		if (!lr)
			lr = GetComponent<LineRenderer> ();

		startColor = (Color)((Vector4)Random.insideUnitSphere) + Color.gray;
		endColor = (Color)((Vector4)Random.insideUnitSphere) + Color.gray;
		lr.SetColors (startColor, endColor);
	}

	public void SetColors()
	{
		if (!lr)
			lr = GetComponent<LineRenderer> ();

		lr.SetColors (startColor, endColor);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	// Cast a ray towards the original direction
	// Recursively follow that path until nothing is hit or you've reflected X times
	void CalculateLaser()
	{
		Vector3 currentDir = initDir;
		Vector3 currentPoint = transform.position;
		
		bool stopLaser = false;
		
		RaycastHit hit;
		while(numHits < maxHits && !stopLaser)
		{
			if(Physics.Raycast(currentPoint, currentDir, out hit))
			{
				wallHits.Add(new WallHit(hit.point, hit.normal));
				
				switch(hit.transform.tag)
				{
				case "NoReflect":
					break;
				case "Split":
					SplitLaser(hit, currentDir);
					stopLaser = true;
					break;
				default:
					break;
				}
				
				currentPoint = hit.point;
				currentDir = Vector3.Reflect(currentDir, hit.normal);

				numHits++;
			}
			else if(!stopLaser)
			{
				if((fromCam && numHits > 0) || !fromCam)
				{
					print ("HIT");
					wallHits.Add(new WallHit(currentDir + currentDir * Camera.main.farClipPlane, hit.normal));
				}
				
				break;
			}
		}
	}
	
	void SplitLaser(RaycastHit hit, Vector3 dir)
	{
//		if (!hit.transform.GetComponent<Splitter> ().active)
//			return;
		
		Vector3 normalReflect = Vector3.Reflect (dir, hit.normal);
		Vector3 splitDir = (hit.normal + normalReflect).normalized;
		Vector3 laserSpawn = hit.point + hit.normal * WadeUtils.KINDASMALLNUM;
		LaserMaker.singleton.instance.ExtendLaser(laserSpawn, splitDir, (maxHits - numHits)/2, false, startColor, endColor);
		splitDir = -Vector3.Reflect (splitDir, normalReflect);
		LaserMaker.singleton.instance.ExtendLaser(laserSpawn, splitDir, (maxHits - numHits)/2, false, startColor, endColor);
	}
	
	// Use the data gathered from CalculateLaser to draw the path of the laser at the desired speed
	IEnumerator DrawLaser()
	{
		Vector3 drawPos = transform.position;
		
		for(int i = 0; i < wallHits.Count; i++)
		{
			float lineDrawTimer = 0.0f;
			float vertDistance = Vector3.Distance(drawPos, wallHits[i].point);
			
			Vector3 lineInitPos = drawPos;
			
			if(fromCam)
				lr.SetVertexCount(i + 1);
			else
			{
				lr.SetVertexCount(i + 2);
				lr.SetPosition(0, transform.position);
			}
			
			if(i == 0 && fromCam)
			{
				drawPos = wallHits[i].point;
				lr.SetPosition(i, drawPos);
			}
			else
			{
				while(vertDistance > WadeUtils.SMALLNUM)
				{
					drawPos = Vector3.Lerp(lineInitPos, wallHits[i].point, lineDrawTimer/(vertDistance * lineDrawTimeFactor));
					lineDrawTimer += Time.deltaTime;
					
					if(!fromCam)
						lr.SetPosition(i + 1, drawPos);
					else
						lr.SetPosition(i, drawPos);
					
					vertDistance = Vector3.Distance(drawPos, wallHits[i].point);
					
					yield return 0;
				}
			}
			
			hitEffectObj.transform.position = wallHits[i].point;
			hitEffectObj.transform.rotation = Quaternion.FromToRotation(Vector3.up,wallHits[i].normal);
			hitEffect.Play();
			
			hitSounder.pitch = Random.Range(0.9f, 1.1f);
			hitSounder.Play();
			
			yield return 0;
		}
		
		yield return new WaitForSeconds (1);
		Destroy (hitEffectObj.gameObject);
	}
}

