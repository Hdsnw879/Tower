using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject target;
    public float speed;
    public float damgePerBullet;
    EnemyMove heath;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GetTarget(GameObject target_)
    {
        target = target_;
        heath = target.gameObject.GetComponent<EnemyMove>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(target != null)
        {
            Vector3 dir = target.transform.position + new Vector3(0,1,0) - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime,Space.World);
            if(dir.magnitude <= 1f)
            {
                heath.enemyHeath -= damgePerBullet;
                Destroy(gameObject);
            }
            }else
            {
                Destroy(gameObject);
            }
    }
    
    
}
