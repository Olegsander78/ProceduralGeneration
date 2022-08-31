using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomeType
{
    Desert,
    Tundra,
    Savanna,
    Forest,
    Rainforest
}

public class BiomeBuilder : MonoBehaviour
{
    public Biome[] biomes;
    public BiomeRow[] tableRows;

    public static BiomeBuilder instance;

    void Awake()
    {
        instance = this;
    }

    public Texture2D BuildTexture(TerrainType[,] heatMapTypes, TerrainType[,] moistureMapTypes)
    {
        int size = heatMapTypes.GetLength(0);
        Color[] pixels = new Color[size * size];

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                int index = (x * size) + z;

                int heatMapIndex = heatMapTypes[x, z].index;
                int moistureMapIndex = moistureMapTypes[x, z].index;

                Biome biome = null;

                foreach (Biome b in biomes)
                {
                    if (b.type == tableRows[moistureMapIndex].tableColumns[heatMapIndex])
                    {
                        biome = b;
                        break;
                    }
                }

                pixels[index] = biome.color;
            }
        }

        Texture2D texture = new Texture2D(size, size);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }

    public Biome GetBiome(TerrainType heatTerrainType, TerrainType moistureTerrainType)
    {
        foreach (Biome b in biomes)
        {
            if (b.type == tableRows[moistureTerrainType.index].tableColumns[heatTerrainType.index])
            {
                return b;
            }
        }

        return null;
    }
}

[System.Serializable]
public class BiomeRow
{
    public BiomeType[] tableColumns;
}

[System.Serializable]
public class Biome
{
    public BiomeType type;
    public Color color;
    public bool spawnPrefabs;
    public GameObject[] spawnablePrefabs;
    [Range(0.0f, 3.0f)]
    public float density = 1.0f;
}