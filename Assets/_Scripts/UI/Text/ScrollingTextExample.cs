using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    private void Update()
    {
        m_deltaTime += Time.deltaTime;

        if (m_deltaTime > m_textUpdateTime)
        {
            UI.Text.StringHelpers.AddNextChar(ref m_partialText, m_fullText);

            text.text = m_partialText;
            m_deltaTime = 0;
        }

    }
}
