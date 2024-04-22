using System.Collections;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class AfflictedLogic : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private float _timeOutOfSightToDeAgro = 5.0f;
    [SerializeField] private float _randomPointRadius = 2.0f;
    [SerializeField] private float _attackRange = 0.75f;
    [SerializeField] private float _attackSpeed = 1.0f;

    [SerializeField] private AudioClip[] _idleSounds;
    [SerializeField] private AudioClip[] _attackSounds;

    [Header("Debugging")]
    [SerializeField] private bool _enableGizmos = false;
    [SerializeField] private float _textYOffset = 2.5f;

    private const int MAX_RANDOM_PATH_RETRIES = 60;
    private const float RANDOM_COOLDOWN_DURATION = 1f;

    private Transform _playerTransform;
    private AIPath _aiPath;
    private IAstarAI _ai;
    private Animator _animator;

    private float _timeOutOfSight = 0.0f;
    private Coroutine _randomPathCooldownCoroutine;
    private Coroutine _attackCoroutine;

    private enum State
    {
        Idle,
        RandomMovement,
        AgroingPlayer,
        Attacking
    }

    private State _state = State.RandomMovement;

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;
        _aiPath = GetComponent<AIPath>();
        _ai = GetComponent<IAstarAI>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        InvokeRepeating("PlayIdleSounds", 3f, 3);
    }

    private void Update()
    {
        HandleAgro();
        CheckIfPlayerInRange();

        switch (_state)
        {
            case State.Idle:
                _animator.SetBool("isIdling", true);
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isAttacking", false);
                _ai.SetPath(null);
                break;
            case State.RandomMovement:
                _animator.SetBool("isIdling", false);
                _animator.SetBool("isWalking", true);
                _animator.SetBool("isAttacking", false);
                MoveToRandomPoint();
                break;
            case State.AgroingPlayer:
                _animator.SetBool("isIdling", false);
                _animator.SetBool("isWalking", true);
                _animator.SetBool("isAttacking", false);
                SetDestinationToPlayer();
                break;
            case State.Attacking:
                _animator.SetBool("isIdling", false);
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isAttacking", true);
                _attackCoroutine ??= StartCoroutine(Attack());
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!_enableGizmos)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _randomPointRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.white;
        Vector3 textPosition = new Vector3(transform.position.x, transform.position.y + _textYOffset, transform.position.z);
#if UNITY_Editor
        switch (_state)
        {
            case State.RandomMovement:
                Handles.Label(textPosition, "State: Random Movement");
                break;
            case State.AgroingPlayer:
                Handles.Label(textPosition, $"State: Agroing Player\nTime till DeAgro: {_timeOutOfSight.ToString("F2")} / {_timeOutOfSightToDeAgro.ToString("F2")}");
                break;
            case State.Attacking:
                Handles.Label(textPosition, "State: Attacking");
                break;
        }
#endif
    }

    private void MoveToRandomPoint()
    {   
        if (!_ai.pathPending && (_ai.reachedEndOfPath || !_ai.hasPath)) {
            Vector3 _randomPoint = PickRandomPoint();
            GraphNode node1 = AstarPath.active.GetNearest(transform.position, NNConstraint.Walkable).node;
            GraphNode node2 = AstarPath.active.GetNearest(_randomPoint, NNConstraint.Walkable).node;

            for (int i = 0; i < MAX_RANDOM_PATH_RETRIES; i++)
            {
                if (PathUtilities.IsPathPossible(node1, node2))
                {
                    _ai.destination = _randomPoint;
                    _ai.SearchPath();
                    return;
                }
                _randomPoint = PickRandomPoint();
                node2 = AstarPath.active.GetNearest(_randomPoint, NNConstraint.Walkable).node;
            }
            _randomPathCooldownCoroutine ??= StartCoroutine(StartRandomPathCooldown());
        }
    }

    private Vector3 PickRandomPoint ()
    {
        Vector3 point = Random.insideUnitSphere * _randomPointRadius;
        point.y = 0;
        point += _ai.position;
        return point;
    }

    private IEnumerator StartRandomPathCooldown()
    {   
        _state = State.Idle;
        yield return new WaitForSeconds(RANDOM_COOLDOWN_DURATION);
        _state = State.RandomMovement;
        _randomPathCooldownCoroutine = null;
    }

    private void HandleAgro()
    {
        if (PlayerInSight()) // Agro
        {
            _timeOutOfSight = 0.0f; // Refresh time out of sight if in sight

            if (_state == State.AgroingPlayer)
                return;
            if (_randomPathCooldownCoroutine != null)
                StopCoroutine(_randomPathCooldownCoroutine);
            _state = State.AgroingPlayer;
        }
        else // DeAgro
        {
            if (_state == State.RandomMovement)
                return;
            _timeOutOfSight += Time.deltaTime;
            
            if (_timeOutOfSight < _timeOutOfSightToDeAgro) // Switch to Random Movement if out of sight for X time
                return;
            _timeOutOfSight = 0.0f;
            _ai.SetPath(null);
            _state = State.RandomMovement;
        }
    }

    private bool PlayerInSight()
    {
        Vector3 directionToPlayer = _playerTransform.position - transform.position;

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag(_playerTag))
            {
                return true;
            }
        }
        return false;
    }

    private void SetDestinationToPlayer()
    {
        _aiPath.destination = _playerTransform.position;
    }

    private void CheckIfPlayerInRange()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) < _attackRange)
            _state = State.Attacking;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(_attackSpeed);
        if (Vector3.Distance(transform.position, _playerTransform.position) < _attackRange)
        {
            HealthManager.Instance.Damage(1.0f);
            PlayRandomAudioClip(_attackSounds);
        }
        _attackCoroutine = null;
    }

    private void PlayIdleSounds()
    {
        PlayRandomAudioClip(_idleSounds);
    }

    public void PlayRandomAudioClip(AudioClip[] audioClips)
    {
        if (audioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            AudioSource.PlayClipAtPoint(audioClips[randomIndex], transform.position, 0.1f);
        }
    }
}
