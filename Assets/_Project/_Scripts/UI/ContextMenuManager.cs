using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
public class ContextMenuManager : MonoBehaviour
{
    [SerializeField] private ContextMenuGameObject contextMenuBase;
    [SerializeField] private LayerMask mask;
    [SerializeField] private CanvasScaler scaler;
    private void OnGUI()
    {
        EventSystem cur = EventSystem.current;
        if (!cur.IsPointerOverGameObject())
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                Vector2 mouse = Camera.main.ScreenToWorldPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
                Collider2D ray = Physics2D.OverlapPoint(mouse, mask);
                if (ray != null)
                {
                    if (ray.transform.TryGetComponent(out ISelectable interactable))
                    {
                        RectTransform rectTrans = contextMenuBase.transform as RectTransform;
                        contextMenuBase.Init(interactable.ContextMenu);
                        Vector2 ratio = scaler.referenceResolution;
                        ratio.x /= Screen.width;
                        ratio.y /= Screen.height;
                        rectTrans.anchoredPosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue() * ratio;
                        Vector2 apos = rectTrans.anchoredPosition;
                        float xpos = apos.x;
                        xpos = Mathf.Clamp(xpos, rectTrans.sizeDelta.x, Screen.width);
                        apos.x = xpos;
                        rectTrans.anchoredPosition = apos;

                        contextMenuBase.gameObject.SetActive(true);
                    }
                }
            }
            else if (Event.current.type == EventType.MouseDown)
            {
                contextMenuBase.gameObject.SetActive(false);
            }
        }
    }
}
[System.Serializable]
public class ContextMenu
{
    [NonReorderable]
    [SerializeField] private List<Option> options = new List<Option>();
    public List<Option> Options => options;
    public void Invoke(int index)
    {
        options[index].Invoke();
    }
    public void Add(string option, UnityEvent action)
    {
        options.Add(new Option(option, action));
    }
    [System.Serializable]
    public class Option
    {
        [SerializeField] private UnityEvent action;
        [SerializeField] private string summary;
        public void Invoke()
        {
            action?.Invoke();
        }
        public string Summary => summary;
        public Option(string name, UnityEvent action)
        {
            summary = name;
            this.action = action;
        }
    }

} 
