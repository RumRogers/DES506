using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cog : Enchantable
{
    [Header("Scale Factors")]
    [SerializeField]
    private float m_smallScaleFactor;

    [SerializeField]
    private float m_largeScaleFactor;


    [SerializeField]
    private bool m_isRotating;


    private float m_counter = 0;

    private const float c_scaleTime = 2.0f;
    private Vector3 m_smallScale = Vector3.zero;
    private Vector3 m_largeScale = Vector3.zero;

    void Start()
    {
        m_smallScale = transform.localScale * m_smallScaleFactor;
        m_largeScale = transform.localScale * m_largeScaleFactor;
    }

    protected override void Update()
    {
        base.Update();

        //transform.RotateAround(transform.position, transform.up, Time.deltaTime * 10);
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

    }

    protected override void SpellReset(Spell spell)
    {

        StartCoroutine(ScaleObject(Vector3.one));
    }
    #endregion

}
