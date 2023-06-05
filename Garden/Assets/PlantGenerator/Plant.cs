using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public float growthTime = 1f;
	public int age = 0; //Grow a new set of branches if age % branchDistance is 0
	public float height = 10;
	public int joints = 50;
	public int jointDistance = 10;
	public float radius = 1;
	public float deviation = 2f;
	public int branchDistance = 200; //Distance between each set of side branches
	public float posOfLastBranch = 0f; //Length the trunk was at when the last branch sprouted
	public PlantStem stemPrefab;

	private PlantStem trunk;
	public List<PlantStem> branches;

	// Use this for initialization
	void Start () {
		Regrow();
	}
	
	// Update is called once per frame
	void Update () {
		// if (trunk != null && trunk.growing)
		// {
		// 	if (trunk.length - posOfLastBranch >= branchDistance)
		// 	{
		// 		GrowSideBranches();
		// 	}
		// }
		Grow(1);
	}

	private void GrowSideBranches()
	{
		float branchHeight = height / 3;
		float branchRadius = radius / 2;
		Vector3 startPosition = trunk.growthMarker.transform.position;
		Quaternion startRotation = trunk.growthMarker.transform.rotation; //Using this as the starting rotation is broken for some reason
		PlantStem sb1 = Instantiate(stemPrefab, startPosition, Quaternion.identity, trunk.transform);
		PlantStem sb2 = Instantiate(stemPrefab, startPosition, Quaternion.identity, trunk.transform);
		branches.Add(sb1);
		branches.Add(sb2);
		sb1.Regrow(growthTime, branchHeight, branchRadius, deviation, joints);
		sb2.Regrow(growthTime, branchHeight, branchRadius, deviation, joints);
		sb1.transform.Rotate(0f, 0f, 15f);
		sb2.transform.Rotate(0f, 0f, -15f);
		posOfLastBranch = trunk.length;
	}

	public void Grow(int time)
	{
		age += time;
		if (age % branchDistance == 0)
		{
			GrowSideBranches();
		}
		trunk.Grow(time);
		foreach (PlantStem branch in branches)
		{
			branch.Grow(time);
		}
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
