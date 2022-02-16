using SimpleEventBus.Disposables;
using UnityEngine;

public class BuffsManager : MonoBehaviour
{
    [SerializeField] 
    private CharacteristicManager _playerCharacteristicManager;

    private CompositeDisposable _subscriptions;

    private void Awake()
    {
        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe<BuffAchieveEvent>(BuffAchieveEventHandler)
        };
    }


    private void BuffAchieveEventHandler(BuffAchieveEvent eventData)
    {
        if(eventData.Buff.GetType() == typeof(Bomb))
        {
            return;    
        }

        var characteristic = _playerCharacteristicManager.GetCharacteristicByType(eventData.Buff.Type);
        characteristic.SetValue(characteristic.GetMaxValue());
    }

    private void OnDestroy()
    {
        _subscriptions.Dispose();
    }
}