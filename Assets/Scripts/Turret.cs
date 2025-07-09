using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    Transform partToRotate;
    GameObject target;
    public float range;
    public float speedRotate;
    public float fireRate = 0;
    private float fireCountDown = 0;
    private Transform shootPoint;
    public GameObject bulletPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        partToRotate = transform.Find("Rotate");
        shootPoint = partToRotate.transform.Find("ShootPoint");
        InvokeRepeating("TurretUpdate",0f,0.1f);
    }
    void TurretUpdate()
    {
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearEnemy = null;
        foreach(GameObject game in allEnemy)
        {
            float enemyDistance = Vector3.Distance(transform.position,game.transform.position);
            if(enemyDistance < shortestDistance)
            {
                shortestDistance = enemyDistance;
                nearEnemy = game;
            }
        }
        if(allEnemy != null && shortestDistance <= range)
        {
            target = nearEnemy;
            
        }else
        {
            target = null;  
        }
        
    }

    void FixedUpdate()
    {
        fireCountDown -= Time.deltaTime;
        if(target == null) return;
        Vector3 toTarget = target.transform.position - transform.position;
        Quaternion look = Quaternion.LookRotation(toTarget);
        partToRotate.transform.rotation = Quaternion.Lerp(partToRotate.transform.rotation,look,Time.deltaTime * speedRotate);
        partToRotate.transform.rotation = Quaternion.Euler(0, partToRotate.transform.eulerAngles.y,0);
        
        // Debug.Log(Quaternion.Angle(look, partToRotate.transform.rotation));
        if(fireCountDown <= 0 && Quaternion.Angle(look, partToRotate.transform.rotation) <= 14f )
        {
            Shoot();
            fireCountDown = fireRate;
        }
        
    }
    void Shoot()
    {
        GameObject that = Instantiate (bulletPrefab,shootPoint.position,shootPoint.rotation);
        Bullet bullet = that.GetComponent<Bullet>();
        if(bullet != null)
        {
            bullet.GetTarget(target);
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }
}
