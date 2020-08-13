using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    string m_textToDisplay;
    [SerializeField]
    Text m_textComponent;
    [SerializeField]
    float m_delayBetweenDots = .25f;
    [SerializeField]
    int m_howManyDots = 3;
    
    const string DOT = ".";
    int m_dotCounter = 0;
    float m_timeCounter = 0f;

    private void Start()
    {
        m_textComponent.text = m_textToDisplay;
        m_timeCounter = m_delayBetweenDots;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;

        m_timeCounter -= Time.deltaTime;

        if(m_timeCounter <= 0f)
        {
            m_timeCounter = m_delayBetweenDots;

            m_dotCounter = (m_dotCounter + 1) % (m_howManyDots + 1);
            m_textComponent.text = m_textToDisplay;

            for(int i = 0; i < m_dotCounter; i++)
            {
                m_textComponent.text += DOT;
            }
        }
    }
}
