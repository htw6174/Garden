using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlantMeshTester : MonoBehaviour {

	public float growthTime = 1f;
	public float height = 10;
	public int joints = 50;
	public float radius = 1;
	public float deviation = 2f;
	public GameObject growthMarker;

	private List<Vector3> stemPoints = new List<Vector3>();
	private List<Vector3> stemRotations = new List<Vector3>();

	// Use this for initialization
	void Start () {
		Regrow();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Regrow()
	{
		growthMarker.transform.position = transform.position;
		growthMarker.transform.rotation = transform.rotation;
		StartCoroutine(GrowStem());
	}

	private IEnumerator GrowStem()
	{
		float growthStep = height / joints;
		stemPoints = new List<Vector3>();
		stemRotations = new List<Vector3>();
		stemPoints.Add(growthMarker.transform.position);
		stemRotations.Add(Vector3.zero);

		for (int i = 0; i < joints; i++)
		{
			yield return new WaitForSeconds(growthTime / joints);

			Vector3 forwardDirection = growthMarker.transform.TransformDirection(Vector3.up);
			growthMarker.transform.position += forwardDirection * growthStep;
			stemPoints.Add(growthMarker.transform.position);
			//Vector3 direction = stemPoints[i + 1] - stemPoints[i];
			stemRotations.Add(growthMarker.transform.eulerAngles * Mathf.Deg2Rad);

			float rotX = Random.Range(-deviation, deviation);
			float rotY = Random.Range(-deviation, deviation);
			float rotZ = Random.Range(-deviation, deviation);
			growthMarker.transform.Rotate(rotX, rotY, rotZ);
			Mesh mesh = PlantMeshGenerator.GenerateMesh(stemPoints.ToArray(), stemRotations.ToArray(), radius);
			MeshFilter filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
			filter.mesh = mesh;
		}
	}
}
