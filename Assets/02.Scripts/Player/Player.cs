using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitable
{
    public Stat stat;
    public static Player instance;


    [HideInInspector]
    public Animator Animator;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Animator = GetComponent<Animator>();
        stat.Init();
    }
    public void Hit(int damage, Vector3 hitPosition)
    {
        stat.Health -= damage;
        if (stat.Health <= 0)
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        Debug.Log("GameOver");
    }
}
