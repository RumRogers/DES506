using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAudio
{
    [AddComponentMenu("FMOD Studio/Andrea's KICKASS Extended FMOD Studio Event Emitter!")]
    public class ExtendedStudioEventEmitter : FMODUnity.StudioEventEmitter
    {
        [SerializeField]
        List<string> m_gameEvents;
        public List<string> p_GameEvents { get => m_gameEvents; }

        private void Start()
        {
            AudioEventsPublisher.Subscribe(this);
        }
    }
}