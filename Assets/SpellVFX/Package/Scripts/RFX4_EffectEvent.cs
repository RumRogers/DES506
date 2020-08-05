using UnityEngine;
using System.Collections;

public class RFX4_EffectEvent : MonoBehaviour
{
    [Header("Spell Effect")]
    [SerializeField]
    private GameObject m_spellEffect;

    [SerializeField]
    private Transform m_spellCastPoint;
    
    [SerializeField]
    private float m_spellDestroyTime;

    [SerializeField]
    private float m_forwardOffset;

    [Space]

    [Header("Eraser Spell Effect")]
    [SerializeField]
    private GameObject m_eraserEffect;

    [SerializeField]
    private Transform m_eraserCastPoint;

    [SerializeField]
    private float m_eraserDestroyTime;

    [SerializeField]
    private float m_rotationOffset;

    private const float c_radian = 0.0174533f;
    
    private GameObject m_instance;
    
    private float m_distance;

    public void CastSpellEffect(Vector3 targetPos)
    {
        if (!m_spellEffect)
            return;

        if (m_instance)
            m_instance = null;
        
        m_instance = Instantiate(m_spellEffect, m_spellCastPoint.position, m_spellCastPoint.rotation * Quaternion.FromToRotation(m_spellCastPoint.position, targetPos));
        m_instance.transform.LookAt(targetPos);
        m_instance.transform.position = m_spellCastPoint.position + m_spellCastPoint.forward * m_forwardOffset;

        if (m_spellDestroyTime > 0.01f)
            Destroy(m_instance, m_spellDestroyTime);
    }

    public void CastEraserEffect(Vector3 targetPos)
    {
        if (!m_eraserEffect || m_instance)
            return;

       // GameObject m_instance;

        m_distance = Vector3.Distance(m_eraserCastPoint.transform.position, targetPos);
        float angle = Vector3.Angle(m_eraserCastPoint.position, targetPos);
        m_instance = Instantiate(m_eraserEffect, m_eraserCastPoint.position, m_eraserCastPoint.rotation);

        m_instance.transform.position = m_eraserCastPoint.position + m_eraserCastPoint.forward * m_forwardOffset;

        //m_instance.transform.RotateAround(m_instance.transform.right, -m_distance * Mathf.Deg2Rad * m_rotationOffset);
        //m_instance.transform.Rotate(m_instance.transform.right, -m_distance * Mathf.Deg2Rad * m_rotationOffset);
        m_instance.transform.Rotate(m_instance.transform.right, angle * Mathf.Deg2Rad);

        if (m_spellDestroyTime > 0.01f)
            Destroy(m_instance, m_spellDestroyTime);
    }
}
