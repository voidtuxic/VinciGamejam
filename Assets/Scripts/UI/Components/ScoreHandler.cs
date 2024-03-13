using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Void.Core.Events;
using Zenject;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    
    private IEventBus eventBus;
    private int score;

    private void Start()
    {
        label.text = "0";
    }

    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
        
        eventBus.SubscribeEvent<PlayerEvent.UpdateScore>(OnUpdateScoreEvent);
    }

    private void OnUpdateScoreEvent(PlayerEvent.UpdateScore evt)
    {
        score += evt.AddedScore;
        label.text = score.ToString();
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<PlayerEvent.UpdateScore>(OnUpdateScoreEvent);
    }
}
