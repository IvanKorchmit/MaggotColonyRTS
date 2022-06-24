using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using LibNoise.Generator;
using LibNoise.Operator;
using LibNoise;


public class MapGen : MonoBehaviour
{
    [System.Serializable]
    public struct NoiseTile
    {
        [Range(0f, 1f)]
        public float value;
        public TileBase tile;
        public Tilemap tilemap;
        public bool isGrass;
    }


    [SerializeField] float riverThreshold;



    [SerializeField] private Gradient grassColor;
    [SerializeField] private int width, height;
    [SerializeField] private Tilemap treeTilemap;
    [NonReorderable]
    [SerializeField] private NoiseTile[] tiles;
    [SerializeField] private NoiseTile defaultTile;
    [NonReorderable]
    [SerializeField] private TileBase[] treeTiles;
    [Range(0f, 1f)]
    [SerializeField] private float treeChance;


    [SerializeField] double frequency = 2;
    [SerializeField] double riverFrequency = 2;


    // Start is called before the first frame update
    void Start()
    {
        Generate();
        
    }

    public void Generate()
    {
        ModuleBase moduleBase = new Voronoi();
        ModuleBase riverNoise = new RidgedMultifractal();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                NoiseTile tile = GetTileFromNoise(x,y, moduleBase, out double samp);
                tile.tilemap.SetTile(pos, tile.tile);   
                if (tile.isGrass)
                {
                    tile.tilemap.SetTileFlags(pos, TileFlags.None);
                    tile.tilemap.SetColor(pos, grassColor.Evaluate((float)samp));
                }
                GenerateRiver(x, y, riverNoise);
            }
        }
    }
    private void GenerateRiver(float x, float y, ModuleBase noise)
    {
        double sample = Mathf.Abs((float)(noise.GetValue(x * riverFrequency, y * riverFrequency, 0)));
        Debug.Log(sample);
        if (sample <= riverThreshold)
        {
            Vector3Int pos = new Vector3Int((int)x, (int)y, 0);
            defaultTile.tilemap.SetTileFlags(pos, TileFlags.None);
            defaultTile.tilemap.SetTile(pos, defaultTile.tile);
            defaultTile.tilemap.SetColor(pos, Color.white);
        }
    }
    private NoiseTile GetTileFromNoise(float x, float y, ModuleBase noise, out double sample)
    {

        // Using perlin Noise
        //float xCord = (float)x / width;
        //float yCord = (float)y / height;
        //float sample = Mathf.PerlinNoise(xCord * noiseScale, yCord * noiseScale);

        // Voronoi
        sample = Mathf.Abs((float)noise.GetValue(x * frequency, y * frequency, 0));
        foreach (NoiseTile nt in tiles)
        {
            if (sample <= nt.value)
            {
                return nt;
            }
        }
        
        Debug.Log(sample);
        return defaultTile;
    }
}
