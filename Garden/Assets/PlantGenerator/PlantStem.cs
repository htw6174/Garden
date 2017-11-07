using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlantStem : MonoBehaviour {

	public bool growing; //Is the stem currently in the progress of growing?
	public float growthTime = 0f;
	public float currentGrowthTime;
	public GameObject growthMarkerPrefab;
	public GameObject growthMarker; //Use the local position of this to create new stems or leaves as child objects of the stem
	public Mesh mesh;
	private MeshFilter filter;

	public float length = 0; //Updated as stem grows; length in local units of the stem if it were straightened out

	// Use this for initialization
	void Start () {
		filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
	}
	
	// Update is called once per frame
	void Update () {
		if (growing)
		{
			GrowStem();
		}
	}

	public void Regrow(float duration, float height = 10f, float radius = 1f, float deviation = 10f, int joints = 50)
	{
		StopAllCoroutines();
		GameObject.Destroy(growthMarker);

		growing = true;
		growthTime = duration;
		currentGrowthTime = 0f;
		transform.localScale = Vector3.one * 0.001f;

		growthMarker = Instantiate(growthMarkerPrefab, transform);
		mesh = new Mesh();
		StartCoroutine(LenghtenStem(growthTime, height, radius, deviation, joints));
	}

	private void GrowStem()
	{
		currentGrowthTime += Time.deltaTime;
		float growthPercentage = currentGrowthTime / growthTime;
		transform.localScale = Vector3.one * Mathf.Min(1f, growthPercentage);
	}

	private IEnumerator LenghtenStem(float growthTime, float height, float radius, float deviation, int joints)
	{
		float segmentLength = height / joints;
		float stepTime = growthTime / joints;
		length = 0f;
		List<Vector3> stemPoints = new List<Vector3>(); //Add slider
		List<Quaternion> stemRotations = new List<Quaternion>();
		stemPoints.Add(growthMarker.transform.localPosition);
		stemRotations.Add(Quaternion.identity);

		for (int i = 0; i < joints; i++)
		{
			yield return new WaitForSeconds(stepTime);

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
		growing = false;
	}
}
