using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{

    public Material cubeMaterial;
    public Block[,,] chunkData;
    public GameObject chunk;

    void BuildChunk()
    {
        chunkData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];
        //create blocks
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);

					int worldX = (int)(x+chunk.transform.position.x);
					int worldY = (int)(y+chunk.transform.position.y);
					int worldZ = (int)(z+chunk.transform.position.z);

                    if (worldY <= Utils.GenerateHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos, chunk.gameObject, this);
                    else
                        chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos, chunk.gameObject, this);
                }
            }
        }
    }

    public void DrawChunk()
    {
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();
                }
            }
        }
        CombineQuads();
    }

    public Chunk(Vector3 position, Material c)
    {
        chunk = new GameObject(World.BuildChunkName(position));
        chunk.transform.position = position;
        cubeMaterial = c;
        BuildChunk();
    }

    void CombineQuads()
    {
        MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].mesh;
            // transforme les (0.5f, 0.5f, 0.5f) qui sont en local en worldmatrix
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        MeshFilter mf = chunk.gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        mf.mesh.CombineMeshes(combine);

        MeshRenderer renderer = chunk.gameObject.AddComponent<MeshRenderer>();
        renderer.material = cubeMaterial;

        for (int i = 0; i < chunk.transform.childCount; i++)
        {
            GameObject.Destroy(chunk.transform.GetChild(i).gameObject, 0.01f);
        }
    }
}
