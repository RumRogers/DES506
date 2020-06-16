using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeFireFlyState : State
{
    private GameObject m_scaledObject;
    private float m_size;
    private float m_maxSize;
    static private float s_timer;
    private float m_randSpeed = Random.Range(0.6f, 4);
    /// <summary>
    /// Constructor for the day state, requires the automaton owner and the light source that influences the skybox
    /// </summary>
    /// <param name="owner">The Day/Night entity</param>
    /// <param name="scaleObj">The object you wish to scale</param>
    public LargeFireFlyState(Automaton owner, GameObject scaleObj) : base(owner)
    {
        m_scaledObject = scaleObj;

        if (!m_scaledObject)
            Debug.Log("<color=red> ERROR: </color>Missing GameObject for LargeState");

        m_maxSize = m_scaledObject.transform.localScale.x * 2;
    }

    public override void Manage()  //Current behaviour is just the animation from night to day
    {
        if (m_scaledObject)
            Grow();

        AnimateBehaviour();
        s_timer += Time.deltaTime; //Only needed for the animation 
    }

    private void AnimateBehaviour() //Temp behaviour
    {
        Vector3 m_updatedPos = m_scaledObject.transform.position;

        m_updatedPos.x += (Mathf.Sin(s_timer * m_randSpeed) / 40);
        m_updatedPos.y += (Mathf.Sin(s_timer * m_randSpeed) / 80);
        m_updatedPos.z += (Mathf.Cos(s_timer * m_randSpeed) / 80);

        m_scaledObject.transform.position = m_updatedPos;
    }

    private void Grow()
    {
        if (m_size < m_maxSize)
        {
            m_size += Time.deltaTime;
            m_scaledObject.transform.localScale = new Vector3(m_size, m_size, m_size);
        }
    }
}
