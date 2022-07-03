using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Economics
{
    private static int money = 150;
    private static int steel = 0;
    private static int fuel = 0;
    private static List<ITank> tanks;
    private static List<IUnit> units;

    public static void GainMoney(int money, int steel, int fuel)
    {
        Economics.money += money;
        Economics.steel += steel;
        Economics.fuel += fuel;
    }
    public static int Money => money;
    public static int Steel => steel;
    public static int Fuel => fuel;

}
