using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour
{
    public GreedBestFirstSearch gd;
    public List<Cell> node1;
    public float speed;
    public int nodeIndex = 0;
    public Vector3 offSetPos;
    Rigidbody rb;
    public float enemyHeath;
    public Cell currentCell;
    [HideInInspector] public List<IEnemyState> enemyStates;
    public IEnemyState currentState;
    void Awake()
    {
        gd = FindAnyObjectByType<GreedBestFirstSearch>();
        enemyStates = new List<IEnemyState>();
        StunState stunState = new StunState(this);
        MoveState moveState = new MoveState(this, gd, offSetPos, speed);
        DeathState deathState = new DeathState(this);

        enemyStates.Add(stunState);
        enemyStates.Add(moveState);
        enemyStates.Add(deathState);

        currentState = enemyStates[1];
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyHeath <= 0)
        {
            ChangeState(enemyStates[2]);
        }
        currentState?.Update();
        

    }
    #region Deprecated
    // void GetPath()
    // {


    //     if (node1.Count == 0)
    //     {
    //         node1 = gd.path;
    //         currentCell = node1[nodeIndex];
    //         transform.LookAt(currentCell.transform);
    //     }
    //     else
    //     {
    //         Move();
    //     }

    // }
    // void Move()
    // {
    //     Vector3 enemyPos = currentCell.transform.position + offSetPos;
    //     Vector3 dir = enemyPos - transform.position;
    //     Quaternion target = Quaternion.LookRotation(dir);
    //     transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * speed);
    //     transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //     if (Vector3.Distance(transform.position, currentCell.transform.position) <= 1.5f)
    //     {
    //         NewNode();
    //     }
    // }
    // void NewNode()
    // {
    //     if (nodeIndex < node1.Count)
    //     {
    //         nodeIndex++;
    //         currentCell = node1[nodeIndex];
    //     }
    // }
#endregion
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("In"))
        {
            transform.gameObject.tag = "Enemy";

        }

        if (collider.CompareTag("Bullet"))
        {
            ChangeState(enemyStates[0]);
        }
        

        if (collider.CompareTag("Finish"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("EndScene");
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
