//Chunk.cs
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour {

	public static int chunkSize = 16;
	public bool update = true;

	MeshFilter filter;
	MeshCollider coll;

	public World world;
	public WorldPos position;


	private Block[ , , ] blocks = new Block[chunkSize,chunkSize,chunkSize];

	// Use this for initialization
	void Start () {
		filter = gameObject.GetComponent<MeshFilter> ();
		coll = gameObject.GetComponent<MeshCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (update) {
			update = false;
			UpdateChunk();
		}
	}

	// Block getter
	public Block GetBlock(int x, int y, int z) {
		if (InRange(x) && InRange(y) && InRange(z))
			return blocks [x, y, z];
		return world.GetBlock (position.x + x, position.y + y, position.z + z);
	}

	public void SetBlock(int x, int y, int z, Block block) {
		if (InRange (x) && InRange (y) && InRange (z)) {
			blocks [x, y, z] = block;
		} else {
			world.SetBlock(position.x + x, position.y + y, position.z + z, block);
		}
	}

	// Function to check if a block belongs to this Chunk
	public static bool InRange(int index) {
		if (index < 0 || index >= chunkSize)
			return false;
		return true;
	}

	// Updates the chunk based on its contents
	void UpdateChunk() {
		MeshData meshData = new MeshData ();

		for (int x = 0; x < chunkSize; x++) {
			for (int y = 0; y < chunkSize; y++) {
				for (int z = 0; z < chunkSize; z++){
					meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
				}
			}
		}

		RenderMesh (meshData);
	}

	// Sends the calculated mesh information 
	// to the mesh and collision components
	void RenderMesh(MeshData meshData) {
		// Reset the mesh and the collider mesh
		filter.mesh.Clear ();
		coll.sharedMesh = null;

		// Mesh polygons
		filter.mesh.vertices = meshData.vertices.ToArray ();
		filter.mesh.triangles = meshData.triangles.ToArray ();

		// Mesh UVs and Normals
		filter.mesh.uv = meshData.uv.ToArray ();
		filter.mesh.RecalculateNormals ();

		// Mesh collisions
		Mesh mesh = new Mesh ();
		mesh.vertices = meshData.colliderVertices.ToArray ();
		mesh.triangles = meshData.colliderTriangles.ToArray ();
		mesh.RecalculateNormals ();
		coll.sharedMesh = mesh;
	}
}
