using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

	public Material cubeMaterial;

	IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ)
	{
		for (int z = 0; z < sizeX; z++)
		{
			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					Vector3 pos = new Vector3(x,y,z);
					Block b = new Block(Block.BlockType.DIRT, pos, this.gameObject, cubeMaterial);
					b.Draw();
					yield return null;
				}
			}
		}
		CombineQuads();
	}

	void CombineQuads()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].mesh;
            // transforme les (0.5f, 0.5f, 0.5f) qui sont en local en worldmatrix
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        mf.mesh.CombineMeshes(combine);

        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.material = cubeMaterial;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject, 0.01f);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
