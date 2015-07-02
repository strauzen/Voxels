using UnityEngine;
using System.Collections;

public struct WorldPos
{
	public int x, y, z;

	public WorldPos(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	// Override of equals to make it more efficient
	public override bool Equals(object obj) {
		if (!(obj is WorldPos))
			return false;

		WorldPos pos = (WorldPos)obj;
		if (pos.x != this.x || pos.y != this.y || pos.z != this.z) {
			return false;
		} else {
			return true;
		}
	}
}