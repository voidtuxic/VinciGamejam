using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Void.Core.Events;
using Zenject;

public class GameOverHandler : MonoBehaviour
{
    private IEventBus eventBus;
    
    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
        
        eventBus.SubscribeEvent<PlayerEvent.GameOver>(OnGameOverEvent);
    }

    private void OnGameOverEvent(PlayerEvent.GameOver evt)
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<PlayerEvent.GameOver>(OnGameOverEvent);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
