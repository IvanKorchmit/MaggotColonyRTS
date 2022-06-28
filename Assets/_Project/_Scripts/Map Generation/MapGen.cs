using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using LibNoise.Generator;
using LibNoise.Operator;
using LibNoise;
using Pathfinding;


public class MapGen : MonoBehaviour
{
    [System.Serializable]
    public struct NoiseTile
    {
        [Range(0f, 1f)]
        public float value;
        public TileBase tile;
        public TileBase alt;
        public Tilemap tilemap;
        public Tilemap altTilemap;
        public bool isGrass;
    }

    [SerializeField] float riverThreshold;

    [SerializeField] private GameObject commandCenter;
    [SerializeField] private GameObject centralEgg;
    [SerializeField] private Grid grid;
    [SerializeField] private Gradient grassColor;
    [SerializeField] private int width, height;
    [SerializeField] private Tilemap treeTilemap;
    [NonReorderable]
    [SerializeField] private NoiseTile[] tiles;
    [NonReorderable]
    [SerializeField] private NoiseTile[] trees;
    [SerializeField] private NoiseTile defaultTile;
    [NonReorderable]
    [Range(0f, 1f)]
    [SerializeField] private float treeDensity;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap[] tilemapToClear;
    [SerializeField] private TileBase tileToReplace;

    [SerializeField] double frequency = 2;
    [SerializeField] double riverFrequency = 2;





    // Start is called before the first frame update
    void Start()
    {
        var gg = AstarPath.active.graphs[0] as GridGraph;
        gg.SetDimensions(width, height, gg.nodeSize);
        gg.center = new Vector3(0,(float)height/4);  
        Generate();
        PlaceCommandCenter();
        Physics2D.SyncTransforms();
        TimerUtils.AddTimer(0.1f,()=>AstarPath.active.Scan(gg));
    }
    private void GenerateTree(float x, float y, ModuleBase noise)
    {
        NoiseTile tree = GetTileFromNoise(x, y, noise, trees, 1f-treeDensity, out double sample);
        if (sample <= treeDensity && tree.altTilemap != null)
        {
            tree.tilemap.SetTile(new Vector3Int((int)x, (int)y), tree.tile);
            tree.altTilemap.SetTile(new Vector3Int((int)x, (int)y), tree.alt);
        }
    }
    private void PlaceCommandCenter()
    {
        
        var cc = Instantiate(commandCenter, new Vector3(), Quaternion.identity);
        Vector3Int origin = Vector3Int.RoundToInt(tilemap.cellBounds.center);
        cc.transform.position = grid.CellToWorld(origin);
        BoundsInt bounds = new (origin, new Vector3Int(24, 24, 1));
        foreach (var item in tilemapToClear)
        {
            foreach (var pos in bounds.allPositionsWithin)
            {
                item.SetTile(pos, tileToReplace);
            }
        }
        Camera.main.transform.position = cc.transform.position;
        origin.x += Random.Range(-tilemap.cellBounds.min.x, -tilemap.cellBounds.max.x);
        origin.y += Random.Range(-tilemap.cellBounds.min.y, -tilemap.cellBounds.max.y);
        var egg = Instantiate(centralEgg, new Vector3(), Quaternion.identity);
        egg.transform.position = origin;
        bounds = new(origin, new Vector3Int(24, 24, 1));
        foreach (var item in tilemapToClear)
        {
            foreach (var pos in bounds.allPositionsWithin)
            {
                item.SetTile(pos, tileToReplace);
            }
        }
        ClearPath(egg, cc);
    }

    private void ClearPath(GameObject egg, GameObject cc)
    {
        ABPath path = ABPath.Construct(egg.transform.position, cc.transform.position);
        foreach (var item in tilemapToClear)
        {
            foreach (var gn in path.path)
            {
                BoundsInt bounds = new(tilemap.origin + Vector3Int.RoundToInt((Vector3)gn.position), new Vector3Int(6, 6, 1));
                foreach (var pos in bounds.allPositionsWithin)
                {
                    item.SetTile(pos, tileToReplace);
                }
            }
        }
    }

    public void Generate()
    {
        ModuleBase moduleBase = new Voronoi();
        ModuleBase riverNoise = new RidgedMultifractal();
        ModuleBase perlinTree = new Perlin();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                NoiseTile tile = GetTileFromNoise(x,y, moduleBase,tiles, frequency, out double samp);
                tile.tilemap.SetTile(pos, tile.tile);   
                if (tile.isGrass)
                {
                    tile.tilemap.SetTileFlags(pos, TileFlags.None);
                    tile.tilemap.SetColor(pos, grassColor.Evaluate((float)samp));
                    if (Random.value >= treeDensity)
                    {
                        GenerateTree(x, y, perlinTree);
                    }
                }
                GenerateRiver(x, y, riverNoise);
            }
        }
    }
    private void GenerateRiver(float x, float y, ModuleBase noise)
    {
        double sample = Mathf.Abs((float)(noise.GetValue(x * riverFrequency, y * riverFrequency, 0)));
        if (sample <= riverThreshold)
        {
            Vector3Int pos = new Vector3Int((int)x, (int)y, 0);
            defaultTile.tilemap.SetTileFlags(pos, TileFlags.None);
            defaultTile.tilemap.SetTile(pos, defaultTile.tile);
            defaultTile.tilemap.SetColor(pos, Color.white);
        }
    }
    private NoiseTile GetTileFromNoise(float x, float y, ModuleBase noise, NoiseTile[] tiles, double frequency, out double sample)
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
        
        return defaultTile;
    }
}
