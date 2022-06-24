using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using LibNoise.Generator;
using LibNoise.Operator;
using LibNoise;


public class MapGen : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] Tilemap walkableTilemap;
    [SerializeField] Tilemap unwaklableTilemap;
    [SerializeField] Tilemap treeTilemap;

    [SerializeField] TileBase[] tiles;
    [SerializeField] TileBase[] treeTiles;

    [SerializeField] float noiseScale = 10f;

    [Range(0f, 1f)]
    [SerializeField] float treeChance;


    [SerializeField] double displacement = 4;
    [SerializeField] double frequency = 2;
    [SerializeField] int seed = 0;
    [SerializeField] int resolution;


    // Start is called before the first frame update
    void Start()
    {
        Generate();
        
    }

    public void Generate()
    {
        ModuleBase moduleBase = new Voronoi(frequency, displacement, seed, false);
        Noise2D noise = new Noise2D(resolution, resolution, moduleBase);
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int tileIndex = GetTileIDFromNoise(x, y, noise);

                if (tileIndex == 0)
                    unwaklableTilemap.SetTile(new Vector3Int(x, y, 0), tiles[tileIndex]);
                else
                {
                    walkableTilemap.SetTile(new Vector3Int(x, y, 0), tiles[tileIndex]);
                }
            }
        }
    }

    int GetTileIDFromNoise(int x, int y, Noise2D noise)
    {
        
        // Using perlin Noise
        //float xCord = (float)x / width;
        //float yCord = (float)y / height;
        //float sample = Mathf.PerlinNoise(xCord * noiseScale, yCord * noiseScale);

        // Voronoi
        float sample = noise.GetTexture().GetPixel(x, y).r;
        
        
        Debug.Log(sample);
        sample *= tiles.Length;
        
        return Mathf.FloorToInt(sample);
    }
}
