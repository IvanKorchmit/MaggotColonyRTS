using UnityEngine;

public class Miner : MonoBehaviour, IMiner
{
    public event System.Action OnDestroyed;

    [SerializeField] private float health;
    [SerializeField] private float radius;
    public float SpaceRequiredCircle => radius;
    [SerializeField] private ContextMenu contextMenu;
    public ContextMenu ContextMenu => contextMenu;

    public void Action(OrderBase order) { }

    public void Damage(float damage, IDamagable owner)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
    public void Sell()
    {
        Economics.GainMoney(25);
        OnDestroyed?.Invoke();

        Destroy(gameObject);
    }
    public bool Deselect() => false;

    public void GainIncome(int money)
    {
        Economics.GainMoney(money);
    }

    public bool Select() => false;
}
