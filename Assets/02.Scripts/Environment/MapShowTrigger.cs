using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShowTrigger : MonoBehaviour
{
    public GameObject MapScreen;
    public CinemachineVirtualCamera BombStageCamera;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(MapShow_Coroutine());
        }
    }
    private IEnumerator MapShow_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(MapScreen);
        BombStageCamera.Priority = 11;
        yield return new WaitForSeconds(2f);
        BombStageCamera.Priority = 1;
    }
}
