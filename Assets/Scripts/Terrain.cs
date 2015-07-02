using UnityEngine;
using System.Collections;

public static class Terrain {

	public static WorldPos GetBlockPos(Vector3 position) {
		WorldPos blockPos = new WorldPos (
			Mathf.RoundToInt(position.x),
			Mathf.RoundToInt(position.y),
			Mathf.RoundToInt(position.z)
			);
		return blockPos;
	}

	public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false) {
		Vector3 position = new Vector3 (
			MoveWithinBlock (hit.point.x, hit.normal.x, adjacent),
			MoveWithinBlock (hit.point.y, hit.normal.y, adjacent),
			MoveWithinBlock (hit.point.z, hit.normal.z, adjacent)
		);
		return GetBlockPos(position);
	}

	static float MoveWithinBlock(float position, float normal, bool adjacent = false) {
		if (position - (int)position == Block.verticeOffset || position - (int)position == -Block.verticeOffset) {
			if (adjacent) {
				position += (normal / 2);
			} else {
				position -= (normal / 2);
			}
		}

		return (float)position;
	}

	public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false) {
		Chunk chunk = hit.collider.GetComponent<Chunk> ();
		if (chunk == null)
			return false;

		WorldPos position = GetBlockPos (hit, adjacent);

		chunk.world.SetBlock(position.x, position.y, position.z, block);

		return true;
	}

	public static Block GetBlock(RaycastHit hit, bool adjacent = false) {
	
		Chunk chunk = hit.collider.GetComponent<Chunk>();
		if (chunk == null)
			return null;

		WorldPos position = GetBlockPos (hit, adjacent);

		Block block = chunk.world.GetBlock (position.x, position.y, position.z);

		return block;
	}
}
