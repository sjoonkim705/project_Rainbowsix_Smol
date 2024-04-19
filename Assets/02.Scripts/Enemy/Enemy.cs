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
    public bool IsStaticType;
    private NavMeshAgent _navMeshAgent;
    private Vector3 _knockBackDir;
    private float _knockbackProgress = 0f;
    private Vector3 _target;
    private Collider _collider;
    public ParticleSystem MuzzleFlash;


    [Header("Idle Variables")]
    public float StartPatrolDistance = 20f;
    public const float TOLERANCE = 0.2f;
    private Vector3 _enemyOriginPos;


    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    public float KnockbackPower = 2.0f;
    public float KnockbackDuration = 0.1f;
    private bool _isInKnockbackProcess;

    private float _traceToComebackTimer = 0;
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
                Attack();
                break;
            case EnemyState.Comeback:
                Comeback();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                DieState();
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
            Die();
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
    private float _idleTimer = 0;
    public float Idletime = 5f;
    private void Idle()
    {
        // 플레이어 발견시 trace
        _idleTimer += Time.deltaTime;
        if (_idleTimer <= Idletime && !IsStaticType)
        {
            Animator.SetTrigger("IdleToPatrol");
            _currentState = EnemyState.Patrol;
            _idleTimer = 0;
        }
        if (TryRaycastToPlayer(out _target, _traceRange, false))
        {
            Animator.SetTrigger("IdleToTrace");
            _currentState = EnemyState.Trace;
            _idleTimer = 0;
        }

    }
    private float _patrolTimer = 0;

    void Patrol()
    {
        _patrolTimer += Time.deltaTime;
        _navMeshAgent.isStopped = false;
        if (_patrolTimer > 10f)
        {
            _currentState = EnemyState.Comeback;
            Animator.SetTrigger("PatrolToComeback");
            _patrolTimer = 0;
        }
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            MoveToRandomPosition();
        }
        if (TryRaycastToPlayer(out _target, _traceRange, false))
        {
            _currentState = EnemyState.Trace;
        }
    }
    private float _traceRange = 20f;
    private float _traceToAttackTimer = 0;
    void Trace()
    {
        // attackDistance가 될때까지 trace
        _navMeshAgent.isStopped = false;
        if (!TryRaycastToPlayer(out _target, _traceRange, false))
        {
            _traceToComebackTimer += Time.deltaTime;
        }
        else
        {
            _traceToAttackTimer += Time.deltaTime;
            _navMeshAgent.SetDestination(_target); //
            if (_traceToAttackTimer > 0.5f)
            {
                _currentState = EnemyState.Attack;
                _traceToAttackTimer = 0;
                _attackTimer = 0;
            }
            _traceToComebackTimer = 0;
        }
        if (_traceToComebackTimer > TraceToComebackTime)
        {
            Animator.SetTrigger("TraceToComeback");
            _currentState = EnemyState.Comeback;
            _comebackTimer = 0;
        }
    }
    public float AttackDistance = 15f;
    private float _attackTimer = 0f;
    private float _attackAngleThershold = 30f;

    private void Attack()
    {
        _navMeshAgent.isStopped = true;
        if (_attackTimer == 0)
        {
            TryRaycastToPlayer(out _target, AttackDistance, true);
            PlayFireAnimation();
            Vector3 directionToPlayer = Player.instance.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(forward, directionToPlayer);

            if (angle <= _attackAngleThershold && directionToPlayer.magnitude < 15f)
            {
                Player.instance.Hit(stat.Damage, transform.position);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.EnemyFire);
            }
        }
            _attackTimer += Time.deltaTime;
        

        if (_attackTimer > 1f)
        {

            Animator.SetTrigger("AttackToTrace");
            _currentState = EnemyState.Trace;
            _attackTimer = 0;
        }

    }

    private void PlayFireAnimation()
    {
        Animator.SetTrigger("Attack");
        if (!MuzzleFlash.isPlaying)
        {
            StartCoroutine(MuzzleFlash_Coroutine());
        }
    }
    private IEnumerator MuzzleFlash_Coroutine()
    {
        MuzzleFlash.Play();
        yield return new WaitForSeconds(0.2f);
        MuzzleFlash.Stop();
    }

    void Damaged()
    {
        if (_knockbackProgress == 0)
        {

            _knockbackStartPosition = transform.position;
            _knockbackEndPosition = transform.position + _knockBackDir * KnockbackPower;
        }
        _knockbackProgress += Time.deltaTime / KnockbackDuration;
        transform.position = Vector3.Lerp(transform.position, _knockbackEndPosition, _knockbackProgress);
        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0f;
            Animator.SetTrigger("DamagedToTrace");
            _currentState = EnemyState.Trace;
        }
    }
    private float _comebackTimer = 0f;
    void Comeback()
    {
        if (_comebackTimer == 0)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.destination = _enemyOriginPos;
        }
        _comebackTimer += Time.deltaTime;
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Animator.SetTrigger("ComebackToIdle");
            _currentState = EnemyState.Idle;
            _comebackTimer = 0;
            _idleTimer = 0;
        }
    }
    void Die()
    {
        _collider.enabled = false;
        _knockbackProgress = 1;
        _navMeshAgent.isStopped = true;
        StartCoroutine(Die_Coroutine());

    }
    void DieState()
    {

    }

    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        _collider.enabled = false;
        yield return new WaitForSeconds(1f);
        ItemObjectFactory.Instance.MakePercent(transform.position);
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

    private bool TryRaycastToPlayer(out Vector3 target, float distance, bool IsAttackMode)
    {
        Vector3 yOffset = new Vector3(0, 1.5f, 0);
        if (IsAttackMode)
        {
            yOffset.y = 1.0f;
        }
        Vector3 targetPos  = Player.instance.transform.position;
        Vector3 rayOrigin = transform.position + yOffset;
        Vector3 rayDir =  (targetPos - rayOrigin).normalized;

        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin, rayDir, out hitInfo, distance))
        {
            if (hitInfo.collider.CompareTag("Player"))
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
        else
        {
            target = Vector3.zero;
            return false;
        }

    }
}
