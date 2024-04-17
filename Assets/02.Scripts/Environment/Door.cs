using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector]
    public Collider MyCollider;
    public bool IsOpen = false;
    public GameObject Screen;

    // Start is called before the first frame update
    void Start()
    {
        MyCollider = GetComponent<Collider>();
    }
    public void ActiveCollider()
    {
        StartCoroutine(Activecollider_Coroutine());
    }
    public void InactiveCollider()
    {
        StartCoroutine(Inactivecollider_Coroutine());
    }
    private IEnumerator Inactivecollider_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        MyCollider.enabled = false;
        if (Screen != null)
        {
            Destroy(Screen);
        }
        IsOpen = true;
    }
    private IEnumerator Activecollider_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        MyCollider.enabled = true;
        IsOpen = false;
    }

}
