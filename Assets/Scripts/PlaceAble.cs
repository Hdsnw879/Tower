using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceAble : MonoBehaviour
{
    public Color hoverColor;
    private Color startColor;
    private Renderer rend;
    GameObject turret;
    public Vector3 posOffset;
    // Start is called before the first frame update
    void Start()
    {
        turret = null;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }
    private void OnMouseDown()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName != ("MainGame"))
        {
            return;
        }
        if(turret != null)
        {
            Debug.Log("Can't build here");
            return;
        }
        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        turret = Instantiate(turretToBuild,transform.position + posOffset,turretToBuild.transform.rotation);
    }
    private void OnMouseEnter()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName != ("MainGame"))
        {
            return;
        }
        rend.material.color = hoverColor;
    }
    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
