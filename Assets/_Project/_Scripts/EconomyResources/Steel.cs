using UnityEngine;

public class Steel : MonoBehaviour, ISteel
{
    public IDrill CurrentDrill => drill;
    private IDrill drill;
    [SerializeField] private int money;
    private void Update()
    {
        TimerUtils.AddTimer(5f, GiveMoney);
    }
    private void GiveMoney()
    {
        if (drill != null && drill.IsAlive())
        {
            drill.GainIncome(money);
        }
    }

    public void Assign(IDrill drill)
    {
        this.drill = drill;
        drill.OnDestroyed += Miner_OnDestroyed;
    }

    private void Miner_OnDestroyed()
    {
        drill = null;
    }

}
