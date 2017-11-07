using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StemMeshGenerator {

	static int circumferencePoints = 50;

	public static Mesh GenerateMesh(Vector3[] centers, Quaternion[] rotations, float stemRadius)
	{
		Mesh stem = StemMesh(centers, rotations, stemRadius);

		return stem;
	}

	private static Mesh StemMesh(Vector3[] centers, Quaternion[] rotations, float radius)
	{
		//Start by generating a cylinder

		//Verticies
		Vector3[] verts = new Vector3[circumferencePoints * centers.Length];
		for (int i = 0; i < centers.Length; i++)
		{
			for (int j = 0; j < circumferencePoints; j++)
			{
				float theta = 2 * Mathf.PI * ((float) j / circumferencePoints);
				//Find point on flat ring
				float pointX = Mathf.Sin(theta);
				float pointY = 0f;
				float pointZ = Mathf.Cos(theta);
				//Rotate point by the rotation of the growth marker
				Vector3 point = rotations[i] * new Vector3(pointX, pointY, pointZ);
				//Factor in radius and offset
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
