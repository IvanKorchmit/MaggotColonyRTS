using UnityEngine;
using Pathfinding;
public class UnitAI : MonoBehaviour, ISelectable, IDamagable
{
    [SerializeField] private Seeker seeker;
    [SerializeField] private MovementAstar move;
    [SerializeField] private RangeFinder range;
    [SerializeField] private float health = 50;
    private IAttackable target;
    private Vector2 targetPosition;
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
    }

    public void Damage(float damage, IDamagable owner)
    {
        if (health <= 0) return;
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool Deselect()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        return true;
    }

    public bool Select()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        void Attack()
        {
            target.Damage(3f, this);
        }
        if (target.IsAlive()) 
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
}
public interface IDamagable
{
    Transform transform { get; }
    void Damage(float damage, IDamagable owner);
}

public static class UnityObjectAliveExtension
{
    public static bool IsAlive(this object aObj)
    {
        var o = aObj as UnityEngine.Object;
        return o != null;
    }
}