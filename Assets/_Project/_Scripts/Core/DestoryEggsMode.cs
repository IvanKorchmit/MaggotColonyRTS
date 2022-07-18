using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryEggsMode : MonoBehaviour
{
    public static event Action ModeOnGameOver;
    public static event Action<string> OnGameOver;

    private List<Egg> bases = new List<Egg>();


    public  void OnEnabled()
    {
        Egg.ServerOnBaseSpawned += ServerHandleBaseSpawned;
        Egg.ServerOnBaseDespawned += ServerHandleBaseDespawned;
    }

    public void OnDisabled()
    {
        Egg.ServerOnBaseSpawned -= ServerHandleBaseSpawned;
        Egg.ServerOnBaseDespawned -= ServerHandleBaseDespawned;
    }

    private void ServerHandleBaseSpawned(Egg unitBase)
    {
        bases.Add(unitBase);
    }


    private void ServerHandleBaseDespawned(Egg unitBase)
    {
        bases.Remove(unitBase);

        if (bases.Count != 1) { return; }

        //int playerId = bases[0].connectionToClient.connectionId;

        RpcGameOver($"Player ");

        ModeOnGameOver?.Invoke();

    }

    private void RpcGameOver(string winner)
    {
        OnGameOver?.Invoke(winner);
    }
       
}
