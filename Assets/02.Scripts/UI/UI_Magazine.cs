using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;



public class UI_Magazine : MonoBehaviour
{
    public List<GameObject> Magazine = new List<GameObject>();
    public Text BulletLeft;
    public PlayerFire PlayerFire;

    void Start()
    {
        Refresh();

    }

    void LateUpdate()
    {

        Refresh();

    }
    public void Refresh()
    {
        string textColor = "green";
        if (Player.instance.stat.Ammo > 10)
        {
            textColor = "green";
        }
        else if (Player.instance.stat.Ammo > 4)
        {
            textColor = "yellow";
        }
        else if (Player.instance.stat.Ammo > 0)
        {
            textColor = "brown";
        }
        else if (Player.instance.stat.Ammo == 0)
        {
            textColor = "grey";
        }
        if (!PlayerFire.IsReloading)
        {
            BulletLeft.text = $"<color={textColor}>{Player.instance.stat.Ammo}</color>/{Player.instance.stat.MaxAmmo}";
        }
        else
        {
            BulletLeft.text = $"재장전중";
        }
        foreach(GameObject bullet in Magazine)
        {
            bullet.SetActive(false);
        }
        for (int i = 0; i < Player.instance.stat.Ammo; i++)
        {
            Magazine[i].SetActive(true);
        }
    }
}
