using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Economics
{
    private static int money = 150;
    private static int steel = 0;
    private static int fuel = 0;
    private static readonly List<ITank> tanks;
    private static readonly List<IUnit> units;
    private static readonly List<IBuilding> buildings;

    private static readonly int maxME = 8;
    private static readonly int maxUnits = 15;
    private static readonly int maxBuildings = 15;

    public static void GainMoney(int money, int steel, int fuel)
    {
        Economics.money += money;
        Economics.steel += steel;
        Economics.fuel += fuel;
    }
    static Economics()
    {
        tanks = new List<ITank>();
        units = new List<IUnit>();
        buildings = new List<IBuilding>();
    }
    public static int MaxME => maxME; // Millitary Equipment
    public static int MaxUnits => maxUnits;
    public static int MaxBuildings => maxBuildings;

    public static int ME => tanks.Count;
    public static int Units => units.Count;
    public static int Buildings => buildings.Count;


    public static bool CountObject(ITransformAndGameObject obj, bool alarm = false)
    {
        ClearDestroyed();
        if (obj == null || !obj.IsAlive()) return false;
        string errorFormat = "Too {0} ({1}/{2})";
        if (obj is ITank tank)
        {
            if (tanks.Count < maxME)
            {
                Debug.Log("Tank");
                tanks.Add(tank);
                return true;
            }
            else
            {
                if (!alarm)
                    ErrorMessageManager.LogError(string.Format(errorFormat, "much military equipment", tanks.Count, maxME));
                return false;
            }
        }
        else if (obj is IUnit unit)
        {
            if (units.Count < maxUnits)
            {
                Debug.Log("Unit");
                units.Add(unit);
                return true;
            }
            else
            {
                if (!alarm)
                    ErrorMessageManager.LogError(string.Format(errorFormat, "many units", units.Count, maxBuildings));
                return false;
            }
        }
        else if (obj is IBuilding building)
        {
            if (buildings.Count < maxBuildings)
            {
                Debug.Log("Building");

                buildings.Add(building);
                return true;
            }
            else
            {
                if (!alarm)
                    ErrorMessageManager.LogError(string.Format(errorFormat, "many buildings", buildings.Count, maxBuildings));
                return false;
            }
        }
        ErrorMessageManager.LogError($"Critical error! {obj.GetType()} {obj.transform.name}");
        return false;
    }

    public static void ClearDestroyed()
    {
        buildings.RemoveAll((match) => match == null || !match.IsAlive());
        tanks.RemoveAll((match) => match == null || !match.IsAlive());
        units.RemoveAll((match) => match == null || !match.IsAlive());
    }

    public static int Money => money;
    public static int Steel => steel;
    public static int Fuel => fuel;

}
