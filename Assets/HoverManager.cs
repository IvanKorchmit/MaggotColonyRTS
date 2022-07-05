using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class HoverManager : MonoBehaviour
{
    [SerializeField] private IHoverable hover;
    [SerializeField] private LayerMask mask;
    private Camera mainCam;
    private void Start()
    {
        mainCam = Camera.main;
    }
    private void OnGUI()
    {
        Vector2 mouse = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Collider2D point = Physics2D.OverlapPoint(mouse, mask);
        if (point != null && point.TryGetComponent(out IHoverable hover))
        {
            if (this.hover != hover)
            {
                if (this.hover != null && this.hover.IsAlive()) this.hover.OnUnHover();
                hover.OnHover();
                this.hover = hover;
            }
        }
        else if (point == null)
        {
            if (this.hover != null && this.hover.IsAlive()) this.hover.OnUnHover();
        }
    }
}
public interface IHoverable : ITransformAndGameObject
{
    void OnHover();
    void OnUnHover();
}