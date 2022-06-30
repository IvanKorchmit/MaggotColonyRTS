using UnityEngine;

public class ContextMenuGameObject : MonoBehaviour
{
    private ContextMenu contextMenu;
    [SerializeField] private GameObject button;
    [SerializeField] private Transform panel;
    public void Init(ContextMenu menu)
    {
        contextMenu = menu;
    }
    private void OnEnable()
    {
        Start();
    }
    private void Start()
    {
        foreach (Transform tr in panel)
        {
            Destroy(tr.gameObject, 0.05f);
        }
        foreach (ContextMenu.Option o in contextMenu.Options)   
        {
            var button = Instantiate(this.button, panel).GetComponent<ContextMenuButton>();
            button.Init(o);
        }
    }
}
