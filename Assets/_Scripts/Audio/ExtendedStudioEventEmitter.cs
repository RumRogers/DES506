using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAudio
{
    [AddComponentMenu("FMOD Studio/Andrea's KICKASS Extended FMOD Studio Event Emitter!")]
    public class ExtendedStudioEventEmitter : FMODUnity.StudioEventEmitter
    {
        private void Update()
        {
            if (instance.isValid())
            {
                PLAYBACK_STATE state;
                instance.getPlaybackState(out state);
                Debug.Log($"FMOD: {state}");
                if(state == PLAYBACK_STATE.STOPPED)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}