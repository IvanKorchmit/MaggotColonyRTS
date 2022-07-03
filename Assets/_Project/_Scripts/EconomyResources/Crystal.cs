using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, ICrystal
{
    public IMiner CurrentMiner => miner;
    private IMiner miner;
    [SerializeField] private int money;
    private void Update()
    {
        TimerUtils.AddTimer(5f, GiveMoney);
    }
    private void GiveMoney()
    {
        if (miner != null && miner.IsAlive())
        {
            miner.GainIncome(money);
        }
    }

    public void Assign(IMiner miner)
    {
        this.miner = miner;
        miner.OnDestroyed += Miner_OnDestroyed;
    }

    private void Miner_OnDestroyed()
    {
        miner = null;
    }

}
public interface IMiner : IBuilding
{
    void GainIncome(int money);
    event System.Action OnDestroyed;
}
public interface ICrystal
{
    IMiner CurrentMiner { get; }
    void Assign(IMiner miner);
}
public interface ISteel
{
    IDrill CurrentDrill { get; }
    void Assign(IDrill drill);
}
public interface IFuel
{
    public interface IPump
    {
        void GainIncome(int money);
        event System.Action OnDestroyed;
    }
    IPump CurrentPump { get; }
    void Assign(IPump pump);
}
public interface IDrill
{
    void GainIncome(int money);
    event System.Action OnDestroyed;
}