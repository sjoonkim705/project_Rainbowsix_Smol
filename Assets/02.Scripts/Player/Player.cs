using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitable
{
    public Stat stat;
    public static Player instance;
    private CinemachineImpulseSource _impulseSource;



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
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        stat.Init();
    }
    public void Hit(int damage, Vector3 hitPosition)
    {
        stat.Health -= damage;
        _impulseSource.GenerateImpulse(0.2f);
        Animator.SetTrigger("Hit");
        transform.Translate((transform.position - hitPosition).normalized/2);

        UI_DamageScreen.Instance.Damaged();
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
