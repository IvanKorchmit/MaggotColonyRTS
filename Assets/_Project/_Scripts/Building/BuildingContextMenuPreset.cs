using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu]
public class BuildingContextMenuPreset : ScriptableObject
{
    [SerializeField] private ContextMenu contextMenu;
    public void AddToObject(Building building, ContextMenu buildingContextMenu)
    {

        foreach (var item in contextMenu.Options)
        {
            UnityEvent e = new UnityEvent();
            string invokeMethod = item.Action.GetPersistentMethodName(0);
            e.AddListener(() => building.Invoke(invokeMethod, 0));
            buildingContextMenu.Add(item.Summary, e);
        }
    }
}