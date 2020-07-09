using GameCore.Spells;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CounterWeight : Enchantable
{
    [SerializeField]
    private float m_largeScaleFactor;
    [SerializeField]
    private float m_largeMass;

    [SerializeField]
    private float m_smallScaleFactor;
    [SerializeField]
    private float m_smallMass;

    private Vector3 m_smallScale;
    private Vector3 m_largeScale;

    private float m_counter = 0;
    private float m_defaultMass;
    
    private const float c_scaleTime = 2.0f;
    
    private Rigidbody m_rigidBody;

    private void Start()
    {
        m_smallScale = transform.localScale * m_smallScaleFactor;
        m_largeScale = transform.localScale * m_largeScaleFactor;

        m_rigidBody = GetComponent<Rigidbody>();

        m_defaultMass = m_rigidBody.mass;
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

    protected override void SpellSizeBig(Spell spell)
    {
        StartCoroutine(ScaleObject(m_largeScale));

        m_rigidBody.mass = m_largeMass;
    }

    protected override void SpellSizeSmall(Spell spell)
    {
        StartCoroutine(ScaleObject(m_smallScale));

        m_rigidBody.mass = m_smallMass;
    }

    protected override void SpellReset(Spell spell)
    {
        StartCoroutine(ScaleObject(Vector3.one));

        m_rigidBody.mass = m_defaultMass;
    }

    #endregion
}
