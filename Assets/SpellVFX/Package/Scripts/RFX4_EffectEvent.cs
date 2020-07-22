using UnityEngine;
using System.Collections;

public class RFX4_EffectEvent : MonoBehaviour
{
    [Header("Quil Spell Effect")]
    [SerializeField]
    private GameObject CharacterEffect;

    [SerializeField]
    private Transform CharacterAttachPoint;

    [SerializeField]
    private float CharacterEffect_DestroyTime = 10;
    [Space]

    //public GameObject CharacterEffect2;
    //public Transform CharacterAttachPoint2;
    //public float CharacterEffect2_DestroyTime = 10;
    //[Space]

    [Header("Main Spell Effect")]
    [SerializeField]
    private GameObject MainEffect;

    [SerializeField]
    private Transform AttachPoint;

    [SerializeField]
    private Transform OverrideAttachPointToTarget;

    [SerializeField]
    private float Effect_DestroyTime = 10;

    [SerializeField]
    private float m_forwardOffset;
    //public GameObject AdditionalEffect;
    //public Transform AdditionalEffectAttachPoint;
    //public float AdditionalEffect_DestroyTime = 10;

    [HideInInspector] public bool IsMobile;

    public void Update()
    {
        AttachPoint.LookAt(OverrideAttachPointToTarget);
    }
    public void ActivateEffect()
    {

        if (MainEffect == null) 
            return;
        
        GameObject instance;
        if (OverrideAttachPointToTarget == null)
            instance = Instantiate(MainEffect, AttachPoint.transform.position, AttachPoint.transform.rotation);

        else
            instance = Instantiate(MainEffect, AttachPoint.transform.position, Quaternion.LookRotation(-(AttachPoint.position - OverrideAttachPointToTarget.position)));

        instance.transform.position = AttachPoint.transform.position + AttachPoint.transform.forward * m_forwardOffset;

        //if (OverrideAttachPointToTarget == null)
        //    instance = Instantiate(MainEffect, pos, AttachPoint.rotation);

        //else
        //    instance = Instantiate(MainEffect, pos, Quaternion.LookRotation(-(pos - OverrideAttachPointToTarget.position)));


        UpdateEffectForMobileIsNeed(instance);
        
        if (Effect_DestroyTime > 0.01f) 
            Destroy(instance, Effect_DestroyTime);
    }

    //public void ActivateAdditionalEffect()
    //{
    //    if (AdditionalEffect == null) return;
    //    if (AdditionalEffectAttachPoint != null)
    //    {
    //        var instance = Instantiate(AdditionalEffect, AdditionalEffectAttachPoint.transform.position, AdditionalEffectAttachPoint.transform.rotation);
    //        UpdateEffectForMobileIsNeed(instance);
    //        if (AdditionalEffect_DestroyTime > 0.01f) Destroy(instance, AdditionalEffect_DestroyTime);
    //    }
    //    else AdditionalEffect.SetActive(true);
    //}

    public void ActivateCharacterEffect()
    {
        if (CharacterEffect == null)
            return;
        
        //var instance = Instantiate(CharacterEffect, pos, CharacterAttachPoint.transform.rotation, CharacterAttachPoint.transform);
        var instance = Instantiate(CharacterEffect, CharacterAttachPoint.transform.position, CharacterAttachPoint.transform.rotation, CharacterAttachPoint.transform);
        
        UpdateEffectForMobileIsNeed(instance);
        
        if (CharacterEffect_DestroyTime > 0.01f)
            Destroy(instance, CharacterEffect_DestroyTime);
    }

    //public void ActivateCharacterEffect2()
    //{
    //    if (CharacterEffect2 == null) return;
    //    var instance = Instantiate(CharacterEffect2, CharacterAttachPoint2.transform.position, CharacterAttachPoint2.transform.rotation, CharacterAttachPoint2);
    //    UpdateEffectForMobileIsNeed(instance);
    //    if (CharacterEffect2_DestroyTime > 0.01f) Destroy(instance, CharacterEffect2_DestroyTime);
    //}

    void UpdateEffectForMobileIsNeed(GameObject instance)
    {
        //if (IsMobile)
        {
            var effectSettings = instance.GetComponent<RFX4_EffectSettings>();
            if (effectSettings != null)
            {
                //effectSettings.EffectQuality = IsMobile ? RFX4_EffectSettings.Quality.Mobile : RFX4_EffectSettings.Quality.PC;
                //effectSettings.ForceInitialize();
            }
        }
    }

}
