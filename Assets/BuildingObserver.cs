using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObserver : MonoBehaviour
{
    public static event System.Action AttackBase;
    private List<IBuilding> buildingsToObserve;
    private static BuildingObserver instance;
    public static void Observe(IBuilding building)
    {
        instance.buildingsToObserve.Add(building);
    }
    private void Start()
    {
        instance = this;
        buildingsToObserve = new List<IBuilding>();
    }
    public static IBuilding GetBuilding()
    {
        return instance.buildingsToObserve[Random.Range(0, instance.buildingsToObserve.Count)];
    }
}

public interface IBuilding
{
    Transform transform { get; }
}
