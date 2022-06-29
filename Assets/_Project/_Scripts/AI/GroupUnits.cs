using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class GroupUnits : MonoBehaviour
{
    private static List<MovementAstar> queue = new List<MovementAstar>();
    public static void QueueGroup(MovementAstar mv,Vector2 target)
    {
        void LaunchQueue()
        {
            TimerUtils.Cancel(LaunchQueue);
            List<Vector3> pos = new List<Vector3>();
            for (int i = 0; i < queue.Count; i++)
            {
                pos.Add(queue[i].transform.position);
            }
            // Initializing pos list

            PathUtilities.GetPointsAroundPoint(target, AstarPath.active.graphs[0] as IRaycastableGraph, pos, 2*queue.Count, 0);
            for (int i = 0; i < queue.Count; i++)
            {
                MovementAstar item = queue[i];
                item.FindPath(item.transform.position, pos[i]);
            }
            queue.Clear();
        }
        TimerUtils.AddTimer(0.05f, LaunchQueue);
        queue.Add(mv);
    }
}
