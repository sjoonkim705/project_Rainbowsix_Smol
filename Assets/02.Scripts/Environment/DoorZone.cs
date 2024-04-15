using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorZone : MonoBehaviour
{
    [HideInInspector]
    public Door Door;
    public GameObject DoorOpenUI;
    [HideInInspector]
    public Animation OpenAnimation;
    private Collider TriggerCollider;
    private bool IsInterActable;
    private bool IsOpen;



    private void Awake()
    {
        Door = GetComponentInParent<Door>();
        OpenAnimation = GetComponentInParent<Animation>();
        TriggerCollider = GetComponent<Collider>();

        DoorOpenUI.SetActive(false);
    }
    private void Update()
    {
        if (IsInterActable)
        {
            if (Input.GetKeyDown(KeyCode.E) && !IsOpen)
            {
                OpenDoor();
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DoorOpenUI.SetActive(true);
            IsInterActable = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DoorOpenUI.SetActive(false);
            IsInterActable = false;
        }
    }
    public void OpenDoor()
    {
        OpenAnimation.Play();
        TriggerCollider.enabled = false;
        DoorOpenUI.SetActive(false);
        Door.InactiveCollider();
        IsOpen = true;
    }

}
