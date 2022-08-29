using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator 
{
    public static float[,] GenerateNoiseMap(int noiseSampleSize, float scale)
    {
        float[,] noiseMap = new float[noiseSampleSize, noiseSampleSize];

        for (int x = 0; x < noiseSampleSize; x++)
        {
            for(int y = 0; y < noiseSampleSize; y++)
            {
                float samplePosX = (float)x / scale;
                float samplePosY = (float)y / scale;

                noiseMap[x, y] = Mathf.PerlinNoise(samplePosX, samplePosY);
            }
        }

        return noiseMap;
    }
}
