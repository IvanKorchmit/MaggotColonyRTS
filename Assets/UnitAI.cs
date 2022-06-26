using UnityEngine;
using Pathfinding;
public class UnitAI : MonoBehaviour, ISelectable
{
    [SerializeField] private Seeker seeker;
    [SerializeField] private MovementAstar move;
    [SerializeField] private RangeFinder range;
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
            target.Damage(3f);
        }
        if (target != null)
        {
            seeker.StartPath(transform.position, targetPosition);
            targetPosition = target.transform.position;
            if (range.ClosestTarget == target.transform)
            {
                move.CanMove = false;
                TimerUtils.AddTimer(2, Attack);
            }
            else
            {
                move.CanMove = true;
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
    void Damage(float damage);
}