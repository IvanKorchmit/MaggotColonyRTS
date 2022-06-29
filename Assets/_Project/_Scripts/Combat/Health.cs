
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    
    private int Currenthealth;

    public event Action<int,int> ClientOnHealthUpdated;


    public void OnEnable()
    {
        Currenthealth = maxHealth;
    }

    private void Update()
    {
        DealDamge(Currenthealth);
    }

    public void DealDamge(int damage)
    {
        if (Currenthealth == 0) { return; }

        Currenthealth = Mathf.Max(Currenthealth - damage, 0);

        if (Currenthealth != 0) { return; }

       
    }

    //public void HandleHealthUpdated(int oldHealth,int newHealth)
    //{
    //    ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    //}
    //#endregion
}
