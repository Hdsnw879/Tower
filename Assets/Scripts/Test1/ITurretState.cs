using System.Collections;
using UnityEngine;

public interface ITurretState
{
    public void Enter();
    public void Update();
    public void Exit();
}

public class NormalState : ITurretState
{
    private Turret turret;
    private int currentShoot;
    private int MaxShoot;
    public NormalState(Turret t) => turret = t;
    public void Enter()
    {
        currentShoot = 0;
        MaxShoot = 5;
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
       
        turret.Shoot();
        currentShoot++;
        Debug.Log($"NormalShoot {currentShoot}");
        if (currentShoot >= MaxShoot)
        {
            turret.ChangeState(turret.rageState);
        }
    }
}


public class RageState : ITurretState
{
    private Turret turret;
    public RageState(Turret t) => turret = t;
    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        Debug.Log("RageShoot");
        Shoot3Time();
        turret.ChangeState(turret.normalState);
    }

    IEnumerator Shoot3Time()
    {
        for (int i = 0; i < 3; i++)
        {
            turret.Shoot();
            yield return new WaitForSeconds(.3f);
        }
    }
}

public class ChargeState : ITurretState
{
    private Turret turret;
    public ChargeState(Turret t) => turret = t;
    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        Debug.Log("ChargeShoot");
        turret.ShootMaxDamage();
        turret.ChangeState(turret.normalState);
    }
}
