using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void Enter();
    void Update();
    void Exit();
}


public class StunState : IEnemyState
{
    private EnemyMove enemy;
    private float stunTime;

    public StunState(EnemyMove e) => enemy = e;
    public void Enter()
    {
        Debug.Log("GetStun");
        stunTime = 1.5f;
    }

    public void Update()
    {
        stunTime -= Time.deltaTime;
        if (stunTime <= 0)
        {
            enemy.ChangeState(enemy.enemyStates[1]); //MoveState
        }
    }

    public void Exit()
    {

    }
}

public class MoveState : IEnemyState
{
    private EnemyMove enemy;
    private GreedBestFirstSearch gd;
    private Vector3 offSetPos;
    private List<Cell> path;
    private Cell currentCell;
    private int currenNode;
    private float speed;
    public MoveState(EnemyMove e, GreedBestFirstSearch alg, Vector3 offPos, float sp)
    {
        enemy = e;
        gd = alg;
        offSetPos = offPos;
        currenNode = 0;
        speed = sp;
        path = new List<Cell>();
    }

    public void Enter()
    {
        if (gd == null)
            Debug.LogError("gd is null");
        if (path == null)
            Debug.LogError("path is null");
    }

    public void Update()
    {
        GetNode();
    }

    void GetNode()
    {
        if (path.Count == 0)
        {
            path = gd.path;
            currentCell = path[currenNode];
            enemy.transform.LookAt(currentCell.transform);
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        Vector3 enemyPos = currentCell.transform.position + offSetPos;
        Vector3 dir = enemyPos - enemy.transform.position;
        Quaternion target = Quaternion.LookRotation(dir);
        enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, target, Time.deltaTime * speed);
        enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Vector3.Distance(enemy.transform.position, currentCell.transform.position) <= 1.5f)
        {
            NewNode();
        }
    }
    void NewNode()
    {
        if (currenNode < path.Count)
        {
            currenNode++;
            currentCell = path[currenNode];
        }
    }

    public void Exit()
    {

    }
}

public class DeathState : IEnemyState
{
    private EnemyMove enemy;
    public DeathState(EnemyMove e) => enemy = e;

    public void Enter()
    {
        enemy.Die();
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}