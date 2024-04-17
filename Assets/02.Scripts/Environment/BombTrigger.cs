using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrigger : MonoBehaviour
{
    [HideInInspector]
    public Animation BombAnimation;
    public GameObject EnemySpawnPoint;
    public GameObject UI_DisarmScreen;
    [HideInInspector]
    private float _disarmProcess;
    public float DisarmProcess
    {
        get { return _disarmProcess; }
        set 
        { 
            _disarmProcess = Mathf.Clamp(value, 0f, 1f);
        }

    }
    public float CompleteTime = 15f;
    private bool _isDisarming;


    private void Awake()
    {
        BombAnimation = GetComponent<Animation>();
    }
    private void Update()
    {
        if (_isDisarming)
        {
            UI_DisarmScreen.SetActive(true);
            EnemySpawnPoint.SetActive(true);

        }
        else
        { 
            UI_DisarmScreen.SetActive(false);
            EnemySpawnPoint.SetActive(false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isDisarming = true;

            BombAnimation.Play();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (_isDisarming)
        {
            DisarmProcess += Time.deltaTime / CompleteTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isDisarming = false;
            DisarmProcess *= 0.8f;
            BombAnimation.Stop();
        }
    }

}
