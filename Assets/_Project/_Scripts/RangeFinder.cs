using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class RangeFinder : MonoBehaviour
{
    public event System.Action OnUnspot;



    [SerializeField] private List<Transform> targets;
    private CircleCollider2D circle;
    public float Radius { get => circle?.radius ?? 0; private set => circle.radius = value; }
    public Transform ClosestTarget => (targets.Count > 0) ? (targets[0] != null) ? targets[0] : null : null;

    private void FixedUpdate()
    {
        targets.RemoveAll(obj => obj == null);
    }

    private void Start()
    {
        circle = GetComponent<CircleCollider2D>();
        targets = new List<Transform>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (transform.root.CompareTag("Player"))
        {
            if (!targets.Contains(collision.transform))
            {
                if (collision.CompareTag("Enemy"))
                {
                    targets.Add(collision.transform);
                    Resort();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.root.CompareTag("Player"))
        {
            if (collision.CompareTag("Enemy"))
            {
                targets.Add(collision.transform);
                Resort();
            }
        }
        else
        {
            if (collision.CompareTag("Wall") || collision.CompareTag("Player"))
            {
                targets.Add(collision.transform);
                Resort();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targets.Contains(collision.transform))
        {
            targets.Remove(collision.transform);
            Resort();
            OnUnspot?.Invoke();
        }

    }
    private void Resort()
    {
        Transform Min(List<Transform> l)
        {
            Transform temp = l[0];
            foreach (var item in l)
            {
                if (temp == item) { continue; }
                if ((temp.position - transform.position).sqrMagnitude <= (item.position - transform.position).sqrMagnitude)
                {
                    temp = item;
                }
            }
            return temp;
        }
        List<Transform> newList = new List<Transform>(targets.Count);
        for (int i = 0; i < targets.Count; i++)
        {
            Transform min = Min(targets);
            targets.Remove(min);
            newList.Add(min);
        }
        targets = newList;

    }
}
