using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryEggsMode : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Eggs")==null)
        {
            Debug.Log("You Won");
        }
    }
}
