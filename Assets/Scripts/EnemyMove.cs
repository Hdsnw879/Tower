using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour
{
    
    internal enum AI
    {
        GreedyBestFirstSearch,
        AStar
    }
    [SerializeField] AI ai;
    public GreedBestFirstSearch gd;
    private WaveFunctionCollapse wfc;
    public List<Cell> node1;
    public int iOffset;
    public float speed;
    int nodeIndex = 0;
    public Vector3 offSetPos;
    Rigidbody rb;
    public float enemyHeath;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(enemyHeath<=0)
        {
            Destroy(gameObject);
        }
        switch (ai)
        {
            case AI.GreedyBestFirstSearch:
                GreedPath();
                break;
            case AI.AStar:
                break;
        }
    }
    void GreedPath()
    {
        if(gd == null)
        {
            gd = GameObject.Find("WaveFunction").GetComponent<GreedBestFirstSearch>();
            wfc = GameObject.Find("WaveFunction").GetComponent<WaveFunctionCollapse>();
            
        }
        if(node1.Count == 0)
        {
            node1 = gd.path;
            currentCell = node1 [nodeIndex];
            transform.LookAt(currentCell.transform);
            
        }else
        {
            Move();
        }

    }
    private Cell currentCell;
    void Move()
    {
        Vector3 enemyPos = currentCell.transform.position + offSetPos;
        Vector3 dir =enemyPos - transform.position;
        Quaternion target = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation,target,Time.deltaTime * speed);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if(Vector3.Distance(transform.position,currentCell.transform.position)<=1.5f)
        {
            NewNode();
        }
    }
    void NewNode()
    {
        if(nodeIndex < node1.Count)
        {
            nodeIndex++;
            currentCell = node1[nodeIndex];
        }
    }
   
   void OnTriggerEnter(Collider collider)
   {
        if(collider.CompareTag("In"))
        {
            transform.gameObject.tag = "Enemy";
        }
        if(collider.CompareTag("Finish"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("EndScene");
        }
   }
}
