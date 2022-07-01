using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : Building, ISelectable, IBuilding
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
            Instantiate(u, transform.position, Quaternion.identity);
        }
    }
}


