using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAudio
{
    // This is a quick implementation of the Observer Pattern, it doesn't provide an unsubscribe method for now
    // because it seems kinda pointless for audio events. Will add it if needed.
    public class AudioEventsPublisher : MonoBehaviour
    {
        static Dictionary<string, List<ExtendedStudioEventEmitter>> m_mapEventsToFMODScripts = new Dictionary<string, List<ExtendedStudioEventEmitter>>();

        public static void Subscribe(ExtendedStudioEventEmitter subscriber)
        {
            var eventIds = subscriber.p_GameEvents;

            foreach(var eventId in eventIds)
            {
                if(eventId != null && !eventId.Equals(""))
                {
                    if(!m_mapEventsToFMODScripts.ContainsKey(eventId))
                    {
                        m_mapEventsToFMODScripts.Add(eventId, new List<ExtendedStudioEventEmitter>());
                    }
                }

                m_mapEventsToFMODScripts[eventId].Add(subscriber);
            }
        }

        public static void RaiseGameEvent(string eventId)
        {
            if(m_mapEventsToFMODScripts.ContainsKey(eventId))
            {
                foreach (var subscriber in m_mapEventsToFMODScripts[eventId])
                {
                    subscriber.Play();
                }
            }
        }
    }
}