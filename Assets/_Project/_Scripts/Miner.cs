using UnityEngine;

public class Miner : Building, IMiner
{
    public event System.Action OnDestroyed;

    protected override void Start()
    {
        BuildingObserver.Observe(this);
    }
    public override void Damage(float damage, IDamagable owner)
    {
        health -= damage;
        if (health <= 0)
        {
            BuildingObserver.StopObserving(this);
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
    public override void Sell()
    {
        Economics.GainMoney(25);
        OnDestroyed?.Invoke();

        Destroy(gameObject);
    }
    public void GainIncome(int money)
    {
        Economics.GainMoney(money);
    }
}
