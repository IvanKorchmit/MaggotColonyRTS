using UnityEngine;
using Pathfinding;


public class UnitAI : MonoBehaviour, ISelectable, IDamagable, IUnit, IHoverable
{
    protected OrderBase order;
    [SerializeField] private GameObject statsDisplay;
    [SerializeField] protected Seeker seeker;
    [SerializeField] protected MovementAstar move;
    [SerializeField] protected RangeFinder range;
    [SerializeField] protected float health = 50;
    [SerializeField] protected float maxHealth;
    [SerializeField] private AudioEvent attack;
    [SerializeField] protected AudioSource audioSource;
    protected IAttackable target;
    protected Vector2 targetPosition;
    [SerializeField] protected ContextMenu contextMenu;
    public ContextMenu ContextMenu => contextMenu;

    public float Health => health;

    public float MaxHealth => maxHealth;

    private void Start()
    {
        maxHealth = health;
    }


    public void Action(OrderBase order)
    {
         
        if (order is OrderBase.AttackOrder attack)
        {
            target = attack.target;
            targetPosition = attack.target.transform.position;
        }
        else if (order is OrderBase.MoveOrder move)
        {
            seeker.StartPath(transform.position, move.position);
        }
        this.order = order;

    }

    public virtual void Damage(float damage, IDamagable owner)
    {
        if (health <= 0) return;
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual bool Deselect()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        return true;
    }

    public virtual bool Select()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        return true;
    }
    protected virtual void Attack()
    {
        target.Damage(3f, this);
        attack.Play(audioSource);
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (target.IsAlive() && (order == null || order is OrderBase.AttackOrder)) 
        {
            seeker.StartPath(transform.position, targetPosition);
            targetPosition = target.transform.position;
            if (range.ClosestTarget == target.transform)
            {
                move.canMove = false;
                TimerUtils.AddTimer(2, Attack);
            }
            else
            {
                move.canMove = true;
            }
        }
        else
        {
            if (range.ClosestTarget != null && range.ClosestTarget.TryGetComponent(out IAttackable att))
            {
                target = att;
            }
        }
    }

    public void OnHover()
    {
        statsDisplay.SetActive(true);
    }

    public void OnUnHover()
    {
        statsDisplay.SetActive(false);
    }
}
public interface IDamagable : ITransformAndGameObject
{
    void Damage(float damage, IDamagable owner);
    float Health { get; }
    float MaxHealth { get; }


}

public static class UnityObjectAliveExtension
{
    public static bool IsAlive(this object aObj)
    {
        var o = aObj as UnityEngine.Object;
        return o != null;
    }
}



public interface IUnit { }
public interface ITank { }