using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyEntity : Automaton
{
    private bool m_isDay = false;

    void Start()
    {
        SetState(new FireFlySleepState(this, this.gameObject));
    }

    override protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_isDay = !m_isDay;

            switch (m_isDay)
            {
                case false:
                    SetState(new FireFlySleepState(this, this.gameObject));
                    break;

                case true:
                    SetState(new FireFlyAwakeState(this, this.gameObject));
                    break;
            }
        }

        if (m_state != null)
            m_state.Manage();
    }
}
