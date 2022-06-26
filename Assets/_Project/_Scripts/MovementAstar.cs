using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;
public class MovementAstar : AIPath
{
    [SerializeField] private RangeFinder range;
    [SerializeField] private RVOController rvo;
    [SerializeField] private SpriteRotation spRot;
    protected Vector2 moveDirection;
    public float CurrentSpeed => moveDirection.normalized.magnitude;
    private void Update()
    {
        moveDirection = rvo.CalculateMovementDelta(Time.deltaTime);
        SetAngle();
        spRot.SetSpeed(moveDirection.normalized.magnitude);
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
            seeker.StartPath(s, e);
        }
        else
        {
            GroupUnits.QueueGroup(this, e);
        }
    }
}