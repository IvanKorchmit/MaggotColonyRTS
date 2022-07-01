using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Building[] buildings = new Building[0];
    [SerializeField] private LayerMask buildingBlockLayer = new LayerMask();
    [SerializeField] private float buildingRangeLimit = 5;
    private int resoucres = 500;

    // List<Unit> myUnits = new List<Unit>();
    private List<Building> myBuildings = new List<Building>();
    private Color teamColor = new Color();

    public event Action<int> OnResourceUpdated;

    //public List<Unit> GetmyUnits()
    //{
    //    return myUnits;
    //}

    public bool CanPlaceBuilding(Vector2 point, BoxCollider2D buildingCollider)
    {
        if (Physics.CheckBox(point + buildingCollider.offset, buildingCollider.size / 2, Quaternion.identity, buildingBlockLayer)) { return false; }
        foreach (Building building in myBuildings)
        {
            Vector2 pointplace = new Vector2(point.x - building.transform.position.x, point.y - building.transform.position.y);
            if (pointplace.sqrMagnitude <= buildingRangeLimit * buildingRangeLimit)
            {
                return true;
            }
        }
        return false;
    }


    public List<Building> GetmyBuildings()
    {
        return myBuildings;
    }
    public int GetResoucres()
    {
        return resoucres;
    }
    public Color GetTeamColor()
    {
        return teamColor;
    }

    public void SetResoucres(int newResoucers)
    {
        resoucres = newResoucers;
    }
    private void HandleResourceUpdated(int oldResoucres, int newResoucres)
    {
        OnResourceUpdated?.Invoke(newResoucres);
    }

    private void HandleBuildingDespawned(Building building)
    {        
        myBuildings.Remove(building);
    }

    private void HandleBuildingSpawned(Building building)
    {        
        myBuildings.Add(building);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
