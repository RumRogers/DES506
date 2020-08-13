using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(HingeJoint),typeof(Rigidbody))]
public class SeeSaw : MonoBehaviour
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

    [SerializeField]
    private float m_angleLimiter = 11.81f;

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

    void Update()
    {
        if (WrapAngle(transform.eulerAngles.x) <= (m_angleLimiter))
        {
            if (m_leftSensor.IsTriggered() && !m_counterW.IsCorrectSize())
            {
                transform.Rotate(Vector3.right, (Time.deltaTime) * m_speed, Space.Self);

            }
        }

        if (WrapAngle(transform.eulerAngles.x) >= -(m_angleLimiter))
        {
            if (m_rightSensor.IsTriggered() || m_counterW.IsCorrectSize())
            {
                transform.Rotate(Vector3.right, (Time.deltaTime) * -m_speed, Space.Self);
            }
        }
    }

    private float WrapAngle(float angle)
    {
        angle %= 360;

        if (angle > 180)
            return angle - 360;

        return angle;
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

}