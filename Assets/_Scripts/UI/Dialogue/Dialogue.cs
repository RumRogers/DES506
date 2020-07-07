using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        GameObject m_TextUI;
        Text m_DialogueText;

        [SerializeField]
        TextAsset m_convoJSON;

        Line[] m_lines;

        int m_lineIndex = 0;
        float m_scrollSpeed;

        private void Awake()
        {
            m_lines = JsonUtility.FromJson<Line[]>(m_convoJSON.text);
            m_TextUI.SetActive(true);


        }

        void StartFillingLine(int index)
        {
            StartCoroutine(GameUI.Dialogue.StringHelpers.FillDialogueBox(m_DialogueText, m_lines[index].text, m_scrollSpeed));
        }

    }
}
