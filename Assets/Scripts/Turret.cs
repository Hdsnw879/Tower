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
    private float maxTimeCharge = 10f;
    private float currentTimeCharge;
    public NormalState normalState;
    public RageState rageState;
    public ChargeState chargeState;
    private ITurretState currentState;
    void Awake()
    {
        normalState = new NormalState(this);
        rageState = new RageState(this);
        chargeState = new ChargeState(this);

        currentState = normalState;

        currentTimeCharge = 0;
    }
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
        currentTimeCharge += Time.deltaTime;
        if (currentTimeCharge >= maxTimeCharge)
        {
            ChangeState(chargeState);
            
        }
        Shooting();
        
    }

    void Shooting()
    {
        fireCountDown -= Time.deltaTime;
        if(target == null) return;
        Vector3 toTarget = target.transform.position - transform.position;
        Quaternion look = Quaternion.LookRotation(toTarget);
        partToRotate.transform.rotation = Quaternion.Lerp(partToRotate.transform.rotation,look,Time.deltaTime * speedRotate * 100);
        partToRotate.transform.rotation = Quaternion.Euler(0, partToRotate.transform.eulerAngles.y,0);

        // Debug.Log(Quaternion.Angle(look, partToRotate.transform.rotation));
        if (fireCountDown <= 0 )
        {
            currentState?.Update();
            fireCountDown = fireRate;
            currentTimeCharge = 0;
        }
    }
    public void Shoot()
    {
        GameObject that = Instantiate(bulletPrefab,shootPoint.position,shootPoint.rotation);
        Bullet bullet = that.GetComponent<Bullet>();
        if(bullet != null)
        {
            bullet.GetTarget(target);
        }
        
    }
    
    public void ShootMaxDamage()
    {
        GameObject that = Instantiate(bulletPrefab,shootPoint.position,shootPoint.rotation);
        Bullet bullet = that.GetComponent<Bullet>();
        bullet.damgePerBullet = float.MaxValue;
        if (bullet != null)
        {
            bullet.GetTarget(target);
        }
        
    }

    public void ChangeState(ITurretState newStates)
    {
        currentState?.Exit();
        currentState = newStates;
        currentState?.Enter();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
