using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMove : PlayerAbility
{
    private float _hAxis;
    private float _vAxis;
    private Vector3 _moveVector;
    public float MoveSpeed;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _hAxis = Input.GetAxisRaw("Horizontal");
        _vAxis = Input.GetAxisRaw("Vertical");
        _moveVector = new Vector3(_hAxis, 0, _vAxis).normalized;
        _animator.SetBool("isRun", _moveVector != Vector3.zero );
        transform.position += _moveVector * MoveSpeed * Time.deltaTime;
        MoveAndRotate();
    }

    void MoveAndRotate()
    {
        if (_moveVector != Vector3.zero)
        {
            // 이동 애니메이션 설정
            _animator.SetBool("isRun", true);

            Quaternion targetRotation = Quaternion.LookRotation(_moveVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            transform.position += _moveVector * MoveSpeed * Time.deltaTime;
        }
        else
        {
            _animator.SetBool("isRun", false);
        }
    }


}
