using UnityEngine;

public class ConstructBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask constructionLayer;

    public void Construct(GameObject obj)
    {


        int layer = constructionLayer;

        Collider2D[] crystals = Physics2D.OverlapCircleAll(transform.position, 12f, constructionLayer);
        foreach (var item in crystals)
        {
            Debug.Log(item.name);
            if (item.TryGetComponent(out ICrystal crystal))
            {
                void GridSnap_OnPlaceSuccessful(Vector3 position, GameObject prefab)
                {
                    Debug.Log(name);
                    if (Vector2.Distance(position, transform.position) <= 25f)
                    {
                        prefab.layer = LayerMask.NameToLayer("Building");
                        prefab.GetComponent<SpriteRenderer>().color = Color.white;
                        var miner = Instantiate(prefab, position, Quaternion.identity).GetComponent<IMiner>();
                        crystal.Assign(miner);
                        AstarPath.active.Scan();
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
                    // Checking space
                    GridSnap.Place(obj);
                    GridSnap.OnPlaceSuccessful += GridSnap_OnPlaceSuccessful;
                    return;
                }
                else
                {
                    Debug.LogError("This crystal is already occupied!");
                    return;
                }
            }
        }
        Debug.LogError("No Crystal found!");
    }
}
