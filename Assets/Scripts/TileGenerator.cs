using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("Parameters")]
    public int noiseSampleSize;
    public float scale;
    public float maxHeight = 1f;
    public int textureResolution = 1;

    [HideInInspector]
    public Vector2 offset;

    [Header("Terrain Types")]
    public TerrainType[] heightTerrainTypes;
    public TerrainType[] heatTerrainTypes;

    [Header("Waves")]
    public Wave[] waves;
    public Wave[] heatWaves;

    [Header("Curves")]
    public AnimationCurve heightCurve;

    private MeshRenderer tileMeshRender;
    private MeshFilter tileMeshFilter;
    private MeshCollider tileMeshCollider;

    private MeshGenerator meshGenerator;
    private MapGenerator mapGenerator;

    private void Start()
    {
        tileMeshRender = GetComponent<MeshRenderer>();
        tileMeshFilter = GetComponent<MeshFilter>();
        tileMeshCollider = GetComponent<MeshCollider>();

        meshGenerator = GetComponent<MeshGenerator>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        
        GenerateTile();
    }

    private void GenerateTile()
    {
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, waves, offset);

        float[,] hdHeightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize - 1, scale, waves, offset, textureResolution);

        Vector3[] verts = tileMeshFilter.mesh.vertices;

        for (int x = 0; x < noiseSampleSize; x++)
        {
            for (int z = 0; z < noiseSampleSize; z++)
            {
                int index = (x * noiseSampleSize) + z;

                verts[index].y = heightCurve.Evaluate(heightMap[x, z]) * maxHeight;
            }
        }

        tileMeshFilter.mesh.vertices = verts;
        tileMeshFilter.mesh.RecalculateBounds();
        tileMeshFilter.mesh.RecalculateNormals();

        tileMeshCollider.sharedMesh = tileMeshFilter.mesh;

        Texture2D heightMapTexture = TextureBuilder.BuildTexture(hdHeightMap, heightTerrainTypes);

        //tileMeshRender.material.mainTexture = heightMapTexture;

        float[,] heatMap = GenerateHeatMap(heightMap);
        tileMeshRender.material.mainTexture = TextureBuilder.BuildTexture(heightMap, heatTerrainTypes);

    }

    float[,] GenerateHeatMap(float[,] heightMap)
    {
        float[,] uniformHeatMap = NoiseGenerator.GenerateUniformNoiseMap(noiseSampleSize,
            transform.position.z * (noiseSampleSize / meshGenerator.xSize), (noiseSampleSize / 2 * mapGenerator.numX) + 1);
        float[,] randomHeatMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, heatWaves, offset);

        float[,] heatMap = new float[noiseSampleSize, noiseSampleSize];

        for (int x = 0; x < noiseSampleSize; x++)
        {
            for (int z = 0; z < noiseSampleSize; z++)
            {
                heatMap[x, z] = randomHeatMap[x, z] * uniformHeatMap[x, z];
                heatMap[x, z] += 0.5f * heightMap[x, z];

                heatMap[x, z] = Mathf.Clamp(heatMap[x, z], 0f, 0.99f);
            }
        }

        return heatMap;
    }
}

[System.Serializable]
public class TerrainType
{
    [Range(0f, 1f)]
    public float threshold;
    public Gradient colorGradient;
}
