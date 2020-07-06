using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint),typeof(Rigidbody))]
public class SeeSaw : Enchantable
{
    [SerializeField]
    private bool m_startFrozen;

    private HingeJoint m_joint;
    private Rigidbody m_rigBod;
    private Vector3 m_smallScale = new Vector3(0.2f, 0.2f, 0.2f);
    private Vector3 m_largeScale = new Vector3(2.0f, 2.0f, 2.0f);
    private float m_counter = 0;

    private const float c_scaleTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        //Bring in and check vital components
        m_joint = GetComponent<HingeJoint>();
        m_rigBod = GetComponent<Rigidbody>();

        //Defaults for the hinge and rigid body
        m_rigBod.isKinematic = m_startFrozen;
        m_joint.axis = new Vector3(0.0f, 0.0f, 1.0f);
        m_joint.anchor = new Vector3(0.0f, 0.5f, 0.0f);
    }
    
    IEnumerator ScaleObject(Vector3 scale)
    {
        Vector3 originalScale = transform.localScale;
        m_counter = 0;

        while (m_counter < c_scaleTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, scale, m_counter);
            m_counter += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    #region Spell Functions
    protected override void SpellTemperatureCold(Spell spell)
    {
        m_rigBod.isKinematic = true;
    }

    protected override void SpellSizeBig(Spell spell)
    {
        StartCoroutine(ScaleObject(m_largeScale));
    }

    protected override void SpellSizeSmall(Spell spell)
    {
        StartCoroutine(ScaleObject(m_smallScale));
    }

    protected override void SpellTemperatureHot(Spell spell)
    {
        m_rigBod.isKinematic = false;
    }

    protected override void SpellReset(Spell spell)
    {
        StartCoroutine(ScaleObject(Vector3.zero));
    }
    #endregion
}