using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuilding, ISelectable, IHoverable
{
    [SerializeField] private GameObject displayHealth;


    public void OnHover()
    {
        displayHealth.SetActive(true);
    }

    public void OnUnHover()
    {
        displayHealth.SetActive(false);
    }


    private void OnDestroy()
    {
        Economics.ClearDestroyed();
    }




    [SerializeField] protected int priceMoney = 100;
    [SerializeField] protected int priceSteel = 0;
    [SerializeField] protected int priceFuel = 0;
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] private ContextMenu contextMenu;
    [SerializeField] private BuildingContextMenuPreset actionsPreset;
    [SerializeField] private ConstructionMenu constructionPreset;
    public (int money,int steel,int fuel) Cost => (priceMoney, priceSteel, priceFuel);

    public ContextMenu ContextMenu => contextMenu;

    public float Health => health;

    public float MaxHealth => maxHealth;

    public virtual void Sell()
    {
        Economics.GainMoney(priceMoney / 2, priceSteel / 2, priceFuel / 2);
        BuildingObserver.StopObserving(this);
        Destroy(gameObject);
    }
    protected virtual void Start()
    {
        Economics.CountObject(this);
        maxHealth = health;
        actionsPreset.AddToObject(this, contextMenu);
        ConstructBehaviour cb = GetComponent<ConstructBehaviour>();
        foreach (var item in constructionPreset.options)
        {
            UnityEngine.Events.UnityEvent e = new UnityEngine.Events.UnityEvent();
            e.AddListener(() => cb.ConstructBuilding(item.building));
            contextMenu.Add(item.option, e);
        }
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

    public bool Select() => true;

    public void Action(OrderBase order) { }

    public bool Deselect() => true;
}
