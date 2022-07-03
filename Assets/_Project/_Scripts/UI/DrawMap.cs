using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DrawMap : MonoBehaviour
{
    [SerializeField] private RawImage image;
    void Awake()
    {
        MapGen.OnMapGenerated += MapGen_OnMapGenerated;
    }

    private void MapGen_OnMapGenerated()
    {
        image.texture = MapGen.instance.GetTexture();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
