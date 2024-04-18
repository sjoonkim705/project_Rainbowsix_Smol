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
    public GameObject AimPoint;


    private void Start()
    {
        _owner.stat.ReloadAmmo();
    }
    private void Update()
    {

        if (GameManager.Instance.State == GameState.Playing)
        {
            AimPoint.SetActive(true);
        }
        else
        {
            AimPoint.SetActive(false);
        }

        if (_timer < 1.0f)
        {
            _timer += Time.deltaTime;
        }
        if (Input.GetMouseButton(0) && _owner.stat.Ammo > 0 && !IsReloading && GameManager.Instance.State == GameState.Playing)
        {

            _owner.Animator.SetBool("Fire", true);
            if (_timer > _owner.stat.FireSpeed)
            {
                Fire();
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Fire);
            }
        }

        if (Input.GetMouseButtonUp(0) || _owner.stat.Ammo == 0 || IsReloading)
        {
            _owner.Animator.SetBool("Fire", false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.BulletShell);
            MuzzleFlash.Stop();
        }
        if ((_owner.stat.Ammo == 0 || Input.GetKeyDown(KeyCode.R)) && !IsReloading)
        {
            StartCoroutine(Reload_Coroutine(ReloadTime));
        }
    }
    public IEnumerator Reload_Coroutine(float reloadTime)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Reload);
        IsReloading = true;
        AimPoint.SetActive(false);
        _owner.Animator.SetBool("Reload", IsReloading);
        _owner.Animator.SetBool("Fire", false);
        yield return new WaitForSeconds(reloadTime);
        _owner.stat.ReloadAmmo();
        IsReloading = false;
        AimPoint.SetActive(true);
        _owner.Animator.SetBool("Reload", IsReloading);
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

        Vector3 yOffset = new Vector3(0, 1.1f, 0);
        Ray ray = new Ray(transform.position + yOffset, transform.forward);
        RaycastHit hitInfo;
        int layermask = (-1) - (1 << LayerMask.NameToLayer("Door"));
        bool IsHit = Physics.Raycast(ray, out hitInfo, 25f,layermask);
        if (IsHit)
        {
            IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
            if (hitObject != null)
            {
                hitObject.Hit(_owner.stat.Damage, transform.position);
            }
            HitEffect.gameObject.transform.position = hitInfo.point;
            HitEffect.gameObject.transform.forward = hitInfo.normal;
            HitEffect.Play();
        }

        _timer = 0;
    }
}
