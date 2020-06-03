using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : Automaton //Needs to be updatedto use mutable class 
{
    //Public Variables
    [Tooltip("This determines the speed of transition, with the larger the number, the faster it goes (Between 0 and 1)")]
    public float m_transitionSpeed = 0.01f;
    
    //Private and Protected
    private Light m_light;
    private bool m_isDay = true;

    void Start()
    {
        m_light = GetComponent<Light>();
        SetState(new DayState(this, m_light));
    }

    override protected void Update() //Needs to be updated to use rules
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_isDay = !m_isDay;

            switch (m_isDay)
            {
                case true:
                    SetState(new DayState(this, m_light));
                    break;

                case false:
                    SetState(new NightState(this, m_light));
                    break;
            }
        }

        base.Update();
    }
}