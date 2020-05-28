using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Dialogue
{
    public class ScrollingTextExample : MonoBehaviour
    {
        [SerializeField]
        string m_fullText = "Hello there, this sentence needs to be longer!";
        string m_partialText;

        [SerializeField]
        float m_textUpdateTime = 0.25f;
        float m_deltaTime;

        [SerializeField]
        Text text; 

        private void Start()
        {
            m_partialText = "";
            text.text = m_partialText;

            StartCoroutine(StringHelpers.FillDialogueBox(text, m_fullText, m_textUpdateTime));
        }
    }
}