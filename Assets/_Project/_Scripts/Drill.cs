using UnityEngine;

public class Drill : Building, IDrill
{
    public event System.Action OnDestroyed;
    [SerializeField] private int multiplier;
    protected override void Start()
    {
        base.Start();
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
        Economics.GainMoney(25, 0, 0);
        OnDestroyed?.Invoke();

        Destroy(gameObject);
    }
    public void GainIncome(int money)
    {
        Economics.GainMoney(0, money * multiplier, 0);
    }
}
