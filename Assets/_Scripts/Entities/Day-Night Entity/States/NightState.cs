#define DEBUG

using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightState : State
{
    private Light m_light;
    private Quaternion m_finalRotation = new Quaternion(0.00845f, -0.2545f, 0.00226f, 0.9754f); // Radians

    public NightState(Automaton owner, Light light) : base(owner) 
    {
        m_light = light;
    }

    public override void Manage()
    {
        if (m_light)
            m_light.transform.rotation = Quaternion.Slerp(m_light.transform.localRotation, m_finalRotation, 0.01f);
    }
}