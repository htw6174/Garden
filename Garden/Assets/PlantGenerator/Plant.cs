using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public float growthTime = 1f;
	public float height = 10;
	public int joints = 50;
	public float radius = 1;
	public float deviation = 2f;
	public float branchDistance = 2f; //Distance between each set of side branches
	public float posOfLastBranch = 0f; //Length the trunk was at when the last branch sprouted
	public PlantStem stemPrefab;
	public GameObject leafPrefab; //change to type of leaf class later

	private PlantStem trunk;

	// Use this for initialization
	void Start () {
		//Regrow();
	}
	
	// Update is called once per frame
	void Update () {
		if (trunk != null && trunk.growing)
		{
			if (trunk.length - posOfLastBranch >= branchDistance)
			{
				GrowSideBranches();
			}
		}
	}

	private void GrowSideBranches()
	{
		float branchHeight = height / 2;
		float branchRadius = radius / 2;
		Vector3 startPosition = trunk.growthMarker.transform.position;
		Quaternion startRotation = trunk.growthMarker.transform.rotation; //Using this as the starting rotation is broken for some reason
		PlantStem sb1 = Instantiate(stemPrefab, startPosition, Quaternion.identity, trunk.transform);
		PlantStem sb2 = Instantiate(stemPrefab, startPosition, Quaternion.identity, trunk.transform);
		sb1.Regrow(growthTime, branchHeight, branchRadius, deviation, joints);
		sb2.Regrow(growthTime, branchHeight, branchRadius, deviation, joints);
		sb1.transform.Rotate(0f, 0f, 15f);
		sb2.transform.Rotate(0f, 0f, -15f);
		posOfLastBranch = trunk.length;
	}

	public void Regrow()
	{
		posOfLastBranch = 0f;
		//Grow a central trunk (stem)
		trunk = Instantiate(stemPrefab, transform);
		trunk.Regrow(growthTime, height, radius, deviation, joints);
		//Grow side branches (stems) during trunk growth
		//Grow leaves on branches during branch growth
	}
}
