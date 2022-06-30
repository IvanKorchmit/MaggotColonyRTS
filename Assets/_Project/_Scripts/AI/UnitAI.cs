using UnityEngine;
using Pathfinding;
public class UnitAI : MonoBehaviour, ISelectable, IDamagable, IUnit
{
    [SerializeField] private GameObject miner;

    [SerializeField] private LayerMask constructionLayer;


    [SerializeField] protected Seeker seeker;
    [SerializeField] protected MovementAstar move;
    [SerializeField] protected RangeFinder range;
    [SerializeField] protected float health = 50;
    protected IAttackable target;
    protected Vector2 targetPosition;
    [SerializeField] private ContextMenu contextMenu;
    public ContextMenu ContextMenu => contextMenu;
    public void ConstructMiner()
    {


        int layer = constructionLayer;

        Collider2D[] crystals = Physics2D.OverlapCircleAll(transform.position, 100f, constructionLayer);
        foreach (var item in crystals)
        {
            Debug.Log(item.name);
            if (item.TryGetComponent(out ICrystal crystal))
            {
                void GridSnap_OnPlaceSuccessful(Vector3 position, GameObject prefab)
                {
                    Debug.Log(name);
                    if (Vector2.Distance(position, transform.position) <= 75f)
                    {
                        prefab.layer = LayerMask.NameToLayer("Building");
                        prefab.GetComponent<SpriteRenderer>().color = Color.white;
                        var miner = Instantiate(prefab, position, Quaternion.identity).GetComponent<IMiner>();
                        crystal.Assign(miner);
                        AstarPath.active.Scan();
                        Destroy(prefab);
                        GridSnap.OnPlaceSuccessful -= GridSnap_OnPlaceSuccessful;
                    }
                    else
                    {
                        Debug.LogError("Too far away " + Vector2.Distance((Vector3)position, transform.position));
                    }
                }
                if (crystal.CurrentMiner == null)
                {
                    // Checking space
                    GridSnap.Place(miner);
                    GridSnap.OnPlaceSuccessful += GridSnap_OnPlaceSuccessful;
                    return;
                }
                else
                {
                    Debug.LogError("This crystal is already occupied!");
                    return;
                }
            }
        }
            Debug.LogError("No Crystal found!");
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
    }
    // Update is called once per frame
    protected virtual void Update()
    {
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
    GameObject gameObject { get; }
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



public interface IUnit { }
public interface ITank { }