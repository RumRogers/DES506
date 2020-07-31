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

    //private GameObject CharacterEffect;
    //private Transform CharacterAttachPoint;
    //private float CharacterEffect_DestroyTime = 10;


    [Space]

    [Header("Eraser Spell Effect")]
    [SerializeField]
    private GameObject m_eraserEffect;

    [SerializeField]
    private Transform m_eraserCastPoint;

    [SerializeField]
    private float m_eraserDestroyTime;

    GameObject m_instance;


    //private GameObject MainEffect;

    //private Transform AttachPoint;

    //private Transform OverrideAttachPointToTarget;

    //private float Effect_DestroyTime = 10;


    private float m_distance;

    public void CastSpellEffect(Vector3 targetPos)
    {
        if (!m_spellEffect || m_instance)
            return;

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

        m_instance = Instantiate(m_eraserEffect, m_eraserCastPoint.position, m_eraserCastPoint.rotation);

        m_instance.transform.position = m_eraserCastPoint.position + m_eraserCastPoint.forward * m_forwardOffset;

        m_instance.transform.RotateAround(m_instance.transform.right, -m_distance * 0.0174533f);

        if (m_spellDestroyTime > 0.01f)
            Destroy(m_instance, m_spellDestroyTime);
    }

    //private void ActivateEffect()
    //{
    //    if (MainEffect == null) 
    //        return;
        
    //    GameObject instance;
    //    if (OverrideAttachPointToTarget == null)
    //        instance = Instantiate(MainEffect, AttachPoint.transform.position, AttachPoint.transform.rotation);

    //    else
    //        instance = Instantiate(MainEffect, AttachPoint.transform.position, Quaternion.LookRotation(-(AttachPoint.position - OverrideAttachPointToTarget.position)));

    //    instance.transform.position = AttachPoint.transform.position + AttachPoint.transform.forward * m_forwardOffset;


    //    instance.transform.RotateAround(instance.transform.right, -m_distance * 0.0174533f);

        
    //    if (Effect_DestroyTime > 0.01f) 
    //        Destroy(instance, Effect_DestroyTime);
    //}

    //private void ActivateCharacterEffect()
    //{
    //    if (CharacterEffect == null)
    //        return;
        
    //    var instance = Instantiate(CharacterEffect, CharacterAttachPoint.transform.position, CharacterAttachPoint.transform.rotation, CharacterAttachPoint.transform);
      
        
    //    if (CharacterEffect_DestroyTime > 0.01f)
    //        Destroy(instance, CharacterEffect_DestroyTime);
    //}
}
