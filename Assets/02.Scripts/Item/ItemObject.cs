using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public enum ItemState
{
    Idle,
    Trace,
    Take
}
public class ItemObject : MonoBehaviour
{
    private ItemState _currentstate = ItemState.Idle;
    public ItemType ItemType;


    private float _pickupDistance = 1f;

    public float _drawSpeed = 5.0f;
    private float _drawProgress = 0;
    private const float DRAW_DURATION = 0.3f;

    private Vector3 _drawStartPosition;
    private Vector3 _drawEndPosition;

    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다. (도형이나 색깔 다르게해서 구별되게)
    // Todo 2. 플레이어와 일정 거리가 되면 아이템이 먹어지고 사라진다.
    // 실습 과제31. 몬스터가 죽으면 아이템이 드랍 (Health:20%, Stamina: 20, bullet 10%)
    // 실습 과제32. 일정 거리가 되면 아이템이 베지어 곡선으로 날아온다.


    public void Init()
    {
        _traceCoroutine = null;
        _drawProgress = 0;
    }
    private void Idle()
    {
        Vector3 playerPosition = Player.instance.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        if (distanceToPlayer <= _pickupDistance)
        {
            _currentstate = ItemState.Trace;
        }
    }


    private Coroutine _traceCoroutine;
    private void Trace()
    {

        Vector3 playerPosition = Player.instance.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        if (_traceCoroutine == null)
        {
            _traceCoroutine = StartCoroutine(Trace_Coroutine());
        }
        if (distanceToPlayer > _pickupDistance)
        {
            _currentstate = ItemState.Idle;
            StopCoroutine(_traceCoroutine);
            _drawProgress = 0;
            _traceCoroutine = null;
        }
    }
    private IEnumerator Trace_Coroutine()
    {
       
        _drawStartPosition = transform.position;
        _drawEndPosition = Player.instance.transform.position;

        while (_drawProgress <= 0.6)
        {
            _drawProgress += Time.deltaTime / DRAW_DURATION;
            transform.position = Vector3.Slerp(_drawStartPosition, _drawEndPosition, _drawProgress);
            yield return null;
        }
        if (ItemType == ItemType.Health)
        {
            Player.instance.stat.Health += 10;
        }
        else if (ItemType == ItemType.Credit)
        {
            UI_Score.Instance.Score++;
        }
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Item);
        gameObject.SetActive(false);
    }

    void Update()
    {
        switch (_currentstate)
        {
            case ItemState.Idle:
                Idle();
                break;
            case ItemState.Trace:
                Trace();
                break;
            case ItemState.Take:
                break;

        }
    }

}
