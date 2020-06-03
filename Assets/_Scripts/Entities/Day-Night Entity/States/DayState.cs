#define DEBUG

using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayState : State
{
    private Light m_light;
    private Quaternion m_finalRotation = new Quaternion(0.4082f, -0.2345f, 0.1093f, 0.8754f); // Radians
    
    /// <summary>
    /// Constructor for the day state, requires the automaton owner and the light source that influences the skybox
    /// </summary>
    /// <param name="owner">The Day/Night entity</param>
    /// <param name="light">The light source of the Day/Night entity</param>
    public DayState(Automaton owner, Light light) : base(owner)
    {
        m_light = light;

        if(!m_light)
            Debug.Log("<color=red> ERROR: </color>Missing Light for DayState");
    }

    public override void Manage()  //Current behaviour is just the animation from night to day
    {
        if(m_light)
            m_light.transform.rotation = Quaternion.Slerp(m_light.transform.localRotation, m_finalRotation, 0.01f);
    }
}