using UnityEngine;
using UnityEngine.AI;

public class MovingState : MonoBehaviour, IState
{
    private static readonly int IsMove = Animator.StringToHash("isMove");
    
    private StateMachine _stateMachine;
    
    [SerializeField]
    private Animator _animator;
    
    [SerializeField]
    private NavMeshAgent _navMeshAgent;

    private GameObject _player;

    private bool _isMoving;


    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _player = GetComponent<EnemyController>().Player;
    }

    public void OnEnter()
    {
        _animator.SetBool(IsMove, true);
        _isMoving = true;
    }

    public void OnExit()
    {
        _isMoving = false;
    }

    private void Update()
    {
        if (!_isMoving)
        {
            return;
        }
        
        _navMeshAgent.destination = _player.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.PLAYER_TAG))
        {
            EventStreams.Game.Publish(new EnemyCollisionEvent(collision.gameObject));
            _stateMachine.Enter<AttackState>();
        }
    }
}
