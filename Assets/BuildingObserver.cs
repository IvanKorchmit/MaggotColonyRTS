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
        InvokeRepeating(nameof(Invoke), 5, 60*4);
    }
    public static IBuilding GetBuilding()
    {
        return instance.buildingsToObserve[Random.Range(0, instance.buildingsToObserve.Count)];
    }
    private void Invoke()
    {
        AttackBase?.Invoke();
    }
}

public interface IBuilding : IDamagable
{
}
