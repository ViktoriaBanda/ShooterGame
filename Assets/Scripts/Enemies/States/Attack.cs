using System;
using SimpleEventBus.Disposables;
using UnityEngine;

public class Attack : MonoBehaviour, IState
{
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    
    private StateMachine _stateMachine;
    
    [SerializeField]
    private Animator _animator;
    
    private GameObject _player;
    
    private CompositeDisposable _subscriptions;

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _player = GetComponent<Enemy>().Player;
    }

    public void OnEnter()
    {
        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe<BulletHitEvent>(BulletHitEventHandler)
        };
        
        _animator.SetBool(IsAttack, true);
    }

    public void OnExit()
    {
        _animator.SetBool(IsAttack, false);
        
        _subscriptions.Dispose();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.PLAYER_TAG))
        {
            EventStreams.Game.Publish(new PlayerGetDamageEvent());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.PLAYER_TAG))
        {
            _stateMachine.Enter<Moving>();
        }
    }
    
    private void BulletHitEventHandler(BulletHitEvent eventData)
    {
        if (eventData.HittedObject == gameObject)
        {
            _stateMachine.Enter<Death>();
        }
    }
}