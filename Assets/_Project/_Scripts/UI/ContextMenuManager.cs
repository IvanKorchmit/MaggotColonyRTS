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

        public UnityEvent Action => action;


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
