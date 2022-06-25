using UnityEngine;
using Pathfinding;
public class UnitAI : MonoBehaviour, ISelectable
{
    [SerializeField] private Seeker seeker;
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
        if (target != null)
        {
            seeker.StartPath(transform.position, targetPosition);
            targetPosition = target.transform.position;
        }
    }
}
