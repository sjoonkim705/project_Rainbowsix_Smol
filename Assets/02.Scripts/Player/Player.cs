using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

    }
}
