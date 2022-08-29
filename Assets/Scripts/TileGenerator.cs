using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("Parameters")]
    public int noiseSampleSize;
    public float scale;

    private void Start()
    {
        GenerateTile();
    }

    private void GenerateTile()
    {
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale);
    }
}
