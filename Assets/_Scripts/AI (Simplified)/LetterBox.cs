using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterBox : MonoBehaviour
{
    private RectTransform m_topBar, m_bottomBar;
    private GameObject m_topGameObj, m_bottomGameObj;

    private float counter = 0;
    private bool pressed = false;
    // Start is called before the first frame update
    void Start()
    {
        m_topGameObj = new GameObject("topBar", typeof(Image));
        m_topGameObj.transform.SetParent(transform, false);
        m_topGameObj.GetComponent<Image>().color = Color.black;

        m_bottomGameObj = new GameObject("topBar", typeof(Image));
        m_bottomGameObj.transform.SetParent(transform, false);
        m_bottomGameObj.GetComponent<Image>().color = Color.black;

        m_topBar = m_topGameObj.GetComponent<RectTransform>();
        m_topBar.anchorMin = new Vector2(0, 1);
        m_topBar.anchorMax = new Vector2(1, 1);
        m_topBar.sizeDelta = new Vector2(0, 0);

        m_bottomBar = m_bottomGameObj.GetComponent<RectTransform>();
        m_bottomBar.anchorMin = new Vector2(0, 0);
        m_bottomBar.anchorMax = new Vector2(1, 0);
        m_bottomBar.sizeDelta = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            pressed = true;
        }

        if(pressed)
        {
            counter += Time.deltaTime;

            if(counter < 2)
            {
                m_topBar.sizeDelta = new Vector2(0, counter * 50);

                m_bottomBar.sizeDelta = new Vector2(0, counter * 50);
            }


        }
    }
}
