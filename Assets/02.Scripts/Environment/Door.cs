using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector]
    public Collider MyCollider;

    // Start is called before the first frame update
    void Start()
    {
        MyCollider = GetComponent<Collider>();
    }

    // Update is called once per frame

    public void InactiveCollider()
    {
        MyCollider.enabled = false;
    }
}
