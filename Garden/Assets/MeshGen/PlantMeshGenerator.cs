using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlantMeshGenerator {

	static int circumferencePoints = 50;
	static int heightPoints = 50;

	public static Mesh GenerateMesh(float height, float stemRadius, float deviation)
	{
		Mesh stem = StemMesh(height, stemRadius, deviation);

		return stem;
	}

	private static Mesh StemMesh(float height, float radius, float deviation = 0f)
	{
		//Start by generating a cylinder
		float heightStep = height / heightPoints;

		//Verticies
		//Start with a point at (0, 0, 0) with a rotation of (-90, 0, 0)
		//Move forward one unit and place another point
		//Rotate slightly in every direction
		//Repeat
		Vector3[] points = new Vector3[circumferencePoints * heightPoints];
		float pointY = 0f;
		float centerX = 0f;
		float centerZ = 0f;
		Vector3 rotation = new Vector3(-90, 0, 0);
		for (int i = 0; i < heightPoints; i++)
		{
			//Shift center of stem
			centerX += Random.Range(-deviation, deviation);
			centerZ += Random.Range(-deviation, deviation);
			for (int j = 0; j < circumferencePoints; j++)
			{
				float iterToRadians = 2 * Mathf.PI * ((float) j / circumferencePoints);
				float pointX = Mathf.Sin(iterToRadians) * radius + centerX;
				float pointZ = Mathf.Cos(iterToRadians) * radius + centerZ;
				points[(i * circumferencePoints) + j] = new Vector3(pointX, pointY, pointZ);
			}
			pointY += heightStep;
		}

		//Triangles
		int[] triangles = new int[2 * 3 * circumferencePoints * (heightPoints - 1)];
		int t = 0;
		for (int i = 0; i < heightPoints - 1; i++)
		{
			int a, b, c, d;
			for (int j = 0; j < circumferencePoints - 1; j++)
			{
				a = (i * circumferencePoints) + j;
				b = a + 1;
				c = a + circumferencePoints;
				d = c + 1;
				triangles[t] = a;
				triangles[t + 1] = b;
				triangles[t + 2] = c;
				triangles[t + 3] = c;
				triangles[t + 4] = b;
				triangles[t + 5] = d;
				t += 6;
			}
			a = (i * circumferencePoints) + circumferencePoints - 1;
			b = (i * circumferencePoints);
			c = a + circumferencePoints;
			d = b + circumferencePoints;
			triangles[t] = a;
			triangles[t + 1] = b;
			triangles[t + 2] = c;
			triangles[t + 3] = c;
			triangles[t + 4] = b;
			triangles[t + 5] = d;
			t += 6;
		}

		//Mesh
		Mesh mesh = new Mesh();
		mesh.vertices = points;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		return mesh;
	}
}
