using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public enum EnemyType
{
    Assult,
    RPG,
}
public enum EnemyState
{
    Idle,
    Patrol,
    Trace,
    Attack,
    Comeback,
    Damaged,
    Die
}
public class Enemy : MonoBehaviour, IHitable
{
    public Stat stat;
    private EnemyState _currentState;
    public EnemyType EnemyType;
    public bool IsPatrolType;
    private NavMeshAgent _navMeshAgent;
    private Vector3 _knockBackDir;
    private float _knockbackProgress = 0f;
    private Vector3 _target;
    private Collider _collider;


    [Header("Idle Variables")]
    public float StartPatrolDistance = 20f;
    public const float TOLERANCE = 0.1f;
    private Vector3 _enemyOriginPos;


    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    public float KnockbackPower = 2.0f;
    public float KnockbackDuration = 0.5f;

    private float _traceTimer = 0;
    public float TraceToComebackTime = 6f;


    public float PatrolRandomRange = 15f;
    private Vector3 _patrolDestination;

    [HideInInspector]
    public Animator Animator;


    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _currentState = EnemyState.Idle;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        stat.Init();
    }
    private void Start()
    {
        _enemyOriginPos = transform.position;
    }

    private void Update()
    {
        Debug.Log(_currentState);

        switch (_currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Trace:
                Trace();
                break;
            case EnemyState.Attack:
                StartCoroutine(Attack_Coroutine());
                Trace();
                break;
            case EnemyState.Comeback:
                Comeback();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }
    public void Hit(int damage, Vector3 position)
    {
        stat.Health -= damage;

        if (stat.Health <= 0)
        {
            Animator.SetTrigger("Die");
            _currentState = EnemyState.Die;
        }
        else
        {
            Animator.SetTrigger("Damaged");
            _knockBackDir = transform.position - position;
            _knockBackDir.y = 0;
            _knockBackDir.Normalize();
            _knockbackProgress = 0;
            _currentState = EnemyState.Damaged;
        }
    }

    private void Idle()
    {
        // 플레이어 발견시 trace
        if (Vector3.Distance(Player.instance.transform.position, transform.position) <= StartPatrolDistance)
        {
            Animator.SetTrigger("IdleToPatrol");
            _currentState = EnemyState.Patrol;
        }
    }
    void Patrol()
    {
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            MoveToRandomPosition();
        }
        if (TryRaycastToPlayer(out _target, traceRange))
        {
            _currentState = EnemyState.Trace;
        }
    }
    private float traceRange = 15f;
    void Trace()
    {
        // attackDistance가 될때까지 trace

        if (!TryRaycastToPlayer(out _target, traceRange))
        {
            _traceTimer += Time.deltaTime;
        }
        else
        {
            _navMeshAgent.SetDestination(_target);
            _navMeshAgent.stoppingDistance = 5f;
            _currentState = EnemyState.Attack;
            _traceTimer = 0;
        }
        if (_traceTimer > TraceToComebackTime)
        {
            _currentState = EnemyState.Comeback;
        }
    }
    private float attackDistance = 5f;
    private IEnumerator Attack_Coroutine()
    {
        if (TryRaycastToPlayer(out _target, attackDistance))
        {
            //AttackPlayer(); 0.8확률로 공격성공
            int randomFactor = Random.Range(0, 10);
            if (randomFactor < 9)
            {
                Player.instance.Hit(stat.Damage, transform.position);
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
        else
        {
            _currentState = EnemyState.Trace;
            yield return new WaitForSeconds(0.2f);
        }
    }


    void Damaged() // knockback effect
    {
        if (_knockbackProgress == 0)
        {
            _knockbackStartPosition = transform.position;
            _knockbackEndPosition = transform.position + _knockBackDir * KnockbackPower;
        }
        _knockbackProgress += Time.deltaTime / KnockbackDuration;
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);
        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0f;
            Animator.SetTrigger("DamagedToTrace");
            _currentState = EnemyState.Trace;
        }
    }                                                                                                                                                                                                                
    void Comeback()
    {
        _navMeshAgent.destination = _enemyOriginPos;
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Animator.SetTrigger("ComebackToIdle");
            _currentState = EnemyState.Idle;
        }
    }
    void Die()
    {
        _collider.enabled = false;
        _navMeshAgent.isStopped = true;
        StartCoroutine(Die_Coroutine());

    }

    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * PatrolRandomRange;
        randomDirection += _navMeshAgent.destination;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomDirection, out hit, PatrolRandomRange, NavMesh.AllAreas);
        Vector3 targetPosition = hit.position;
        _navMeshAgent.SetDestination(targetPosition);
        _patrolDestination = targetPosition;
    }

    private bool TryRaycastToPlayer(out Vector3 target, float distance)
    {
        Vector3 targetPos  = Player.instance.transform.position;
        Vector3 rayOrigin = transform.position;
        Vector3 rayDir =  (targetPos - rayOrigin).normalized;

        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin, rayDir, out hitInfo, distance))
        {
            target = hitInfo.point;
            return true;
        }
        else
        {
            target = Vector3.zero;
            return false;
        }

    }
}
