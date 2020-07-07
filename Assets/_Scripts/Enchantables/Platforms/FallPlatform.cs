using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallPlatform : Enchantable
{
    [SerializeField]
    private int m_normalTimeTillFall = 1;

    [SerializeField]
    private int m_timeTillFallFrozen = 2;

    private Rigidbody m_rigBod;
    
    private float m_timeTillFall = 1;
    private float m_counter = 0;

    private const float c_scaleTime = 2.0f;

    private Vector3 m_position = Vector3.zero;
    private Vector3 m_smallScale = new Vector3(0.2f, 0.2f, 0.2f);
    private Vector3 m_largeScale = new Vector3(2.0f, 2.0f, 2.0f);

    private bool m_fallTriggerStart = false;

    // Start is called before the first frame update
    void Start()
    {
        m_rigBod = GetComponent<Rigidbody>();
        m_rigBod.isKinematic = true;
        m_position = transform.position;
        m_timeTillFall = m_normalTimeTillFall;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !m_fallTriggerStart)
            StartCoroutine(StartFall());
    }

    #region Coroutines
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

    IEnumerator StartFall()
    {
        m_fallTriggerStart = true;
        m_counter = 0;

        while(m_rigBod.isKinematic)
        {
            m_counter += Time.deltaTime;

            if (m_counter >= m_timeTillFall)
            {
                m_rigBod.isKinematic = false;
            }

            else
                transform.position = m_position + (transform.right * (Mathf.Sin(m_counter * 30)) * 0.01f);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;
    }
    #endregion

    #region Spell Functions
    protected override void SpellTemperatureCold(Spell spell)
    {
        m_timeTillFall = m_timeTillFallFrozen;
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
        m_timeTillFall = m_normalTimeTillFall;
    }

    protected override void SpellReset(Spell spell)
    {
        m_timeTillFall = m_normalTimeTillFall;
        StartCoroutine(ScaleObject(Vector3.one));
    }
    #endregion
}
