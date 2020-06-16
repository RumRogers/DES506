using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightState : State
{
    private Light m_light;
    private Quaternion m_finalRotation = new Quaternion(0.00845f, -0.2545f, 0.00226f, 0.9754f); // Radians
    private float m_transitionSpeed = 0.1f; //Default speed

    #region
    /// <summary>
    /// Constructor for the night state, requires the automaton owner and the light source that influences the skybox
    /// </summary>
    /// <param name="owner">The Day/Night entity</param>
    /// <param name="light">The light source of the Day/Night entity</param>
    /// <param name="speed">This sets the speed at which night turns to day</param>
    public NightState(Automaton owner, Light light, float speed) : base(owner) 
    {
        m_light = light;

        m_transitionSpeed = speed;

        if (!m_light)
            Debug.Log("<color=red> ERROR: </color>Missing Light for NightState");
    }

    /// <summary>
    /// Constructor for the night state, requires the automaton owner and the light source that influences the skybox
    /// </summary>
    /// <param name="owner">The Day/Night entity</param>
    /// <param name="light">The light source of the Day/Night entity</param>
    public NightState(Automaton owner, Light light) : base(owner)
    {
        m_light = light;

        if (!m_light)
            Debug.Log("<color=red> ERROR: </color>Missing Light for NightState");
    }
    #endregion

    public override void Manage()
    {
        if (m_light)
            m_light.transform.rotation = Quaternion.Slerp(m_light.transform.localRotation, m_finalRotation, m_transitionSpeed);
    }
}