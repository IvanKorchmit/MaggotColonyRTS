using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField] Health health;

    public static event Action<int> ServerOnPlayerDie;
    public static event Action<UnitBase> OnBaseSpawned;
    public static event Action<UnitBase> OnBaseDespawned;

    private void OnEnable()
    {
        {
            //health.ServerOnDie += ServerHandleDie;

            OnBaseSpawned?.Invoke(this);
        }
    }

    private void OnDisable()
    {
        OnBaseDespawned?.Invoke(this);
    }
}
