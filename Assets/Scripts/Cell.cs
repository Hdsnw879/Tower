using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool collapsed;
    public Tile[] tileOptions;
    Vector3 worldPosition;
    public float heuristicGreedy;
    public Cell parent;
    public Cell(Vector3 pos)
    {
        heuristicGreedy = 0;
        parent = null;  
    }

    public void CreateCell(bool collapseState, Tile[] tiles)
    {
        collapsed = collapseState;
        tileOptions = tiles;
    }

    public void RecreateCell(Tile[] tiles)
    {
        tileOptions = tiles;
    }
}
