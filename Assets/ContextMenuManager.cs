using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class ContextMenuManager : MonoBehaviour
{
    [SerializeField] private ContextMenuGameObject contextMenuBase;
    [SerializeField] private LayerMask mask;
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
                        contextMenuBase.Init(interactable.ContextMenu);
                        (contextMenuBase.transform as RectTransform).anchoredPosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
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
    }

} 
