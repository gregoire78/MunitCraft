﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuads3 : MonoBehaviour
{
    public Material cubeMaterial;
    public BlockType btype;
    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };
    public enum BlockType { GRASS, DIRT, STONE };

    Vector2[,] blockUVs = {
        /*GRASS*/      {new Vector2(0.125f,0.375f), new Vector2(0.1875f,0.375f), new Vector2(0.125f, 0.4375f), new Vector2(0.1875f,0.4375f)},
        /*GRASS SIDE*/ {new Vector2(0.1875f,0.9375f), new Vector2(0.25f,0.9375f), new Vector2(0.1875f, 1.0f), new Vector2(0.25f,1.0f)},
        /*DIRT*/       {new Vector2(0.125f,0.9375f), new Vector2(0.1875f,0.9375f), new Vector2(0.125f, 1.0f), new Vector2(0.1875f,1.0f)},
        /*STONE*/      {new Vector2(0, 0.875f), new Vector2(0.0625f,0.875f), new Vector2(0, 0.9375f), new Vector2(0.0625f,0.9375f)}
    };



    void CreateQuad(Cubeside side)
    {
        Mesh mesh = new Mesh();
        mesh.name = side.ToString();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        //all possible UVS
        Vector2 uv00;
        Vector2 uv10;
        Vector2 uv01;
        Vector2 uv11;

        if (btype == BlockType.GRASS && side == Cubeside.TOP)
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }
        else if ((btype == BlockType.GRASS && side == Cubeside.BOTTOM) || btype == BlockType.DIRT)
        {
            uv00 = blockUVs[2, 0];
            uv10 = blockUVs[2, 1];
            uv01 = blockUVs[2, 2];
            uv11 = blockUVs[2, 3];
        }
        else if (btype == BlockType.GRASS)
        {
            uv00 = blockUVs[1, 0];
            uv10 = blockUVs[1, 1];
            uv01 = blockUVs[1, 2];
            uv11 = blockUVs[1, 3];
        }
        else
        {
            uv00 = blockUVs[3, 0];
            uv10 = blockUVs[3, 1];
            uv01 = blockUVs[3, 2];
            uv11 = blockUVs[3, 3];
        }

        //all possible vertices
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        switch (side)
        {
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 0, 2, 1, 0, 3, 2 };
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 0, 2, 1, 0, 3, 2 };
                break;
            case Cubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 0, 2, 1, 0, 3, 2 };
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 0, 2, 1, 0, 3, 2 };
                break;
            case Cubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 0, 2, 1, 0, 3, 2 };
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 0, 2, 1, 0, 3, 2 };
                break;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("quad");
        quad.transform.parent = transform;
        MeshFilter meshFilter = quad.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
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

    void CreateCube()
    {
        CreateQuad(Cubeside.FRONT);
        CreateQuad(Cubeside.BACK);
        CreateQuad(Cubeside.TOP);
        CreateQuad(Cubeside.BOTTOM);
        CreateQuad(Cubeside.LEFT);
        CreateQuad(Cubeside.RIGHT);
        CombineQuads();
    }

    // Use this for initialization
    void Start()
    {
        CreateCube();
    }
}
