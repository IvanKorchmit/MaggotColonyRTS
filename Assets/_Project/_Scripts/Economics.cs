using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Economics
{
    private static int money;
    private static List<ITank> tanks;
    private static List<IUnit> units;

    public static void GainMoney(int v)
    {
        money += v;
    }


}
