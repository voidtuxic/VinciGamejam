using System.Collections.Generic;
using System.Linq;
using Void.Events;
using UnityEngine;
using Zenject;

namespace Void.Audio
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private BGMData[] bgm;
        [SerializeField] private SFXData[] sfx;
    
        private IEventBus _eventBus;
        private Dictionary<BGMType, AudioClip> _bgmDictionary;
        private Dictionary<SFXType, AudioClip> _sfxDictionary;

        private void Awake()
        {
            _bgmDictionary = bgm.ToDictionary(d => d.Type, d => d.Clip);
            _sfxDictionary = sfx.ToDictionary(d => d.Type, d => d.Clip);
        }
    
        [Inject]
        private void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            eventBus.SubscribeEvent<AudioEvent.PlayBGM>(OnPlayBGMEvent);
            eventBus.SubscribeEvent<AudioEvent.PlaySFX>(OnPlaySFXEvent);
        }

        private void OnPlayBGMEvent(AudioEvent.PlayBGM evt)
        {
            bgmSource.clip = _bgmDictionary[evt.Type];
            bgmSource.Play();
        }

        private void OnPlaySFXEvent(AudioEvent.PlaySFX evt)
        {
            sfxSource.PlayOneShot(_sfxDictionary[evt.Type]);
        }

        private void OnDestroy()
        {
            _eventBus.UnsubscribeEvent<AudioEvent.PlayBGM>(OnPlayBGMEvent);
            _eventBus.UnsubscribeEvent<AudioEvent.PlaySFX>(OnPlaySFXEvent);
        }
    }
}