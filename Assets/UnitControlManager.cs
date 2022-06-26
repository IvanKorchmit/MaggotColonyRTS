using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class UnitControlManager : MonoBehaviour
{
    [SerializeField] private float doubleClickTime;
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
    private void OnClick()
    {
        OnDoubleClick();
        selectionBox.gameObject.SetActive(true);
        lastClick = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        selectionBox.transform.position = lastClick;
        foreach (var item in selected)
        {
            item.Deselect();
        }
        selected.Clear();
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
    private void OnGUI()
    {
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown)
        {
            OnClick();
        }
        else if (Event.current.isMouse && Event.current.type == EventType.MouseUp)
        {
            OnRelease();
        }
    }
    private void OnDoubleClick()
    {
            Vector2 mouse = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D point = Physics2D.CircleCast(mouse, 2, new Vector2(), 1f, LayerMask.GetMask("Enemy"));
            if (point.collider != null && point.collider.TryGetComponent(out IAttackable attackable))
            {
                OrderBase.AttackOrder order = new OrderBase.AttackOrder { target = attackable };
                foreach (var item in selected)
                {
                    item.Action(order);
                }
            }
            else
            {
                OrderBase.MoveOrder order = new OrderBase.MoveOrder { position = mouse };
                foreach (var item in selected)
                {
                    item.Action(order);
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
    void Action(OrderBase order);
    bool Deselect();
}
public interface IAttackable : IDamagable
{
    Transform transform { get; }
    GameObject gameObject { get; }
}
public abstract class OrderBase
{
    public class MoveOrder : OrderBase
    {
        public Vector2 position;
    }

    public class AttackOrder : OrderBase
    {
        public IAttackable target;
    }
}