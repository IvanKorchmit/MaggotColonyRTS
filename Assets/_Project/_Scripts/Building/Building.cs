using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] GameObject buildingPrewiew;
    [SerializeField] Sprite icon;
    [SerializeField] int price = 100;
    [SerializeField] int id = -1;


    public static event Action<Building> OnBuildingSpawned;
    public static event Action<Building> OnBuildingDespawned;

    public int GetPrice()
    {
        return price;
    }

    public int GetId()
    {
        return id;
    }
    public Sprite GetIcon()
    {
        return icon;
    }

    public GameObject GetBuildingPreview()
    {
        return buildingPrewiew;
    }

}
