using UnityEngine;

public class ConstructBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask constructionLayer;
    private const float MAX_DISTANCE = 5f;
    public void ConstructMiner(GameObject obj)
    {
        Building bld = obj.GetComponent<Building>();
        if (bld.Cost.money > Economics.Money) return;
        if (bld.Cost.steel > Economics.Steel) return;
        if (bld.Cost.fuel > Economics.Fuel) return;
        int layer = constructionLayer;

        Collider2D[] resources = Physics2D.OverlapCircleAll(transform.position, MAX_DISTANCE, constructionLayer);
        foreach (var item in resources)
        {
            if (item.TryGetComponent(out ICrystal crystal))
            {
                void GridSnap_OnPlaceSuccessful(Vector3 position, GameObject prefab)
                {
                    if (Vector2.Distance(position, transform.position) <= MAX_DISTANCE)
                    {
                        Component[] components = prefab.GetComponentsInChildren<Component>();
                        foreach (Component comp in components)
                        {
                            if (comp is Renderer) continue;
                            if (comp is MonoBehaviour beh)
                            {
                                beh.enabled = true;
                            }
                        }
                        prefab.layer = LayerMask.NameToLayer("Building");
                        BuildingObserver.StopObserving(prefab.GetComponent<IBuilding>());
                        prefab.GetComponent<SpriteRenderer>().color = Color.white;
                        var miner = Instantiate(prefab, position, Quaternion.identity).GetComponent<IMiner>();
                        crystal.Assign(miner);
                        AstarPath.active.Scan();
                        Economics.GainMoney(-bld.Cost.money, -bld.Cost.steel, -bld.Cost.fuel);
                        Destroy(prefab);
                        GridSnap.OnPlaceSuccessful -= GridSnap_OnPlaceSuccessful;
                    }
                    else
                    {
                        Debug.LogError("Too far away " + Vector2.Distance((Vector3)position, transform.position));
                    }
                }
                if (crystal.CurrentMiner == null)
                {
                    GridSnap.Place(obj);
                    GridSnap.OnPlaceSuccessful += GridSnap_OnPlaceSuccessful;
                    return;
                }
                else
                {
                    Debug.LogError("This crystal is already occupied!");
                }
            }
            else if (item.TryGetComponent(out ISteel steel))
            {
                void GridSnap_OnPlaceSuccessful(Vector3 position, GameObject prefab)
                {
                    if (Vector2.Distance(position, transform.position) <= MAX_DISTANCE)
                    {
                        Component[] components = prefab.GetComponentsInChildren<Component>();
                        foreach (Component comp in components)
                        {
                            if (comp is Renderer) continue;
                            if (comp is MonoBehaviour beh)
                            {
                                beh.enabled = true;
                            }
                        }
                        prefab.layer = LayerMask.NameToLayer("Building");
                        BuildingObserver.StopObserving(prefab.GetComponent<IBuilding>());
                        prefab.GetComponent<SpriteRenderer>().color = Color.white;
                        var drill = Instantiate(prefab, position, Quaternion.identity).GetComponent<IDrill>();
                        steel.Assign(drill);
                        AstarPath.active.Scan();
                        Economics.GainMoney(-bld.Cost.money, -bld.Cost.steel, -bld.Cost.fuel);
                        Destroy(prefab);
                        GridSnap.OnPlaceSuccessful -= GridSnap_OnPlaceSuccessful;
                    }
                    else
                    {
                        Debug.LogError("Too far away " + Vector2.Distance((Vector3)position, transform.position));
                    }
                }

                if (steel.CurrentDrill == null)
                {
                    GridSnap.Place(obj);
                    GridSnap.OnPlaceSuccessful += GridSnap_OnPlaceSuccessful;
                    return;
                }
                else
                {
                    Debug.LogError("This steel is already occupied!");
                }

            }
        
        }
        Debug.LogError("No Crystal found!");
    }

    public void ConstructBuilding(GameObject obj)
    {
        Building bld = obj.GetComponent<Building>();
        var price = bld.Cost;
        if (price.money > Economics.Money) return;
        if (price.steel > Economics.Steel) return;
        if (price.fuel > Economics.Fuel) return;
        void GridSnap_OnPlaceSuccessful(Vector3 position, GameObject prefab)
        {
                Economics.GainMoney(-price.money,-price.steel,-price.fuel);
            if (Vector2.Distance(position, transform.position) <= 25f)
            {
                Component[] components = prefab.GetComponentsInChildren<Component>();
                foreach (Component comp in components)
                {
                    if (comp is Renderer) continue;
                    if (comp is MonoBehaviour beh)
                    {
                        beh.enabled = true;
                    }
                }
                prefab.layer = LayerMask.NameToLayer("Building");
                prefab.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                Instantiate(prefab, position, Quaternion.identity);
                AstarPath.active.Scan();
                BuildingObserver.StopObserving(prefab.GetComponent<IBuilding>());
                Destroy(prefab);
                GridSnap.OnPlaceSuccessful -= GridSnap_OnPlaceSuccessful;
            }
            else
            {
                Debug.LogError("Too far away " + Vector2.Distance((Vector3)position, transform.position));
            }
        }
        int layer = constructionLayer;
        GridSnap.Place(obj);
        GridSnap.OnPlaceSuccessful += GridSnap_OnPlaceSuccessful;
    }

}
