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

    private IEnumerator m_boxOn;
    private IEnumerator m_boxOff;

    bool shrinking = true;
    bool growing = false;

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

        m_boxOn = translateBoxes(new Vector2(0, 120));
        m_boxOff = translateBoxes(new Vector2(0, 0));
    }

    public void TurnOn()
    {
        if(shrinking == true)
        {
            StopCoroutine(m_boxOff);
            StartCoroutine(m_boxOn);
        }

        growing = true;
        shrinking = false;
    }

    public void TurnOff()
    {
        if (growing == true)
        {
            StopCoroutine(m_boxOn);
            StartCoroutine(m_boxOff);
        }

        growing = false;
        shrinking = true;
    }


    IEnumerator translateBoxes(Vector2 des)
    {
        while(true)
        {
            m_topBar.sizeDelta = Vector2.Lerp(m_topBar.sizeDelta, des, Time.deltaTime * 12);
            m_bottomBar.sizeDelta = Vector2.Lerp(m_topBar.sizeDelta, des, Time.deltaTime*12);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(m_boxOn);
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StopCoroutine(m_boxOff);
        //    StartCoroutine(m_boxOn);
        //}

        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    StopCoroutine(m_boxOn);
        //    StartCoroutine(m_boxOff);
        //}
        //StartCoroutine(translateBoxes(new Vector2(0, 0)));

        if (pressed)
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
