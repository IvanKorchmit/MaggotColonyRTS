using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObserver : MonoBehaviour
{

    private const int MINUTES_BEFORE_ATTACK = 3;
    public static event System.Action AttackBase;
    private List<IBuilding> buildingsToObserve;
    private static BuildingObserver instance;
    private static int selectAmount = 3;
    public static int SelectAmount => selectAmount;
    public static int currentlySelected = 0;
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
        InvokeRepeating(nameof(Invoke), 60*MINUTES_BEFORE_ATTACK, 60*MINUTES_BEFORE_ATTACK);
    }
    public static IBuilding GetBuilding()
    {
        if (instance.buildingsToObserve.Count > 0)
        {
            return instance.buildingsToObserve[Random.Range(0, instance.buildingsToObserve.Count)];
        }
        return null;
    }
    private void Invoke()
    {
        AttackBase?.Invoke();
        currentlySelected = 0;
        selectAmount *= selectAmount;
    }
    private void OnGUI()
    {
        GUILayout.Label(Time.time.ToString());
    }
}

public interface IBuilding : IDamagable, ISelectable
{
    void Sell();
}
