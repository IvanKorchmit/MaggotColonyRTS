using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
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
}
public interface IMiner : IBuilding
{
    void GainIncome();

}