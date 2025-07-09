using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject turretToBuild;
    public static BuildManager instance;
    public GameObject standardTurret;
    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        turretToBuild = standardTurret;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
