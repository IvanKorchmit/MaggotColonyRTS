using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;
public class MovementAstar : AIPath
{
    [SerializeField] private RangeFinder range;
    [SerializeField] private RVOController rvo;
    [SerializeField] private SpriteRotation spRot;
    public float CurrentSpeed => velocity.normalized.magnitude;
    private void Update()
    {
        SetAngle();
        spRot.SetSpeed(velocity.normalized.magnitude);
    }
    private void SetAngle()
    {
        if (velocity == Vector3.zero) return;
        float a = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        spRot.SetAngle((int)a);
    }

    public void SetAngle(Vector2 moveDirection)
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