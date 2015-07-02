using UnityEngine;
using System.Collections;

public class Block {

	// Offset from the center of the Block, it is used to calculate vertexes. i.e. 0.5 means blocks of size 1;
	public const float verticeOffset = 0.5f;
	// 1 divided by the number of tiles per side
	const float tileSize = 0.25f;

	// Block faces directions
	public enum Direction {	north, east, south, west, up, down };

	// Position of the texture in the tileset
	public struct Tile { public int x; public int y; }

	// Base Block constructor
	public Block() {
	
	}

	public virtual MeshData Blockdata (Chunk chunk, int x, int y, int z, MeshData meshData){

		// Check if block on top has a solid down face
		if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down)) {
			meshData = FaceDataUp(chunk, x, y, z, meshData);
		}

		// Check if the block below has a solid up face
		if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up)) {
			meshData = FaceDataDown(chunk, x, y, z, meshData);
		}

		// Check if the block north has a solid south face
		if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south)) {
			meshData = FaceDataNorth(chunk, x, y, z, meshData);
		}

		// Check if the block south has a solid north face
		if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north)) {
			meshData = FaceDataSouth(chunk, x, y, z, meshData);
		}

		// Check if the block east has a solid west face
		if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west)) {
			meshData = FaceDataEast(chunk, x, y, z, meshData);
		}

		// Check if the block west has a solid east face
		if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east)) {
			meshData = FaceDataWest(chunk, x, y, z, meshData);
		}

		return meshData;
	}

	public virtual bool IsSolid(Direction direction) {
		switch (direction) {
		case Direction.north:
			return true;
		case Direction.east:
			return true;
		case Direction.south:
			return true;
		case Direction.west:
			return true;
		case Direction.up:
			return true;
		case Direction.down:
			return true;
		}
		return false;
	}

	public virtual Tile TexturePosition(Direction direction) {
		Tile tile = new Tile ();
		tile.x = 0;
		tile.y = 0;

		return tile;
	}

	// Calculates the vertexes of a tile of a tileset. The tile is selected depending on direction. 
	// The size is determined by the const value tileSize
	public virtual Vector2[] FaceUVs(Direction direction) {
		Vector2[] UVs = new Vector2[4];
		Tile tilePos = TexturePosition (direction);

		UVs [0] = new Vector2 (tileSize * tilePos.x + tileSize, tileSize * tilePos.y);
		UVs [1] = new Vector2 (tileSize * tilePos.x + tileSize, tileSize * tilePos.y + tileSize);
		UVs [2] = new Vector2 (tileSize * tilePos.x, tileSize * tilePos.y + tileSize);
		UVs [3] = new Vector2 (tileSize * tilePos.x, tileSize * tilePos.y);

		return UVs;
	}

	protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData){

		meshData.AddVertex (new Vector3(x - verticeOffset, y + verticeOffset, z + verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y + verticeOffset, z + verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y + verticeOffset, z - verticeOffset));
		meshData.AddVertex (new Vector3(x - verticeOffset, y + verticeOffset, z - verticeOffset));

		meshData.AddQuadTriangles();

		meshData.uv.AddRange (FaceUVs(Direction.up));

		return meshData;
	}

	protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData){

		meshData.AddVertex (new Vector3(x - verticeOffset, y - verticeOffset, z - verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y - verticeOffset, z - verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y - verticeOffset, z + verticeOffset));
		meshData.AddVertex (new Vector3(x - verticeOffset, y - verticeOffset, z + verticeOffset));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange (FaceUVs(Direction.down));

		return meshData;
	}

	protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData){

		meshData.AddVertex (new Vector3(x + verticeOffset, y - verticeOffset, z + verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y + verticeOffset, z + verticeOffset));
		meshData.AddVertex (new Vector3(x - verticeOffset, y + verticeOffset, z + verticeOffset));
		meshData.AddVertex (new Vector3(x - verticeOffset, y - verticeOffset, z + verticeOffset));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange (FaceUVs(Direction.north));

		return meshData;
	}

	protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData){

		meshData.useRenderDataForCol = true;

		meshData.AddVertex (new Vector3(x - verticeOffset, y - verticeOffset, z - verticeOffset));
		meshData.AddVertex (new Vector3(x - verticeOffset, y + verticeOffset, z - verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y + verticeOffset, z - verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y - verticeOffset, z - verticeOffset));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange (FaceUVs(Direction.south));

		return meshData;
	}

	protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData){

		meshData.AddVertex (new Vector3(x + verticeOffset, y - verticeOffset, z - verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y + verticeOffset, z - verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y + verticeOffset, z + verticeOffset));
		meshData.AddVertex (new Vector3(x + verticeOffset, y - verticeOffset, z + verticeOffset));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange (FaceUVs(Direction.east));

		return meshData;
	}

	protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData){

		meshData.AddVertex (new Vector3(x - verticeOffset, y - verticeOffset, z + verticeOffset));
		meshData.AddVertex (new Vector3(x - verticeOffset, y + verticeOffset, z + verticeOffset));
		meshData.AddVertex (new Vector3(x - verticeOffset, y + verticeOffset, z - verticeOffset));
		meshData.AddVertex (new Vector3(x - verticeOffset, y - verticeOffset, z - verticeOffset));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange (FaceUVs(Direction.west));

		return meshData;
	}

}
