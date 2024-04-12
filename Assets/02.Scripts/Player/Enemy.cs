using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyType
{
    Assult,
    RPG,
}
public class Enemy : MonoBehaviour, IHitable
{
    public Stat stat;
    [HideInInspector]
    public Animator Animator;

    public EnemyType EnemyType;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
    public void Hit()
    {
        Animator.SetTrigger("Hit");
        stat.Health--;

    }
}
