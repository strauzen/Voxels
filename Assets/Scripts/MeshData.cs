// MeshData.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData {

	public List<Vector3> vertices = new List<Vector3> ();
	public List<int> triangles = new List<int> ();
	public List<Vector2> uv = new List<Vector2> ();

	public bool useRenderDataForCol;
	public List<Vector3> colliderVertices = new List<Vector3> ();
	public List<int> colliderTriangles = new List<int> ();

	public MeshData() { }

	public void AddVertex(Vector3 vertex) {
		vertices.Add (vertex);

		if (useRenderDataForCol) {
			colliderVertices.Add(vertex);
		}
	}

	public void AddTriangle(int triangle) {
		triangles.Add (triangle);

		if (useRenderDataForCol) {
			colliderTriangles.Add(triangle - (vertices.Count - colliderVertices.Count));
		}
	}

	public void AddQuadTriangles() {
		// Triangle 1 of the Quad
		triangles.Add (vertices.Count - 4);
		triangles.Add (vertices.Count - 3);
		triangles.Add (vertices.Count - 2);
		// Triangle 2 of the Quad
		triangles.Add (vertices.Count - 4);
		triangles.Add (vertices.Count - 2);
		triangles.Add (vertices.Count - 1);

		if (useRenderDataForCol) {
			// Triangle 1 of the collider Quad
			colliderTriangles.Add (colliderVertices.Count - 4);
			colliderTriangles.Add (colliderVertices.Count - 3);
			colliderTriangles.Add (colliderVertices.Count - 2);
			// Triangle 2 of the collider Quad
			colliderTriangles.Add (colliderVertices.Count - 4);
			colliderTriangles.Add (colliderVertices.Count - 2);
			colliderTriangles.Add (colliderVertices.Count - 1);
		}
	}
}
