using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class UnitControlManager : MonoBehaviour
{

    private Vector2 lastClick;
    private Camera mainCam;
    private List<ISelectable> selected;
    [SerializeField] private Transform selectionBox;
    void Start()
    {
        mainCam = Camera.main;
        selected = new List<ISelectable>();
    }
    private void MouseHold()
    {
        Vector2 mouse = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 lowerLeft = new Vector2(Mathf.Min(mouse.x, lastClick.x), Mathf.Min(mouse.y, lastClick.y));
        Vector2 upperRight = new Vector2(Mathf.Max(mouse.x, lastClick.x), Mathf.Max(mouse.y, lastClick.y));
        selectionBox.transform.position = lowerLeft;
        selectionBox.transform.localScale = upperRight - lowerLeft;

    }
    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            selectionBox.gameObject.SetActive(true);
            lastClick = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            selectionBox.transform.position = lastClick;
            foreach (var item in selected)
            {
                item.Deselect();
            }
            selected.Clear();
        }
        else if (ctx.canceled)
        {
            OnRelease();
        }
    }
    private void OnRelease()
    {
        selectionBox.gameObject.SetActive(false);
        Vector2 releasePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var units = Physics2D.OverlapAreaAll(lastClick, releasePos, LayerMask.GetMask("Player"));
        foreach (var item in units)
        {
            var sel = item != null ? item.GetComponent<ISelectable>() : null;
            if (sel != null)
            {
                sel.Select();
                selected.Add(sel);
            }
        }
    }
    private void Update()
    {
        if (Mouse.current.leftButton.ReadValue() >= 0.1f) MouseHold();
    }
}
public interface ISelectable
{
    bool Select();
    bool Deselect();
}