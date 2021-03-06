using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : Building, IBuilding
{
    [SerializeField] private GameObject[] startUnits;
    public override void Sell()
    {
        Debug.Log("Cannot sell the command center");
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        foreach (var u in startUnits)
        {
            var st_ = Instantiate(u, (Vector2)transform.position + Random.insideUnitCircle * 2f, Quaternion.identity);
            Economics.CountObject(st_.GetComponent<IDamagable>());
        }
    }
}
