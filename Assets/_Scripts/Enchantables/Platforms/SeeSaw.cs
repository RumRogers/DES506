using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(HingeJoint),typeof(Rigidbody))]
public class SeeSaw : Enchantable
{
    [SerializeField]
    private bool m_startFrozen;

    [Header("Seesaw Sensors")]
    [SerializeField]
    private Sensor m_leftSensor;

    [SerializeField]
    private Sensor m_rightSensor;

    [SerializeField]
    private float m_speed;

    [SerializeField]
    private CounterWeight m_counterW;
    //Scale
    private Vector3 m_smallScale;// = new Vector3(0.2f, 0.2f, 0.2f);
    private Vector3 m_largeScale;// = new Vector3(2.0f, 2.0f, 2.0f);
    
    private float m_counter = 0;
    
    private Renderer m_renderer;

    private const float c_scaleTime = 2.0f;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();
                
        m_smallScale = transform.localScale * 0.5f;
        m_largeScale = transform.localScale * 2.0f;
    }

    protected override void Update()
    {
        base.Update();


        if(transform.rotation.x <= (Mathf.Deg2Rad * 11.81f)/2)
        {
            if (m_leftSensor.IsTriggered() && !m_counterW.IsCorrectSize())
            {
                transform.Rotate(transform.right, (Mathf.Deg2Rad * Time.deltaTime) * m_speed);
            }
        }
        
        if(transform.rotation.x >= -(Mathf.Deg2Rad * 11.81f) / 2)
        {
            if (m_rightSensor.IsTriggered() || m_counterW.IsCorrectSize())
            {
                transform.Rotate(transform.right, (Mathf.Deg2Rad * Time.deltaTime) * -m_speed);
            }
        }

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

    public Vector3 GetDimensions()
    {
        return m_renderer.bounds.size;
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
        StartCoroutine(ScaleObject(Vector3.zero));
    }

    #endregion
}