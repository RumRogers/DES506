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
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Cog c in m_cogs)
        {

        }
    }
}
