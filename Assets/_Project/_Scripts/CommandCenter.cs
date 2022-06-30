using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : MonoBehaviour, ISelectable, IBuilding
{
    [SerializeField] private float health = 300f;
    [SerializeField] private GameObject[] startUnits;
    public void Action(OrderBase order)
    {
        throw new System.NotImplementedException();
    }

    public void Damage(float damage, IDamagable owner)
    {
        health -= damage;
        owner.Damage(3, this);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool Deselect()
    {
        throw new System.NotImplementedException();
    }

    public bool Select()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        BuildingObserver.Observe(this);
        foreach (var u in startUnits)
        {
            Instantiate(u, transform.position, Quaternion.identity);
        }
    }
}
