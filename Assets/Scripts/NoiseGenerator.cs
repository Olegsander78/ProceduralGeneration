using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator 
{
    public static float[,] GenerateNoiseMap(int noiseSampleSize, float scale, int resolution = 1)
    {
        float[,] noiseMap = new float[noiseSampleSize * resolution, noiseSampleSize * resolution];

        for (int x = 0; x < noiseSampleSize * resolution; x++)
        {
            for (int y = 0; y < noiseSampleSize * resolution; y++)
            {
                float samplePosX = (float)x / scale / (float)resolution;
                float samplePosY = (float)y / scale / (float)resolution;

                noiseMap[x, y] = Mathf.PerlinNoise(samplePosX, samplePosY);
            }
        }

        return noiseMap;
    }
}
