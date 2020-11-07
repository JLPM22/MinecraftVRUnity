using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusModules.Grab;

public class BlockColliderAdd : MonoBehaviour
{
    public Block Block;
    public AudioClip PlaceSound;

    private Collider Collider;
    private Grabbable Grabbable;
    private AudioSource AudioSource;

    private int TerrainLayer;
    private bool Destroying;

    private void Awake()
    {
        Grabbable = GetComponent<Grabbable>();
        AudioSource = GetComponent<AudioSource>();
        Collider = GetComponentInChildren<Collider>();
        TerrainLayer = LayerMask.NameToLayer("Terrain");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Destroying && !Grabbable.IsGrabbed && other.gameObject.layer == TerrainLayer)
        {
            Vector3 pos = other.contacts[0].point + other.contacts[0].normal * 0.5f;
            int chunkX = Mathf.FloorToInt(pos.x / Chunk.ChunkSize.x);
            int chunkY = Mathf.FloorToInt(pos.y / Chunk.ChunkSize.y);
            int chunkZ = Mathf.FloorToInt(pos.z / Chunk.ChunkSize.z);
            Chunk selectedChunk = ChunkManager.Instance.GetChunk(chunkX, chunkY, chunkZ);
            if (selectedChunk != null)
            {
                int z = pos.z >= 0.0f ? (int)pos.z : (int)pos.z - 1;
                int y = pos.y >= 0.0f ? (int)pos.y : (int)pos.y - 1;
                int x = pos.x >= 0.0f ? (int)pos.x : (int)pos.x - 1;
                Vector3Int selectedVoxel = new Vector3Int((x % Chunk.ChunkSize.x + Chunk.ChunkSize.x) % Chunk.ChunkSize.x, // To allow negative coordinates
                                                          (y % Chunk.ChunkSize.y + Chunk.ChunkSize.y) % Chunk.ChunkSize.y,
                                                          (z % Chunk.ChunkSize.z + Chunk.ChunkSize.z) % Chunk.ChunkSize.z);
                Block block = selectedChunk.GetBlock(selectedVoxel.x, selectedVoxel.y, selectedVoxel.z);
                if (block == Block.AIR)
                {
                    selectedChunk.SetBlock(selectedVoxel.x, selectedVoxel.y, selectedVoxel.z, Block);
                    // Sound
                    AudioSource.PlayOneShot(PlaceSound, 4);
                    GameObject.Destroy(gameObject, 0.5f);
                    Destroying = true;
                }
            }
        }
    }
}
