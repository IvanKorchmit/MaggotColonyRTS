using UnityEngine;

public class ConstructBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask constructionLayer;
    private const float MAX_DISTANCE = 5f;
    public void ConstructMiner(GameObject obj)
    {
        Building bld = obj.GetComponent<Building>();
        if (bld.Cost.money > Economics.Money) 
        {
            ErrorMessageManager.LogError($"Not enough money ({bld.Cost.money})");
            return;
        }
        if (bld.Cost.steel > Economics.Steel)
        {
            ErrorMessageManager.LogError($"Not enough steel ({bld.Cost.steel})");
            return;
        }
        if (bld.Cost.fuel > Economics.Fuel)
        {
            ErrorMessageManager.LogError($"Not enough fuel ({bld.Cost.fuel})");
            return;
        }
        int layer = constructionLayer;
        Collider2D[] resources = Physics2D.OverlapCircleAll(transform.position, MAX_DISTANCE, constructionLayer);
        bool isMiner = obj.TryGetComponent  (out IMiner _);
        bool isDrill = obj.TryGetComponent  (out IDrill _);
        bool isPump = obj.TryGetComponent   (out IFuel.IPump _);
        
        foreach (var item in resources)
        {
            if      (isMiner &&item.TryGetComponent(out ICrystal crystal))
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
                        ErrorMessageManager.LogError("Too far away " + Vector2.Distance((Vector3)position, transform.position));
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
                    ErrorMessageManager.LogError("This crystal is already occupied!");
                }
            }
            else if (isDrill &&item.TryGetComponent(out ISteel steel))
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
                        ErrorMessageManager.LogError("Too far away " + Vector2.Distance((Vector3)position, transform.position));
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
                    ErrorMessageManager.LogError("This steel is already occupied!");
                }

            }
            else if (isPump  &&item.TryGetComponent(out IFuel fuel))
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
                        var pump = Instantiate(prefab, position, Quaternion.identity).GetComponent<IFuel.IPump>();
                        fuel.Assign(pump);
                        AstarPath.active.Scan();
                        Economics.GainMoney(-bld.Cost.money, -bld.Cost.steel, -bld.Cost.fuel);
                        Destroy(prefab);
                        GridSnap.OnPlaceSuccessful -= GridSnap_OnPlaceSuccessful;
                    }
                    else
                    {
                        ErrorMessageManager.LogError("Too far away " + Vector2.Distance((Vector3)position, transform.position));
                    }
                }

                if (fuel.CurrentPump == null)
                {
                    GridSnap.Place(obj);
                    GridSnap.OnPlaceSuccessful += GridSnap_OnPlaceSuccessful;
                    return;
                }
                else
                {
                    ErrorMessageManager.LogError("This fuel is already occupied!");
                }
            }
        }
        ErrorMessageManager.LogError("No Resource found!");
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
            if (Vector2.Distance(position, transform.position) <= 25f && Economics.CountObject(prefab.GetComponent<IBuilding>()))
            {
                Economics.GainMoney(-price.money,-price.steel,-price.fuel);
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
            else if (Vector2.Distance(position, transform.position) > 25f)
            {
                ErrorMessageManager.LogError("Too far away " + Vector2.Distance((Vector3)position, transform.position));
            }
        }
        int layer = constructionLayer;
        GridSnap.Place(obj);
        GridSnap.OnPlaceSuccessful += GridSnap_OnPlaceSuccessful;
    }

}
