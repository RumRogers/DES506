using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlySleepState : State
{
    private GameObject m_fireFlyObject;
    private const float m_shrinkSpeed = 2;
    private float m_size = 0.1f;
    private Renderer m_fireFlyRenderer;

    public FireFlySleepState(Automaton owner, GameObject fireFlyObj) : base(owner)
    {
        m_fireFlyObject = fireFlyObj;
        m_fireFlyObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        if (!m_fireFlyObject)
            Debug.Log("<color=red> Error: </color>Missing gameobject in FireFlySleepState");
    }

    public override void Manage()
    {
        Shrink();
    }

    private void Shrink()
    {
        if(m_fireFlyObject.transform.localScale.x > 0)
        {
            m_size -= Time.deltaTime * m_shrinkSpeed;
            m_fireFlyObject.transform.localScale = new Vector3(m_size, m_size, m_size);
        }
    }
}
