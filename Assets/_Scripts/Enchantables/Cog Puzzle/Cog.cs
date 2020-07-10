using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cog : Enchantable
{
    //Cog Behaviour
    private bool m_isTicking;
    private bool m_isClockwise;
    private float m_cogSpeed;

    //Spell Components
    private bool m_isFrozen;
    public bool IsFrozen { get { return m_isFrozen; } }
    
    private float m_counter = 0;

    //Scale components
    private const float c_scaleTime = 2.0f;
    private Vector3 m_smallScale = Vector3.zero;
    private Vector3 m_largeScale = Vector3.zero;


    /// <summary>
    /// The setup call for when the cog manager. Should probably change to a constructor, but seeing as it's an existing component, should be fine this way.
    /// </summary>
    /// <param name="direction">True is clockwise, false is anticlockwise</param>
    /// <param name="tick">Do you want the rotation to tick instead of be smooth</param>
    /// <param name="smallScaleFactor"></param>
    /// <param name="largeScaleFactor"></param>
    public void SetUp(bool direction, bool tick, float smallScaleFactor, float largeScaleFactor, float speed)
    {
        m_isClockwise = direction;
        m_isTicking = tick;
        m_smallScale = transform.localScale * smallScaleFactor;
        m_largeScale = transform.localScale * largeScaleFactor;
    }

    /// <summary>
    /// Until stopped will rotation cog in desired direction, and effect dependant on the m_isClockwise, and m_isTicking variables
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateRotation()
    {
        int m_direction = m_isClockwise ? 1 : -1;
        
        while (true)
        {
            if (m_isTicking)
            {

            }
            else
            {
                transform.RotateAround(transform.position, transform.right, Time.deltaTime * m_direction * m_cogSpeed);
            }

            m_counter += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    protected override void Update()
    {
        base.Update();
        
        if(m_isClockwise)
            transform.RotateAround(transform.position, transform.right, Time.deltaTime * 10);
        else
            transform.RotateAround(transform.position, transform.right, Time.deltaTime * -10);
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
        m_isFrozen = true;
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
        m_isFrozen = false;
    }

    protected override void SpellReset(Spell spell)
    {
        m_isFrozen = false;
        StartCoroutine(ScaleObject(Vector3.one));
    }
    #endregion

}
