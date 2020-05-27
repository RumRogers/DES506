#define DEBUG

using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayState : State
{
    private Light m_light;
    private Quaternion m_finalRotation = new Quaternion(0.4082f, -0.2345f, 0.1093f, 0.8754f); // Radians
    
    public DayState(Automaton owner, Light light) : base(owner)
    {
        m_light = light;
    }

    public override void Manage() 
    {
        if(m_light)
            m_light.transform.rotation = Quaternion.Slerp(m_light.transform.localRotation, m_finalRotation, 0.01f);
    }
}