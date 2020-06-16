using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenState : State
{
    private GameObject m_pivot;
    private Vector3 m_point;
    public OpenState(Automaton owner, GameObject piv) : base(owner)
    {
        m_pivot = piv;
        m_point = m_pivot.transform.position - new Vector3(0.7f, 0, 0);
    }

    public override void Manage()  //Current behaviour is just the animation from night to day
    {
        if(m_pivot.transform.rotation.eulerAngles.y < 100)
            m_pivot.transform.RotateAround(m_point, Vector3.up, 0.8f);
        //m_pivot.transform.Rotate(Vector3.up, 1);
    }
}
