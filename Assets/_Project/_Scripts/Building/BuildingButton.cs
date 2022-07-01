using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] Building building;
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text priceText;
    [SerializeField] LayerMask floorMask=new LayerMask();

    Camera mainCamera;
    Player player;
    GameObject buildingPreviewInstance;
    Renderer buildingRendererInstance;
    private BoxCollider2D buildingCollider;


    private void Update()
    {
        if (buildingPreviewInstance == null) { return; }
        UpdateBuildingPreview();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (buildingPreviewInstance == null) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if(Physics.Raycast(ray,out RaycastHit hit,Mathf.Infinity, floorMask))
        {
            //player.TryPlaceBuilding(building.GetId(), hit.point);
        }

        Destroy(buildingPreviewInstance);
    }

    private void UpdateBuildingPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask)) { return; }

        buildingPreviewInstance.transform.position = hit.point;

        if(!buildingPreviewInstance.activeSelf)
        {
            buildingPreviewInstance.SetActive(true);
        }
        Color color = player.CanPlaceBuilding(hit.point,buildingCollider) ? Color.green : Color.red;

        buildingRendererInstance.material.SetColor("_BaseColor", color);

    }

    

}


