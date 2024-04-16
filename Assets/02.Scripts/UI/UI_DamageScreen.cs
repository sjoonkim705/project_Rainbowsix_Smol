using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DamageScreen : MonoBehaviour
{
    public static UI_DamageScreen Instance { get; private set; }
    public Image DamagedScreen;
    public float ShowInterval = 0.2f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DamagedScreen.enabled = false;
    }

    public void Damaged()
    {
        StartCoroutine(ShowDamagedScreen_Coroutine(ShowInterval));
    }
    private IEnumerator ShowDamagedScreen_Coroutine(float duration)
    {
        yield return new WaitForSeconds(duration/2);
        DamagedScreen.enabled = true;
        yield return new WaitForSeconds(duration/2);
        DamagedScreen.enabled = false;
        yield return new WaitForSeconds(duration / 2);
        DamagedScreen.enabled = true;
        yield return new WaitForSeconds(duration / 2);
        DamagedScreen.enabled = false;
    }
}
