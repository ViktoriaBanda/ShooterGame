using SimpleEventBus.Disposables;
using UnityEngine;

public class FreeWalk : MonoBehaviour,IState
{
    private static readonly int IsMove = Animator.StringToHash("isMove");
    
    [SerializeField]
    private Animator _animator;
    
    [SerializeField] 
    private float _speed;

    [SerializeField] 
    private PlayerArgRegion _playerArgRegion;
    
    
    private StateMachine _stateMachine;

    private CompositeDisposable _subscriptions;
    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter()
    {
        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe<PlayerDiedEvent>(PlayerDiedEventHandler)
        };
        _playerArgRegion.OnEnemyGetIntoArgRegion += _stateMachine.Enter<Shooting>;
    }

    public void OnExit()
    {
        _subscriptions.Dispose();
        _playerArgRegion.OnEnemyGetIntoArgRegion -= _stateMachine.Enter<Shooting>;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _animator.SetBool(IsMove, true);
            transform.Translate(Vector3.back * Time.deltaTime * _speed, Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _animator.SetBool(IsMove, true);
            transform.Translate(Vector3.forward * Time.deltaTime * _speed, Space.World);
        }

        if (Input.GetKey(KeyCode.W))
        {
            _animator.SetBool(IsMove, true);
            transform.Translate(Vector3.left * Time.deltaTime * _speed, Space.World);
            return;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _animator.SetBool(IsMove, true);
            transform.Translate(Vector3.right * Time.deltaTime * _speed, Space.World);
            return;
        }
        
        _animator.SetBool(IsMove, false);
    }
    
    private void PlayerDiedEventHandler(PlayerDiedEvent obj)
    {
        _stateMachine.Enter<PlayerDeath>();
    }
}