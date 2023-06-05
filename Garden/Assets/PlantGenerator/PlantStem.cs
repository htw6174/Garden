using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlantStem : MonoBehaviour {

	public bool growing; //Is the stem currently in the progress of growing?
	public float growthTime = 0f;
	public float age = 0f;
	public float currentGrowthTime;
	public GameObject growthMarkerPrefab;
	public GameObject growthMarker; //Use the local position of this to create new stems or leaves as child objects of the stem
	public Mesh mesh;
	public float radius;
	public int maxSegments = 50;
	public int segmentAgeThreshold = 50;
	private MeshFilter filter;
	private List<Vector3> stemPoints;
	private List<Quaternion> stemRotations;

	public float length = 0; //Updated as stem grows; length in local units of the stem if it were straightened out
	private float segmentLength;
	private float deviation;

	// Use this for initialization
	void Start () {
		filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
	}
	
	// Update is called once per frame
	void Update () {
		// if (growing)
		// {
		// 	GrowStem();
		// }
	}

	public void Grow(int time)
	{
		age += time;
		//transform.localScale += Vector3.one * 0.001f;
		if (stemPoints.Count < maxSegments && age % segmentAgeThreshold == 0)
		{
			AddSegment();
		}
	}

	public void Regrow(float duration, float height = 10f, float radius = 1f, float deviation = 10f, int joints = 50)
	{
		GameObject.Destroy(growthMarker);

		growing = true;
		growthTime = duration;
		segmentLength = height / joints;
		this.deviation = deviation;
		this.radius = radius;
		length = 0f;
		currentGrowthTime = 0f;
		stemPoints = new List<Vector3>(); //Add slider
		stemRotations = new List<Quaternion>();
		//transform.localScale = Vector3.one * 0.001f;

		growthMarker = Instantiate(growthMarkerPrefab, transform);
		mesh = new Mesh();
	}

	private void AddSegment()
	{
		Vector3 forwardDirection = growthMarker.transform.TransformDirection(Vector3.up);
		growthMarker.transform.localPosition += forwardDirection * segmentLength;
		length += segmentLength;
		stemPoints.Add(growthMarker.transform.localPosition);
		stemRotations.Add(growthMarker.transform.localRotation);
		//Rotate growth marker randomly
		float rotX = Random.Range(-deviation, deviation);
		float rotY = 0f; //Better looking results when not rotating around y axis
		float rotZ = Random.Range(-deviation, deviation);
		growthMarker.transform.Rotate(rotX, rotY, rotZ);
		mesh = StemMeshGenerator.GenerateMesh(stemPoints.ToArray(), stemRotations.ToArray(), radius);
		filter.mesh = mesh;
	}
}
