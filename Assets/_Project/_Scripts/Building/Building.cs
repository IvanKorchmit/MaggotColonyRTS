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

    public int Price => price;

    public int ID => id;
    public Sprite Icon => icon;

    public GameObject BuildingPreview => buildingPrewiew;

}
