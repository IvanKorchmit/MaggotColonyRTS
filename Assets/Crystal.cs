using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, ICrystal
{
    private IMiner miner;
    [SerializeField] private int money;

    private void Update()
    {
        TimerUtils.AddTimer(10f, GiveMoney);
    }
    private void GiveMoney()
    {
        if (miner != null && miner.IsAlive())
        {
            miner.GainIncome();
        }
    }

    public void Assign(IMiner miner)
    {
        this.miner = miner;
    }

    public void Damage(float damage, IDamagable owner)
    {
        throw new System.NotImplementedException();
    }
}
public interface IMiner : IBuilding
{
    void GainIncome();

}
public interface ICrystal : IBuilding
{
    void Assign(IMiner miner);
}