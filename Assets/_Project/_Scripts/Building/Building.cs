using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuilding, ISelectable
{
    [SerializeField] protected int price = 100;
    [SerializeField] protected float health;
    [SerializeField] private ContextMenu contextMenu;
    [SerializeField] private ConstructionMenu constructionPreset;
    public int Price => price;

    public ContextMenu ContextMenu => contextMenu;

    public virtual void Sell()
    {
        Economics.GainMoney(price);
    }
    private void OnEnable()
    {
        ConstructBehaviour cb = GetComponent<ConstructBehaviour>();
        foreach (var item in constructionPreset.options)
        {
            UnityEngine.Events.UnityEvent e = new UnityEngine.Events.UnityEvent();
            e.AddListener(() => cb.ConstructBuilding(item.building));
            contextMenu.Add(item.option, e);
        }
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
