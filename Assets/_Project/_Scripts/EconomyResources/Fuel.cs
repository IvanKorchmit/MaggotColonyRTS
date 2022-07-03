using UnityEngine;

public class Fuel : MonoBehaviour, IFuel
{
    private IFuel.IPump pump;
    public IFuel.IPump CurrentPump => pump;
    [SerializeField] private int money;
    public void Assign(IFuel.IPump pump)
    {
        this.pump = pump;
        this.pump.OnDestroyed += Pump_OnDestroyed;
    }
    private void Update()
    {
        TimerUtils.AddTimer(5f, GiveMoney);
    }
    private void Pump_OnDestroyed()
    {
        pump = null;
    }

    private void GiveMoney()
    {
        if (pump != null && pump.IsAlive())
        {
            pump.GainIncome(money);
        }
    }
} 
