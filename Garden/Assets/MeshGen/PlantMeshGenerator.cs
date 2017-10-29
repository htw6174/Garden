using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlantMeshGenerator {

	static int circumferencePoints = 50;

	public static Mesh GenerateMesh(Vector3[] centers, Vector3[] rotations, float stemRadius)
	{
		Mesh stem = StemMesh(centers, rotations, stemRadius);

		return stem;
	}

	private static Mesh StemMesh(Vector3[] centers, Vector3[] rotations, float radius)
	{
		//Start by generating a cylinder

		//Verticies
		//Start with a point at (0, 0, 0) with a rotation of (-90, 0, 0)
		//Move forward one unit and place another point
		//Rotate slightly in every direction
		//Repeat
		Vector3[] verts = new Vector3[circumferencePoints * centers.Length];
		for (int i = 0; i < centers.Length; i++)
		{
			float centerX = centers[i].x;
			float centerY = centers[i].y;
			float centerZ = centers[i].z;
			float rotX = rotations[i].x;
			float rotY = rotations[i].y;
			float rotZ = rotations[i].z;
			float sinX = Mathf.Sin(rotX);
			float cosX = Mathf.Cos(rotX);
			float sinY = Mathf.Sin(rotY);
			float cosY = Mathf.Cos(rotY);
			float sinZ = Mathf.Sin(rotZ);
			float cosZ = Mathf.Cos(rotZ);

			Vector3 xAxis = new Vector3(
				cosY * cosZ,
				cosX * sinZ + sinX * sinY * cosZ,
				sinX * sinZ - cosX * sinY * cosZ
			);
			Vector3 yAxis = new Vector3(
				-cosY * sinZ,
				cosX * cosZ - sinX * sinY * sinZ,
				sinX * cosZ + cosX * sinY * sinZ
			);
			Vector3 zAxis = new Vector3(
				sinY,
				-sinX * cosY,
				cosX * cosY
			);

			for (int j = 0; j < circumferencePoints; j++)
			{
				float theta = 2 * Mathf.PI * ((float) j / circumferencePoints);
				float pointX = Mathf.Sin(theta);
				float pointY = 0f;
				float pointZ = Mathf.Cos(theta);
				//float pointX = (-Mathf.Cos(theta) * normals[i].z) + (Mathf.Sin(theta) * normals[i].y);
				//float pointY = (-Mathf.Cos(theta) * normals[i].z) + (-Mathf.Sin(theta) * normals[i].x);
				//float pointZ = (-Mathf.Sin(theta) * normals[i].x) + (Mathf.Cos(theta) * normals[i].y);
				Vector3 point = xAxis * pointX + yAxis * pointY + zAxis * pointZ;
				//point = Vector3.ProjectOnPlane(point, normals[i]);
				verts[(i * circumferencePoints) + j] = (point * radius) + centers[i];
			}
		}

		//Triangles
		int[] tris = new int[2 * 3 * circumferencePoints * (centers.Length - 1)];
		int t = 0;
		for (int i = 0; i < centers.Length - 1; i++)
		{
			int a, b, c, d;
			for (int j = 0; j < circumferencePoints - 1; j++)
			{
				a = (i * circumferencePoints) + j;
				b = a + 1;
				c = a + circumferencePoints;
				d = c + 1;
				tris[t] = a;
				tris[t + 1] = b;
				tris[t + 2] = c;
				tris[t + 3] = c;
				tris[t + 4] = b;
				tris[t + 5] = d;
				t += 6;
			}
			a = (i * circumferencePoints) + circumferencePoints - 1;
			b = (i * circumferencePoints);
			c = a + circumferencePoints;
			d = b + circumferencePoints;
			tris[t] = a;
			tris[t + 1] = b;
			tris[t + 2] = c;
			tris[t + 3] = c;
			tris[t + 4] = b;
			tris[t + 5] = d;
			t += 6;
		}

		//Mesh
		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.RecalculateNormals();

		return mesh;
	}
}
