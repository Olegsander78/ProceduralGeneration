using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator2
{
    // generates a new noise map based on a number of parameters
    // returns a 2D float array
    public static float[,] GenerateNoiseMap (int noiseSampleSize, float scale, Wave[] waves, Vector2 offset, int resolution = 1)
    {
        // create the 2D float array
        float[,] noiseMap = new float[noiseSampleSize * resolution, noiseSampleSize * resolution];

        for(int x = 0; x < noiseSampleSize * resolution; x++)
        {
            for(int y = 0; y < noiseSampleSize * resolution; y++)
            {
                // get an X and Y position to know where we're going to sample in perlin noise
                float samplePosX = ((float)x / scale / (float)resolution) + offset.y;
                float samplePosY = ((float)y / scale / (float)resolution) + offset.x;

                float noise = 0.0f;
                float normalization = 0.0f;

                // apply the various different waves to add in varied terrain
                foreach(Wave wave in waves)
                {
                    noise += wave.amplitude * Mathf.PerlinNoise(samplePosX * wave.frequency + wave.seed, samplePosY * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }

                noise /= normalization;

                noiseMap[x, y] = noise;
            }
        }

        return noiseMap;
    }

    public static float[,] GenerateUniformNoiseMap (int size, float vertexOffset, float maxVertexDistance)
    {
        float[,] noiseMap = new float[size, size];

        for(int x = 0; x < size; x++)
        {
            float xSample = x + vertexOffset;
            float noise = Mathf.Abs(xSample) / maxVertexDistance;

            for(int z = 0; z < size; z++)
            {
                noiseMap[x, size - z - 1] = noise;
            }
        }

        return noiseMap;
    }
}

[System.Serializable]
public class Wave
{
    public float seed;
    public float frequency;
    public float amplitude;
}