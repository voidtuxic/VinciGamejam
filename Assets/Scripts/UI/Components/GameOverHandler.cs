using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Void.Core.Events;
using Zenject;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    
    private IEventBus eventBus;
    
    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
        
        eventBus.SubscribeEvent<GameEvent.GameOver>(OnGameOverEvent);
    }

    private void OnGameOverEvent(GameEvent.GameOver evt)
    {
        label.text = evt.Message;
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<GameEvent.GameOver>(OnGameOverEvent);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
