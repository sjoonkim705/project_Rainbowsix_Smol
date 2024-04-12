using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFire : PlayerAbility
{
    private float _timer = 0;
    public float ReloadTime = 3f;
    public ParticleSystem MuzzleFlash;
    public ParticleSystem HitEffect;
    public bool IsReloading;


    private void Start()
    {
        _owner.stat.ReloadAmmo();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            _owner.stat.MaxAmmo = 100;
            _owner.stat.ReloadAmmo();
        }
        if (_timer < 1.0f)
        {
            _timer += Time.deltaTime;
        }
        if (Input.GetMouseButton(0) && _owner.stat.Ammo > 0)
        {

            _owner.Animator.SetBool("Fire", true);
            if (_timer > _owner.stat.FireSpeed)
            {
                Fire();
            }
        }

        if (Input.GetMouseButtonUp(0) || _owner.stat.Ammo == 0)
        {
            _owner.Animator.SetBool("Fire", false);
            MuzzleFlash.Stop();
        }
        if (_owner.stat.Ammo == 0 && !IsReloading)
        {
            IsReloading = true;
            StartCoroutine(Reload_Coroutine(ReloadTime));
        }

    }
    public IEnumerator Reload_Coroutine(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        _owner.stat.ReloadAmmo();
        IsReloading = false;
    }

    private void Fire()
    {
        if (_owner.stat.Ammo > 0)
        {
            _owner.stat.Ammo--;
        }
        else
        {
            return;
        }

        if (!MuzzleFlash.isPlaying)
        {
            MuzzleFlash.Play();
        }

        Vector3 yOffset = new Vector3(0, 1.5f, 0);
        Ray ray = new Ray(transform.position+yOffset, transform.forward);
        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction);
        bool IsHit = Physics.Raycast(ray, out hitInfo);
        Debug.Log(hitInfo.point);
        if (IsHit)
        {
            IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
            if (hitObject != null)
            {
                Debug.Log("Hit");
                hitObject.Hit();
            }
            HitEffect.gameObject.transform.position = hitInfo.point;
            HitEffect.gameObject.transform.forward = hitInfo.normal;
            HitEffect.Play();
        }

        _timer = 0;
    }
}
