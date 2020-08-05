using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogManager : MonoBehaviour
{
    [Header("Scale Factors")]
    [SerializeField]
    private float m_smallScaleFactor;

    [SerializeField]
    private float m_largeScaleFactor;

    [Header("Cog Array")]
    [SerializeField]
    private Cog[] m_cogs;

    [Header("Cog Attributes")]
    [SerializeField]
    private float m_speed;
    
    [SerializeField]
    private bool m_tick;

    [Header("Door")]
    [SerializeField]
    private GameObject m_door;

    [SerializeField]
    private float m_doorSpeed;

    private bool m_complete = false;
    private bool m_doorClosed = true;
    private float m_counter = 0.0f;
    private float m_finalX = 0.0f;
    private Quaternion m_globRotation;
    private IEnumerator m_openFunc;

    // Start is called before the first frame update
    void Start()
    {
        if (m_cogs.Length < 2)
            Debug.LogError("Please add more cogs to the manager");

        bool m_directionCheck = true;

        foreach (Cog c in m_cogs)
        {
            c.SetUp(m_directionCheck, m_tick, m_smallScaleFactor, m_largeScaleFactor, m_speed);

            //Invert the direction to have them alternate
            m_directionCheck = !m_directionCheck;
        }

        m_finalX = m_door.transform.position.x + 40f;
    }

    // Update is called once per frame
    void Update()
    {
        //Get rotation of root cog
        m_globRotation = m_cogs[0].GlobalRotation;
        
        m_complete = true;  

        foreach (Cog c in m_cogs)
        {
            c.GlobalRotation = m_cogs[0].transform.rotation;

            if (!c.IsCorrect())
                m_complete = false;
        }

        if(m_complete)
        {
            m_openFunc = OpenDoor();
            StartCoroutine(m_openFunc);
        }

        else if(!m_complete && m_openFunc != null)
        {
            StopCoroutine(m_openFunc);
        }
    }

    IEnumerator OpenDoor()
    {
        while (m_doorClosed)
        {
            m_counter += Time.deltaTime * m_doorSpeed;

            m_door.transform.position += new Vector3(Time.deltaTime * -m_doorSpeed, 0, 0);

            if (m_door.transform.position.x >= m_finalX)
                m_doorClosed = false;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
