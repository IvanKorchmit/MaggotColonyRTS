using UnityEngine;

public class ConstructBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask constructionLayer;

    public void ConstructMiner(GameObject obj)
    {
        Building bld = obj.GetComponent<Building>();
        if (bld.Price > Economics.Money) return;
        int layer = constructionLayer;
        
        Collider2D[] crystals = Physics2D.OverlapCircleAll(transform.position, 12f, constructionLayer);
        foreach (var item in crystals)
        {
            if (item.TryGetComponent(out ICrystal crystal))
            {
                void GridSnap_OnPlaceSuccessful(Vector3 position, GameObject prefab)
                {
                    if (Vector2.Distance(position, transform.position) <= 25f)
                    {
                        prefab.layer = LayerMask.NameToLayer("Building");
                        BuildingObserver.StopObserving(prefab.GetComponent<IBuilding>());
                        prefab.GetComponent<SpriteRenderer>().color = Color.white;
                        var miner = Instantiate(prefab, position, Quaternion.identity).GetComponent<IMiner>();
                        crystal.Assign(miner);
                        AstarPath.active.Scan();
                        Economics.GainMoney(-bld.Price);
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
        }
        Debug.LogError("No Crystal found!");
    }

    public void ConstructBuilding(GameObject obj)
    {
        int price;
        void GridSnap_OnPlaceSuccessful(Vector3 position, GameObject prefab)
        {
            Debug.Log(name);
                Economics.GainMoney(-price);
            if (Vector2.Distance(position, transform.position) <= 25f)
            {
                prefab.layer = LayerMask.NameToLayer("Building");
                prefab.GetComponent<SpriteRenderer>().color = Color.white;
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
        Building bld = obj.GetComponent<Building>();
        if (bld.Price <= Economics.Money)
        {
            price = bld.Price;
            GridSnap.Place(obj);
            GridSnap.OnPlaceSuccessful += GridSnap_OnPlaceSuccessful;
        }
    }

}
