using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class UnitControlManager : MonoBehaviour
{
    [SerializeField] private GameObject contextMenuBase;
    [SerializeField] private Transform panel;
    [SerializeField] private LayerMask selectionMask;
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
        Vector2 mouse = RT2Mouse.GetMousePosition();
        Vector2 lowerLeft = new Vector2(Mathf.Min(mouse.x, lastClick.x), Mathf.Min(mouse.y, lastClick.y));
        Vector2 upperRight = new Vector2(Mathf.Max(mouse.x, lastClick.x), Mathf.Max(mouse.y, lastClick.y));
        selectionBox.transform.position = lowerLeft;
        selectionBox.transform.localScale = upperRight - lowerLeft;

    }

    public void CancelSelection()
    {
        foreach (var item in selected)
        {
            item.Deselect();
        }
        foreach (Transform item in panel)
        {
            item.SetParent(null);
            Destroy(item.gameObject);
        }
        selected.Clear();
    }

    private void OnClick()
    {
        selectionBox.gameObject.SetActive(true);
        lastClick = RT2Mouse.GetMousePosition();
        selectionBox.transform.position = lastClick;
    }
    private void OnRelease()
    {
        selectionBox.gameObject.SetActive(false);
        Vector2 releasePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var units = Physics2D.OverlapAreaAll(lastClick, releasePos, selectionMask);
        foreach (var item in units)
        {
            var sel = item != null ? item.GetComponent<ISelectable>() : null;
            if (sel != null)
            {
                if (selected.Contains(sel)) continue;
                if (sel.Select())
                {
                    selected.Add(sel);
                    Instantiate(contextMenuBase, panel).GetComponent<ContextMenuGameObject>().Init(sel.ContextMenu);
                }
            }
        }
    }
    private void OnGUI()
    {
        if (Event.current.button == 0)
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
    }
    public void MoveUnits()
    {
        selected.RemoveAll((m) => m == null || !m.IsAlive());
        Vector2 mouse = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D point = Physics2D.CircleCast(mouse, 2, new Vector2(), 1f, LayerMask.GetMask("Enemy", "Building"));
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

public interface ITransformAndGameObject
{
    Transform transform { get; }
    GameObject gameObject { get; }
}

public interface ISelectable : ITransformAndGameObject
{
    ContextMenu @ContextMenu { get; }
    bool Select();
    void Action(OrderBase order);
    bool Deselect();
}
public interface IAttackable : IDamagable
{
}

public interface IHealable : ITransformAndGameObject
{
    void Heal();
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

    public class HealOrder : OrderBase
    {
        public IHealable target;
    }
}
