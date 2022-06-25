using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;
public class BugAI : MonoBehaviour, IAttackable
{
    [SerializeField] private int wanderSpread;
    [SerializeField] private int wanderLength;
    [SerializeField] private Seeker seeker;
    [SerializeField] private RangeFinder range;
    RandomPath path;
    private void Start()
    {
        Wander();
        InvokeRepeating(nameof(Wander), 0, 3f);
        range.OnUnspot += Range_OnUnspot;
    }

    private void Range_OnUnspot()
    {
        Wander();
    }

    private void Wander()
    {
        if (range.ClosestTarget != null)
        {
            CancelInvoke(nameof(Wander));
        }
        path = RandomPath.Construct(transform.position, wanderLength);
        path.spread = wanderSpread;
        seeker.StartPath(path);
    }
}
