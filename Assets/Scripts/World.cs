﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	// Dictionary to store Chunks
	public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();

	// GameObject that stores chunks
	public GameObject chunkPrefab;

	// Use this for initialization
	void Start () {
		// Creating some chunks to test the world Script
		for (int x = -2; x < 2; x++) {
			for (int y = 0; y < 1; y++) {
				for (int z = 0; z < 1; z++) {
					CreateChunk (x * Chunk.chunkSize, y * Chunk.chunkSize, z * Chunk.chunkSize);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateChunk(int x, int y, int z) {
		WorldPos worldPos = new WorldPos (x, y, z);

		// Instantiate the chunk at the coordinates using the chunk prefab
		GameObject newChunkObject = Instantiate (chunkPrefab, 
		                                         new Vector3 (x, y, z), 
		                                         Quaternion.Euler (Vector3.zero)
		                                         ) as GameObject;

		Chunk newChunk = newChunkObject.GetComponent<Chunk> ();

		newChunk.position = worldPos;
		newChunk.world = this;

		// Add the chunk to the dictionary with its position as key
		chunks.Add (worldPos, newChunk);

		// Chunk construction
		for (int xi = 0; xi < 16; xi++)
		{
			for (int yi = 0; yi < 16; yi++)
			{
				for (int zi = 0; zi < 16; zi++)
				{
					if (yi <= 7)
					{
						SetBlock(x+xi, y+yi, z+zi, new BlockGrass());
					}
					else
					{
						SetBlock(x + xi, y + yi, z + zi, new BlockAir());
					}
				}
			}
		}
	}

	public Chunk GetChunk(int x, int y, int z) {
		WorldPos pos = new WorldPos ();

		// Variable to operate division as float
		float multiple = Chunk.chunkSize;

		// We divide to get the coord as ints then multiply to get a clean x,y,z of the chunk we are in.
		pos.x = Mathf.FloorToInt (x / multiple) * Chunk.chunkSize;
		pos.y = Mathf.FloorToInt (y / multiple) * Chunk.chunkSize;
		pos.z = Mathf.FloorToInt (z / multiple) * Chunk.chunkSize;

		Chunk containerChunk = null;

		chunks.TryGetValue (pos, out containerChunk);

		return containerChunk;
	}

	public void DestroyChunk(int x, int y, int z) {
		Chunk chunk = null;
		if (chunks.TryGetValue (new WorldPos (x, y, z), out chunk)) {
			Object.Destroy(chunk.gameObject);
			chunks.Remove(new WorldPos (x, y, z));
		}
	}

	public Block GetBlock(int x, int y, int z) {
		Chunk containerChunk = GetChunk (x, y, z);

		if (containerChunk != null) {
			Block block = containerChunk.GetBlock (x - containerChunk.position.x, 
			                                      y - containerChunk.position.y, 
			                                      z - containerChunk.position.z);

			return block;
		} else {
			return new BlockAir();
		}
	}

	public void SetBlock(int x, int y, int z, Block block) {
		Chunk chunk = GetChunk(x, y, z);

		if (chunk != null) {
			chunk.SetBlock(x - chunk.position.x, y - chunk.position.y, z - chunk.position.z, block);
			chunk.update = true;
		}

		//Add these lines line
		UpdateIfEqual(x - chunk.position.x, 0, new WorldPos(x - 1, y, z));
		UpdateIfEqual(x - chunk.position.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
		UpdateIfEqual(y - chunk.position.y, 0, new WorldPos(x, y - 1, z));
		UpdateIfEqual(y - chunk.position.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
		UpdateIfEqual(z - chunk.position.z, 0, new WorldPos(x, y, z - 1));
		UpdateIfEqual(z - chunk.position.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
	}

	void UpdateIfEqual(int value1, int value2, WorldPos pos) {
		if (value1 == value2) {
			Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
			if (chunk != null)
				chunk.update = true;
		}
	}
}