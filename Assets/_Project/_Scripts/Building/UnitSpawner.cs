using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] Health health;
    [SerializeField] UnitAI unitPrefab;
    [SerializeField] Transform unitSpawnpoint;
    [SerializeField] TMP_Text remainingUnitText;
    [SerializeField] Image unitProgressImage;
    [SerializeField] int maxUnitQueue =5;
    [SerializeField] float spawnMoveRange = 7f;
    [SerializeField] float unitSpawnDuration = 5f;


    private int queueUnits;
    private float unitTimer;

    float progressImageVelocity;

    private void Update()
    {
        ProduceUnits();
        UpdateTimerDisplay();
    }

    private void ProduceUnits()
    {
        if (queueUnits == 0) { return; }

        unitTimer += Time.deltaTime;
        if (unitTimer < unitSpawnDuration) { return; }

        GameObject unitInstance = Instantiate(unitPrefab.gameObject, unitSpawnpoint.position, unitSpawnpoint.rotation);
        

        Vector3 spawnOffset = UnityEngine.Random.insideUnitSphere * spawnMoveRange;
        spawnOffset.y = unitSpawnpoint.position.y;

        MovementAstar unitMovement=  unitInstance.GetComponent<MovementAstar>();
        unitMovement.Move(unitSpawnpoint.position + spawnOffset);

        queueUnits--;
        unitTimer = 0;

    }

    private void UpdateTimerDisplay()
    {
        float newProgress = unitTimer / unitSpawnDuration;
        if(newProgress<unitProgressImage.fillAmount)
        {
            unitProgressImage.fillAmount = newProgress;
        }
        else
        {
            unitProgressImage.fillAmount = Mathf.SmoothDamp(unitProgressImage.fillAmount, newProgress, ref progressImageVelocity, 0.1f);
        }
    }



    //private void CmdSpawnUnit()
    //{
    //    if(queueUnits == maxUnitQueue) { return; }
    //    Player player;//GEt Playter Component
    //   // if (player.GetResoucres() < unitPrefab.GetResoucreCost()) { return; } UnitAI must have Resouce COst
    //    queueUnits++;

    //    player.SetResoucres(player.GetResoucres() - unitPrefab.GetResoucreCost());
    //}


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }       

        //CmdSpawnUnit();
    }
    private void HandleQueuedUnitsUpdated(int oldUnits,int newUnits)
    {
        remainingUnitText.SetText(newUnits.ToString());
    }

}
