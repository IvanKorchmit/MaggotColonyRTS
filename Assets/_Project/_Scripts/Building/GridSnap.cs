using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class GridSnap : MonoBehaviour
{
    [SerializeField] private LayerMask constructionLayer;


    private Grid m_Grid;
    private GameObject placement;
    private SpriteRenderer placement_sprite;
    public static event System.Action<Vector3, GameObject> OnPlaceSuccessful;
    private static GridSnap instance;
    // Start is called before the first frame update
    void Start()
    {
        m_Grid = FindObjectOfType<Grid>();
        instance = this;
    }

    public static void Place(GameObject placement)
    {
        OnPlaceSuccessful = null;
        while (instance.transform.childCount > 0)
        {
            Transform child = instance.transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        instance.placement = Instantiate(placement, instance.transform);
        instance.placement.transform.localPosition = new Vector3();
        instance.placement_sprite = instance.placement.GetComponentInChildren<SpriteRenderer>();
        instance.placement.layer = LayerMask.NameToLayer("Placement");
        Component[] components = instance.placement.GetComponentsInChildren<Component>();
        foreach (Component comp in components)
        {
            if (comp is Renderer) continue;
            if (comp is MonoBehaviour beh)
            {
                beh.enabled = false;
            }
        }
    }


    private void OnGUI()
    {
        EventSystem cur = EventSystem.current;
        if (!cur.IsPointerOverGameObject())
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (placement == null) return;
                IBuilding building = placement.GetComponent<IBuilding>();
                List<Collider2D> colls = new List<Collider2D>();
                if (Physics2D.OverlapBox(placement.transform.position, Vector2.one * 3, 0, constructionLayer) == null)
                {
                    Component[] components = placement.GetComponentsInChildren<Component>();
                    foreach (Component comp in components)
                    {
                        if (comp is Renderer) continue;
                        if (comp is MonoBehaviour beh)
                        {
                            beh.enabled = true;
                        }
                    }
                    OnPlaceSuccessful?.Invoke(transform.position, placement);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector2.one * 3);
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
        Vector3Int cp = m_Grid.LocalToCell(mouse);
        transform.localPosition = m_Grid.GetCellCenterLocal(cp);
        if (placement == null) return;
        Collider2D coll = Physics2D.OverlapBox(placement.transform.position, Vector2.one * 3, 0, constructionLayer);
        placement_sprite.color = coll == null? Color.green : Color.red;
    }
}
