using UnityEngine;

public class Drill : Building, IDrill
{
    public event System.Action OnDestroyed;
    [SerializeField] private int multiplier;
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
        OnDestroyed?.Invoke();
        base.Sell();
    }
    public void GainIncome(int money)
    {
        Economics.GainMoney(0, money * multiplier, 0);
    }
}
