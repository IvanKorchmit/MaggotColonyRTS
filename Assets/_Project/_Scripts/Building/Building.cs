using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuilding, ISelectable
{
    [SerializeField] GameObject buildingPrewiew;
    [SerializeField] Sprite icon;
    [SerializeField] protected int price = 100;
    [SerializeField] protected float health;
    [SerializeField] private ContextMenu contextMenu;
    public int Price => price;

    public ContextMenu ContextMenu => contextMenu;

    public virtual void Sell()
    {
        Economics.GainMoney(price);
    }
    protected virtual void Start()
    {
        BuildingObserver.Observe(this);
    }

    public virtual void Damage(float damage, IDamagable owner)
    {
        health -= damage;
        if (health <= 0)
        {
            BuildingObserver.StopObserving(this);
            Destroy(gameObject);
        }
    }

    public bool Select() => false;

    public void Action(OrderBase order) { }

    public bool Deselect() => false;
}
