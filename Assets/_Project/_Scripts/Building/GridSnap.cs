using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnap : MonoBehaviour
{
    private Grid m_Grid;

    // Start is called before the first frame update
    void Start()
    {
        m_Grid = FindObjectOfType<Grid>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int cp=m_Grid.LocalToCell(transform.localPosition);
        transform.localPosition=m_Grid.GetCellCenterLocal(cp);
    }
}
