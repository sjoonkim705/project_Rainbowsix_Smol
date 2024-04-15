using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    private int _health;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {

            _health = value;
            if (_health > MaxHealth)
            {
                _health = MaxHealth;
            }
            else if (_health < 0)
            {
                _health = 0;
            }
        }
    }
    public int MaxHealth;
    private int _ammo;
    public int Damage;

    public int Ammo
    {
        get
        {
            return _ammo;
        }
        set
        {
            _ammo = value;
            if (_ammo > MaxAmmo)
            {
                _ammo = MaxAmmo;
            }
        }
    }
    public int MaxAmmo;

    public float FireSpeed;
    public void Init()
    {
        Health = MaxHealth;
    }
    public void ReloadAmmo()
    {
        Ammo = MaxAmmo;
    }

}
