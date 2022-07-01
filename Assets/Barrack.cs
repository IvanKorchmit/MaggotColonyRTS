using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Building, ISelectable, IBuilding
{
    [SerializeField] private ContextMenu contextMenu;

    private List<GameObject> pending;
    protected override void Start()
    {
        pending = new List<GameObject>();
    }
    private void Create()
    {
        if (pending.Count > 0)
        {
            Instantiate(pending[0], transform.position, Quaternion.identity);
            pending.RemoveAt(0);
            TimerUtils.AddTimer(5, Create);
        }
    }
    public void ProduceUnit(GameObject unit)
    {
        pending.Add(unit);
        TimerUtils.AddTimer(5, Create);
    }
}
