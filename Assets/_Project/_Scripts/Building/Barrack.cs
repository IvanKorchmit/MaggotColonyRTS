using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Building, ISelectable, IBuilding
{

    private List<GameObject> pending = new List<GameObject>();

    private IEnumerator Create()
    {
        while (pending.Count > 0)
        {
            yield return new WaitForSeconds(5);
            var u = Instantiate(pending[0], transform.position, Quaternion.identity);
            if (Economics.CountObject(u.GetComponent<IDamagable>(), true))
            {

                pending.RemoveAt(0);
            }
            else
            {
                Destroy(u);
            }
        }
    }
    public void ProduceUnit(UnitBarrack unit)
    {
        if (unit.priceMoney <= Economics.Money && unit.priceSteel <= Economics.Steel && unit.priceFuel <= Economics.Fuel)
        {
                pending.Add(unit.unit);
                StopAllCoroutines();
                StartCoroutine(Create());
                Economics.GainMoney(-unit.priceMoney, -unit.priceSteel, -unit.priceFuel);
        }
    }   
}
