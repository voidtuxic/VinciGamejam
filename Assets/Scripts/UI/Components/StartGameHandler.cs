using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Void.Core.Events;
using Zenject;

public class StartGameHandler : MonoBehaviour
{
    private IEventBus eventBus;
    
    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }
    
    public void StartGame()
    {
        gameObject.SetActive(false);
        eventBus.PublishEvent(new GameEvent.StartGame());
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
