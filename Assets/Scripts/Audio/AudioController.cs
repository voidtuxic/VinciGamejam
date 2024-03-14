using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Void.Core.Events;
using Zenject;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private BGMData[] bgm;
    [SerializeField] private SFXData[] sfx;
    
    private IEventBus eventBus;
    private Dictionary<BGMType, AudioClip> bgmDictionary;
    private Dictionary<SFXType, AudioClip> sfxDictionary;

    private void Awake()
    {
        bgmDictionary = bgm.ToDictionary(d => d.Type, d => d.Clip);
        sfxDictionary = sfx.ToDictionary(d => d.Type, d => d.Clip);
    }
    
    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
        eventBus.SubscribeEvent<AudioEvent.PlayBGM>(OnPlayBGMEvent);
        eventBus.SubscribeEvent<AudioEvent.PlaySFX>(OnPlaySFXEvent);
    }

    private void OnPlayBGMEvent(AudioEvent.PlayBGM evt)
    {
        bgmSource.clip = bgmDictionary[evt.Type];
        bgmSource.Play();
    }

    private void OnPlaySFXEvent(AudioEvent.PlaySFX evt)
    {
        sfxSource.PlayOneShot(sfxDictionary[evt.Type]);
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<AudioEvent.PlayBGM>(OnPlayBGMEvent);
        eventBus.UnsubscribeEvent<AudioEvent.PlaySFX>(OnPlaySFXEvent);
    }
}