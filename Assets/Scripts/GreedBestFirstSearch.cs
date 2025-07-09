using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GreedBestFirstSearch : MonoBehaviour
{
    WaveFunctionCollapse wfc;
    public List<Cell> wayToGo;
    public List<Cell> path;
    

    void Start()
    {
        wfc = GetComponent<WaveFunctionCollapse>(); 
    }

    public void StartFindPath()
    {
        wayToGo = new List<Cell>(wfc.cellForWay);
        List<int> startIndex = wfc.wayIn;
        List<int> endIndex = wfc.gateOut;
        List<Cell> startCell = new List<Cell>();
        List<Cell> endCell = new List<Cell>();

        foreach (int index in startIndex)
        {
            startCell.Add(wayToGo[index]);
        }

        foreach (int index in endIndex)
        {
            endCell.Add(wayToGo[index]);
        }

        FindPath(startCell, endCell);
    }

    void FindPath(List<Cell> startCell, List<Cell> endCell)
    {
        List<Cell> openSet = new List<Cell>(startCell);
        HashSet<Cell> closedSet = new HashSet<Cell>();

        Cell current = openSet[0];

        foreach (Cell cell in wayToGo)
        {
            cell.heuristicGreedy =current.heuristicGreedy + Heuristic(current, cell);
        }

        while (openSet.Count > 0)
        {
            current = openSet.OrderBy(c => c.heuristicGreedy).First();
            openSet.Remove(current);
            closedSet.Add(current);

            if (endCell.Contains(current))
            {
                RetracePath(startCell[0], current);
                return;
            }

            foreach (Cell nextCell in wayToGo)
            {
                if (closedSet.Contains(nextCell)) continue;
                float distance = Heuristic(current, nextCell);
                if (distance <= 2 && !openSet.Contains(nextCell))
                {
                    nextCell.parent = current;
                    openSet.Add(nextCell);
                }
            }
            
        }
    }

    void RetracePath(Cell startCell, Cell endCell)
    {
        path = new List<Cell>();
        Cell currentCell = endCell;

        while (currentCell != startCell)
        {
            path.Add(currentCell);
            currentCell = currentCell.parent;
        }
        path.Add(startCell);
        path.Reverse();
    }

    float Heuristic(Cell firstCell, Cell secondCell)
    {
        return Vector3.Distance(firstCell.transform.position, secondCell.transform.position);
    }
    void OnDrawGizmos()
    {
        if (path != null && path.Count > 1)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                if(path[i] != null && path[i + 1]!= null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(path[i].transform.position, path[i + 1].transform.position);
                }
            }
        }
    }
}


