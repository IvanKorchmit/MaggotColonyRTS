using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using LibNoise.Generator;
using LibNoise.Operator;
using LibNoise;
using Pathfinding;
using System.Linq;
using Cinemachine;

public class MapGen : MonoBehaviour
{

    public static event System.Action OnMapGenerated;

    public static MapGen instance;

    [System.Serializable]
    public struct NoiseTile
    {
        [Range(0f, 1f)]
        public float value;
        public TileBase tile;
        public TileBase alt;
        public Tilemap tilemap;
        public Tilemap altTilemap;
        public Color32 minimapColor;
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

    [SerializeField] private double frequency = 2;
    [SerializeField] private double riverFrequency = 2;


    [SerializeField] private Seeker seeker;

    [SerializeField] private NoiseTile infested;
    [NonReorderable]
    [SerializeField] private NoiseTile[] minimapTiles;

    [SerializeField] private Transform vcam;
    [SerializeField] private GameObject crystal;



    private float seed;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        seed = Random.Range(0, 10000f);


        for (int i = 0; i < AstarPath.active.graphs.Length; i++)
        {
            var gg = AstarPath.active.graphs[i] as GridGraph;
            gg.SetDimensions(width, height, gg.nodeSize);
            gg.center = new Vector3(0, (float)height / 4);
        }
        Generate();
        PlaceCommandCenter();
        var graph = AstarPath.active.graphs[0] as GridGraph;
        TimerUtils.AddTimer(0.1f, () => AstarPath.active.Scan(graph));
        OnMapGenerated?.Invoke();
    }
    private void GenerateTree(float x, float y, ModuleBase noise)
    {
        NoiseTile tree = GetTileFromNoise(x, y, noise, trees, 1f - treeDensity, out double sample);
        if (sample <= treeDensity && tree.altTilemap != null)
        {
            tree.tilemap.SetTile(new Vector3Int((int)x, (int)y), tree.tile);
            tree.altTilemap.SetTile(new Vector3Int((int)x, (int)y), tree.alt);
        }
    }

    public Texture2D GetTexture()
    {
        Texture2D result = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                foreach (var t in minimapTiles)
                {
                    TileBase tile = t.tilemap.GetTile(pos);
                    if (tile == null) continue;
                    if (tile != t.tile) continue;
                    result.SetPixel(x, y, t.isGrass ? t.tilemap.GetColor(pos) : t.minimapColor);
                }

            }
        }
        result.filterMode = FilterMode.Point;
        result.Apply();
        return result;
    }

    private void PlaceCommandCenter()
    {

        var cc = Instantiate(commandCenter, new Vector3(), Quaternion.identity);
        Vector3Int origin = Vector3Int.RoundToInt(tilemap.cellBounds.center);
        cc.transform.position = grid.CellToWorld(origin);
        Vector3Int size = new Vector3Int(24, 24, 1);
        ClearAround(size, origin);
        float z = vcam.position.z;
        vcam.transform.position = cc.transform.position;

        vcam.transform.position = new Vector3(vcam.transform.position.x, vcam.transform.position.y, z);
        for (int i = 0; i < 10; i++)
        {
            SpawnNest(cc);
            SpawnCrystal(cc);

        }

    }

    private bool DefaultConstraint(Vector3Int origin, GameObject commandCenter)
    {
        return Vector2.Distance((Vector3)origin, commandCenter.transform.position) < 128;
    }

    private void SpawnCrystal(GameObject commandCenter)
    {
        Vector3Int origin = GetRandomPosition(commandCenter, DefaultConstraint);
        var c = Instantiate(crystal, tilemap.CellToWorld(origin), Quaternion.identity);
        Vector3Int size = new Vector3Int(24, 24, 1);
        BoundsInt bounds = new(origin - (size / 2), size);
        foreach (var item in tilemapToClear)
        {
            foreach (var pos in bounds.allPositionsWithin)
            {
                item.SetTile(pos, tileToReplace);
            }
        }
        ClearPath(c, commandCenter);
    }


    private void SpawnNest(GameObject commandCenter)
    {
        Vector3Int origin = GetRandomPosition(commandCenter, DefaultConstraint);

        GameObject egg = Instantiate(centralEgg, new Vector3(), Quaternion.identity);
        egg.transform.position = tilemap.CellToLocal(origin);
        Vector3Int size = new Vector3Int(24, 24, 1);
        ModuleBase noise = new RidgedMultifractal();
        ClearAround(size, origin);
        size = new Vector3Int(60, 60, 1);
        BoundsInt bounds = new(origin - (size / 2), size);
        foreach (var pos in bounds.allPositionsWithin)
        {
            double sample = Mathf.Abs((float)noise.GetValue((pos.x + seed) * infested.value, (pos.y + seed) * infested.value, 0));
            // sample *= radius - Vector2.Distance((Vector3)pos, bounds.center);
            if (sample <= infested.value && infested.tilemap.GetTile(pos) != null && Vector2.Distance((Vector3)origin, (Vector3)pos) <= 25f)
            {
                infested.tilemap.SetTile(pos, infested.tile);
            }
        }
        ClearPath(egg, commandCenter);
    }

    private void ClearAround(Vector3Int size, Vector3Int origin)
    {
        BoundsInt bounds = new BoundsInt(origin - (size / 4), size);
        foreach (var item in tilemapToClear)
        {
            foreach (var pos in bounds.allPositionsWithin)
            {
                item.SetTile(pos, tileToReplace);
            }
        }
    }
    public delegate bool ConditionFilter(Vector3Int origin, GameObject commandCenter);
    private Vector3Int GetRandomPosition(GameObject commandCenter, ConditionFilter rules)
    {
        Vector3Int origin;
        // Determining random point within the map
        do
        {
            origin = new Vector3Int(0, 0);
            origin.x += Random.Range(0, width);
            origin.y += Random.Range(0, height);
        } while (rules == null || !rules(origin, commandCenter));
        return origin;
    }

    void OnPathDone(Path p)
    {
        if (p.error)
        {
            Debug.Log(p.errorLog);
            return;
        }
        foreach (var gn in p.vectorPath)
        {
            ClearAround(new Vector3Int(8, 8, 1), tilemap.origin + tilemap.WorldToCell(gn));
        }
    }
    private void ClearPath(GameObject a, GameObject b)
    {
        var pathcreator = AstarPath.active.graphs[1] as GridGraph;
        AstarPath.active.Scan(pathcreator);
        seeker.StartPath(a.transform.position, b.transform.position, OnPathDone);
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
                NoiseTile tile = GetTileFromNoise(x, y, moduleBase, tiles, frequency, out double samp);
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
        double sample = Mathf.Abs((float)(noise.GetValue((x + seed) * riverFrequency, (y + seed) * riverFrequency, 0)));
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
        sample = Mathf.Abs((float)noise.GetValue((x + seed) * frequency, (y + seed) * frequency, 0));
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
