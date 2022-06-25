using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class UnitAI : MonoBehaviour, ISelectable
{
    public bool Deselect()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        return true;
    }

    public bool Select()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
