using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResoucreGenerator : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] int resoucresPerInterval;
    [SerializeField] float interval = 2f;

    float timer;
    [SerializeField] Player player;

    private void OnEnable()
    {
        timer = interval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //player.SetResoucres(player.GetResoucres() + resoucresPerInterval);
            timer += interval;
        }
    }

    //public override void OnStartServer()
    //{
    //    timer = interval;
    //    player=connectionToClient.identity.GetComponent<RTSPlayer>();

    //    health.ServerOnDie += ServerHandleDie;
    //    GameOverHandler.ServerOnGameOver += ServerHandleOnGameOver;
    //}

    //public override void OnStopServer()
    //{
    //    health.ServerOnDie -= ServerHandleDie;
    //    GameOverHandler.ServerOnGameOver -= ServerHandleOnGameOver;
    //}

    //private void ServerHandleOnGameOver()
    //{
    //    enabled = false;
    //}

    //private void ServerHandleDie()
    //{
    //    NetworkServer.Destroy(gameObject);
    //}



    //[ServerCallback]
    //void Update()
    //{
    //    timer -= Time.deltaTime;
    //    if (timer<=0)
    //    {
    //        player.SetResoucres(player.GetResoucres()+resoucresPerInterval);
    //        timer += interval;
    //    }
    //}


}
