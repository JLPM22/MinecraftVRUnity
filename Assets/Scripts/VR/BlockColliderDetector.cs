using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusModules.Grab;

public class BlockColliderDetector : MonoBehaviour
{
    public BoxCollider[] PickColliders;
    public AudioClip PickaxeSound;
    public GameObject DirtParticleSystem;
    public GameObject StoneParticleSystem;

    private Grabbable Grabbable;
    private AudioSource AudioSource;

    private int TerrainLayer;

    private void Awake()
    {
        Grabbable = GetComponent<Grabbable>();
        AudioSource = GetComponent<AudioSource>();
        TerrainLayer = LayerMask.NameToLayer("Terrain");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Grabbable.IsGrabbed && other.gameObject.layer == TerrainLayer)
        {
            for (int i = 0; i < PickColliders.Length; ++i)
            {
                Vector3 pos = PickColliders[i].bounds.center;
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
                    if (block != Block.AIR)
                    {
                        selectedChunk.SetBlock(selectedVoxel.x, selectedVoxel.y, selectedVoxel.z, Block.AIR);
                        // Sound
                        AudioSource.PlayOneShot(PickaxeSound);
                        if (block == Block.DIRT || block == Block.GRASS) Instantiate(DirtParticleSystem, pos, Quaternion.identity);
                        else if (block == Block.STONE || block == Block.COBBLESTONE) Instantiate(StoneParticleSystem, pos, Quaternion.identity);
                    }
                }
            }
        }
    }
}
