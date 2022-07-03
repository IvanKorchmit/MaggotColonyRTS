using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObserver : MonoBehaviour
{
    private const int MINUTES_BEFORE_ATTACK = 1;
    public static event System.Action AttackBase;
    private List<IBuilding> buildingsToObserve;
    private static BuildingObserver instance;
    public static void Observe(IBuilding building)
    {
        instance.buildingsToObserve.Add(building);
    }
    public static void StopObserving(IBuilding building)
    {
        instance.buildingsToObserve.RemoveAll((match)=>match==building || match == null || !match.IsAlive());
    }
    private void Start()
    {
        instance = this;
        buildingsToObserve = new List<IBuilding>();
        InvokeRepeating(nameof(Invoke), 5, 60*MINUTES_BEFORE_ATTACK);
    }
    public static IBuilding GetBuilding()
    {
        Debug.Log("Attack");
        if (instance.buildingsToObserve.Count > 0)
        {
            return instance.buildingsToObserve[Random.Range(0, instance.buildingsToObserve.Count)];
        }
        return null;
    }
    private void Invoke()
    {
        AttackBase?.Invoke();
    }
}

public interface IBuilding : IDamagable, ISelectable
{
    void Sell();
}