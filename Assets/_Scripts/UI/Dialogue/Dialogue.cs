using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI.Dialogue
{
    public class Line //going to set this up this way so we can extend the line class to include additional info, i.e. Speaker, options, etc.
    {
        public string text;
    }

    public class Dialogue : MonoBehaviour
    {
        Transform m_player;
        Transform m_talkingNPC;

        [SerializeField]
        TextAsset m_convoJSON;

        Line[] m_lines;

        private void Awake()
        {
            m_lines = JsonUtility.FromJson<Line[]>(m_convoJSON.text);
        }

    }
}
