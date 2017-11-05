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
	public GameObject growthMarkerPrefab;
	public List<GameObject> growthMarkers;
	public List<Mesh> stemMeshes;

	// Use this for initialization
	void Start () {
		Regrow();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Regrow()
	{
		StopAllCoroutines();
		foreach (GameObject marker in growthMarkers)
		{
			GameObject.Destroy(marker);
		}
		growthMarkers = new List<GameObject>();
		stemMeshes = new List<Mesh>();
		growthMarkers.Add(Instantiate(growthMarkerPrefab, transform.position, transform.rotation));
		stemMeshes.Add(new Mesh());
		StartCoroutine(GrowStem(growthMarkers[0], stemMeshes[0]));
		growthMarkers.Add(Instantiate(growthMarkerPrefab, transform.position, transform.rotation));
		stemMeshes.Add(new Mesh());
		StartCoroutine(GrowStem(growthMarkers[1], stemMeshes[1]));
	}

	private void CreateMainMesh()
	{
		int vertCount = 0;
		int triCount = 0;
		foreach (Mesh mesh in stemMeshes)
		{
			vertCount += mesh.vertexCount;
			Debug.Log(mesh.vertexCount);
			triCount += mesh.triangles.Length;
		}
		Vector3[] verts = new Vector3[vertCount];
		int[] triangles = new int[triCount];

		int vertIndex = 0;
		int triIndex = 0;
		foreach (Mesh mesh in stemMeshes)
		{
			int vertLength = mesh.vertexCount;
			int triLength = mesh.triangles.Length;
			for (int i = 0; i < vertLength; i++)
			{
				verts[vertIndex + i] = mesh.vertices[i];
			}
			for (int i = 0; i < triLength; i++)
			{
				triangles[triIndex + i] = mesh.triangles[i] + vertIndex;
			}
			vertIndex += vertLength;
			triIndex += triLength;
		}

		Mesh mainMesh = new Mesh();
		mainMesh.vertices = verts;
		mainMesh.triangles = triangles;
		mainMesh.RecalculateNormals();
		MeshFilter filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = mainMesh;
	}

	private void SplitStem()
	{
		
	}

	private IEnumerator GrowStem(GameObject marker, Mesh mesh)
	{
		float growthStep = height / joints;
		List<Vector3> stemPoints = new List<Vector3>();
		List<Quaternion> stemRotations = new List<Quaternion>();
		stemPoints.Add(marker.transform.position);
		stemRotations.Add(Quaternion.identity);

		for (int i = 0; i < joints; i++)
		{
			yield return new WaitForSeconds(growthTime / joints);

			Vector3 forwardDirection = marker.transform.TransformDirection(Vector3.up);
			marker.transform.position += forwardDirection * growthStep;
			stemPoints.Add(marker.transform.position);
			stemRotations.Add(marker.transform.rotation);
			//Rotate growth marker randomly
			float rotX = Random.Range(-deviation, deviation);
			float rotY = 0f; //Better looking results when not rotating around y axis
			float rotZ = Random.Range(-deviation, deviation);
			marker.transform.Rotate(rotX, rotY, rotZ);
			int meshIndex = stemMeshes.IndexOf(mesh);
			Debug.Log(meshIndex);
			mesh = PlantMeshGenerator.GenerateMesh(stemPoints.ToArray(), stemRotations.ToArray(), radius);
			stemMeshes[meshIndex] = mesh;
			CreateMainMesh();
		}
	}
}
