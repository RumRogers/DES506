using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Work in progress

public class ColorState : State
{
    private Renderer m_colorChangeObject;
    private Color m_entityColor;
    private float m_timer = 0;

    public ColorState(Automaton owner, Renderer entityObject, Color desiredColor) : base(owner)
    {
        m_colorChangeObject = entityObject;
        m_entityColor = desiredColor;

        if(!m_colorChangeObject)
            Debug.Log("<color=red> Error: </color>Missing renderer");
    }

    public override void Manage()
    {
        if(m_timer <= 1)
        {
            m_colorChangeObject.material.color = Color.white;
            m_timer += Time.deltaTime;
        }
    }
}

