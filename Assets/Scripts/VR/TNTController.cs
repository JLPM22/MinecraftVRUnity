using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusModules.Grab;

public class TNTController : MonoBehaviour
{
    public AudioClip TNTSound;
    public int ExplosionRadius = 2;
    public GameObject DirtParticleSystem;
    public GameObject StoneParticleSystem;

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
            Vector3 pos = other.contacts[0].point;
            GetChunkVoxel(pos, out Chunk selectedChunk, out Vector3Int selectedVoxel);
            if (selectedChunk != null)
            {
                // Destroy blocks
                for (int y = -ExplosionRadius; y <= ExplosionRadius; ++y)
                {
                    for (int z = -ExplosionRadius; z <= ExplosionRadius; ++z)
                    {
                        for (int x = -ExplosionRadius; x <= ExplosionRadius; ++x)
                        {
                            Vector3 offset = new Vector3(x, y, z);
                            if (offset.sqrMagnitude < ExplosionRadius * ExplosionRadius)
                            {
                                GetChunkVoxel(pos + offset, out Chunk chunk, out Vector3Int voxel);
                                if (chunk != null)
                                {
                                    // Particle System
                                    Block block = chunk.GetBlock(voxel.x, voxel.y, voxel.z);
                                    if (block == Block.DIRT || block == Block.GRASS) Instantiate(DirtParticleSystem, pos + offset, Quaternion.identity);
                                    else if (block == Block.STONE || block == Block.COBBLESTONE) Instantiate(StoneParticleSystem, pos + offset, Quaternion.identity);
                                    // Set to AIR
                                    chunk.SetBlock(voxel.x, voxel.y, voxel.z, Block.AIR);
                                }
                            }
                        }
                    }
                }
                // Sound
                AudioSource.PlayOneShot(TNTSound, 2);
                GetComponentInChildren<MeshRenderer>().enabled = false;
                GameObject.Destroy(gameObject, 2.0f);
                Destroying = true;
            }
        }
    }

    private void GetChunkVoxel(Vector3 pos, out Chunk chunk, out Vector3Int voxel)
    {
        int chunkX = Mathf.FloorToInt(pos.x / Chunk.ChunkSize.x);
        int chunkY = Mathf.FloorToInt(pos.y / Chunk.ChunkSize.y);
        int chunkZ = Mathf.FloorToInt(pos.z / Chunk.ChunkSize.z);
        chunk = ChunkManager.Instance.GetChunk(chunkX, chunkY, chunkZ);
        if (chunk != null)
        {
            int z = pos.z >= 0.0f ? (int)pos.z : (int)pos.z - 1;
            int y = pos.y >= 0.0f ? (int)pos.y : (int)pos.y - 1;
            int x = pos.x >= 0.0f ? (int)pos.x : (int)pos.x - 1;
            voxel = new Vector3Int((x % Chunk.ChunkSize.x + Chunk.ChunkSize.x) % Chunk.ChunkSize.x, // To allow negative coordinates
                                                      (y % Chunk.ChunkSize.y + Chunk.ChunkSize.y) % Chunk.ChunkSize.y,
                                                      (z % Chunk.ChunkSize.z + Chunk.ChunkSize.z) % Chunk.ChunkSize.z);
        }
        else voxel = new Vector3Int();
    }
}
