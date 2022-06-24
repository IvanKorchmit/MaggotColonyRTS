using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;
public class MovementAstar : MonoBehaviour
{
    [SerializeField] private RangeFinder range;
    [SerializeField] private Seeker seeker;
    [SerializeField] private RVOController rvo;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRotation spRot;
    [SerializeField] private float wayPointDistance;
    protected Vector2 moveDirection;
    private Path path;
    public bool CanMove { get; set; } = true;
    private void OnPathCalculated(Path p)
    {
        if (!p.error)
        {
            path = p;
        }
    }
    private void Update()
    {
        MoveAlong();
        moveDirection = rvo.CalculateMovementDelta(Time.deltaTime);
        SetAngle();
        transform.Translate(moveDirection * speed);
        
    }
    private void SetAngle()
    {
        if (moveDirection == Vector2.zero) return;
        float a = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        spRot.SetAngle((int)a);
    }
    public void FindPath(Vector2 s, Vector2 e, bool grouped = false)
    {
        if (!grouped)
        {
            seeker.StartPath(s, e, OnPathCalculated);
           
        }
        else
        {
            GroupUnits.QueueGroup(this, e);
        }
    }
    private void MoveAlong()
    {
        if (!CanMove)
        {
            return;
        }
        else if (path == null)
        {
            if (range.ClosestTarget != null)
            {
                seeker.StartPath(transform.position,
                    range.ClosestTarget.position, OnPathCalculated);
            }
        }
        if (path != null)
        {
            if (range.ClosestTarget != null)
            {
                seeker.StartPath
                    (
                    transform.position,
                    range.ClosestTarget.position,
                    OnPathCalculated
                    );
                CanMove = true;
            }
            rvo.SetTarget((Vector3)path.path[0].position, speed, speed);
            if (Vector2.Distance(transform.position, (Vector3)path.path[0].position) <= wayPointDistance)
            {
                if (path.path.Count - 1 > 0)
                {
                    path.path.RemoveAt(0);
                }
                else
                {
                    rvo.SetTarget(new Vector3(), 0, 0);
                    path = null;
                }
            }
        }
    }
}