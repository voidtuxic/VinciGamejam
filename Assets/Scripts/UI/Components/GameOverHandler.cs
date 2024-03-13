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
        
        eventBus.SubscribeEvent<PlayerEvent.GameOver>(OnGameOverEvent);
    }

    private void OnGameOverEvent(PlayerEvent.GameOver evt)
    {
        label.text = evt.Message;
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
