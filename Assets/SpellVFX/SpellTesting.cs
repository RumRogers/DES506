using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTesting : MonoBehaviour
{
    [SerializeField]
    private RFX4_EffectEvent eventHandler;

    [Header("Debugging Options")]
    [SerializeField]
    private bool m_autoFire = false;

    [SerializeField]
    private float m_timeTillAutoFire;

    private float m_counter = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CastSpell();


        if (m_autoFire)
        {
            m_counter += Time.deltaTime;

            if(m_counter > m_timeTillAutoFire)
            {
                m_counter = 0;
                CastSpell();
            }
        }
    }

    void CastSpell()
    {
     
       // eventHandler.ActivateCharacterEffect();
    }
}
