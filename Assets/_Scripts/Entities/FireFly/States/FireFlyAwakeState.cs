using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyAwakeState : State
{
    //Reminents of a colision avoidance check 
    private const float m_distanceCheck = 100.0f;

    //Defaults with override constructors for changes
    private float m_maxSize = 0.1f;
    private float m_growthSpeed = 2;

    //The rest of the class memeber variables
    private GameObject m_fireFlyObject;
    static private float s_timer; //Static to ensure it starts where it left off
    private float m_size = 0.0f;
    private bool m_inMotion = false;
    private float m_randSpeed = Random.Range(0.6f, 4.0f);

    //Constructors
    #region
    public FireFlyAwakeState(Automaton owner, GameObject fireFlyObj) : base(owner)
    {
        m_fireFlyObject = fireFlyObj;
       
        //The asleep version can sometimes cast a very faint shadow, so for now, this is the solution
        m_fireFlyObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        if (!m_fireFlyObject)
            Debug.Log("<color=red> Error: </color>Missing gameobject in FireFlyAwakeState");
    }

    public FireFlyAwakeState(Automaton owner, GameObject fireFlyObj, float speed) : base(owner)
    {
        m_fireFlyObject = fireFlyObj;

        m_randSpeed = speed;

        m_fireFlyObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        if (!m_fireFlyObject)
            Debug.Log("<color=red> Error: </color>Missing gameobject in FireFlyAwakeState");
    }
    #endregion 

    public override void Manage()
    {
        s_timer += Time.deltaTime; //Only needed for the animation 
        AnimateBehaviour();
        Grow();
    }

    private void AnimateBehaviour() //Temp behaviour
    {
        Vector3 m_updatedPos = m_fireFlyObject.transform.position;

            m_updatedPos.x += (Mathf.Sin(s_timer * m_randSpeed) / 40);
            m_updatedPos.y += (Mathf.Sin(s_timer * m_randSpeed) / 80);
            m_updatedPos.z += (Mathf.Cos(s_timer * m_randSpeed) / 80);

        m_fireFlyObject.transform.position = m_updatedPos;
    }

    private void Grow()
    {
        if (m_size < m_maxSize)
        {
            m_size += Time.deltaTime * m_growthSpeed;
            m_fireFlyObject.transform.localScale = new Vector3(m_size, m_size, m_size);
        }
    }

    //Currently not needed, and will likely eventually be removed
    void CollisionChecker()
    {

        Ray m_rayTest = new Ray(m_fireFlyObject.transform.position, m_fireFlyObject.transform.forward);
        RaycastHit m_raycastHit;
        if (!m_inMotion)
        {
            if (!Physics.Raycast(m_rayTest, m_distanceCheck))
            {
                Debug.DrawRay(m_rayTest.origin, m_rayTest.direction * 100, Color.yellow);
                Debug.Log("Did Not Hit");

                //m_inMotion = true;
            }
            else if (Physics.Raycast(m_rayTest, out m_raycastHit, m_distanceCheck))
            {
                Debug.DrawRay(m_rayTest.origin, m_rayTest.direction * m_raycastHit.distance, Color.red);
                Debug.Log("Did Hit");
            }
        }

        if (m_inMotion) { }
    }
}