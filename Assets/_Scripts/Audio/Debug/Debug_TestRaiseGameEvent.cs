using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAudio
{
    public class Debug_TestRaiseGameEvent : MonoBehaviour
    {
        string m_eventName = "";
        [SerializeField]
        Vector2 m_topLeft = new Vector2(10, 10);
        Vector2 m_labelSize = new Vector2(100, 20);
        Vector2 m_btnSize = new Vector2(100, 20);
        float m_verticalSpacing = 5f;

        private void OnGUI()
        {
            Vector2 pos = m_topLeft;

            m_eventName = GUI.TextField(new Rect(pos.x, pos.y, m_labelSize.x, m_labelSize.y), m_eventName, 25);

            pos.y += m_labelSize.y + m_verticalSpacing;

            if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Raise Event"))
            {
                AudioEventsPublisher.RaiseGameEvent(m_eventName);

            }
        }
    }
}