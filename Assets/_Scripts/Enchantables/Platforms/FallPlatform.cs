using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallPlatform : Enchantable
{
    [Header("Time Attributes")]
    [SerializeField]
    private int m_normalTimeTillFall = 1;

    [Header("Shake Attributes")]
    [SerializeField]
    private float m_shakeScale;

    [SerializeField]
    private float m_shakeSpeed;

    [Header("Required Objects")]
    [SerializeField]
    private GameObject m_platform;

    private Rigidbody m_rigBod;
    
    private float m_timeTillFall = 0;
    private float m_counter = 0;

    private const float c_scaleTime = 2.0f;
    private const float c_respawnTime = 5.0f;

    private Vector3 m_position = Vector3.zero;
    private Vector3 m_platBasePosition = Vector3.zero;
    private Vector3 m_smallScale = new Vector3(0.2f, 0.2f, 0.2f);
    private Vector3 m_largeScale = new Vector3(2.0f, 2.0f, 2.0f);

    private bool m_fallTriggerStart = false;

    private bool m_isFrozen = false;

    // Start is called before the first frame update
    void Start()
    {
        m_rigBod = GetComponent<Rigidbody>();
        m_rigBod.isKinematic = true;
        m_platform.GetComponent<Rigidbody>().isKinematic = true;
        m_position = transform.position;
        m_platBasePosition = m_platform.transform.position;
        m_timeTillFall = m_normalTimeTillFall;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !m_fallTriggerStart)
            StartCoroutine(StartFall());
    }

    #region Coroutines
    /// <summary>
    /// Takes any input for the new scale, allowing for reuse.
    /// </summary>
    /// <param name="scale">A vector3 with appropriate scale values. Keep in mind that currently time to scale is fixed</param>
    /// <returns></returns>
    IEnumerator ScaleObject(Vector3 scale)
    {
        //Reset counter as it might have been used elsewhere. Set original scale as it is needed for the lerp
        Vector3 originalScale = transform.localScale;
        m_counter = 0;

        while (m_counter < c_scaleTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, scale, m_counter);
            m_counter += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /// <summary>
    /// Once player has colided with platform, it will eventually fall. 
    /// </summary>
    /// <returns></returns>
    IEnumerator StartFall()
    {
        //Reset the counter (as it might have used on scale)
        m_fallTriggerStart = true;
        m_counter = 0;

        if(!m_isFrozen)
        {
            while (m_rigBod.isKinematic)
            {
                m_counter += Time.deltaTime;

                //If timer is up, fall. Otherwise, shake using call in else statement
                if (m_counter >= m_timeTillFall)
                {
                    m_rigBod.isKinematic = false;
                    m_platform.GetComponent<Rigidbody>().isKinematic = false;
                }

                else //Shaking  
                {
                    //transform.RotateAround(transform.position, transform.forward, m_rotation.z + (Mathf.Sin(m_counter * 0.1f)));
                    //transform.position = m_position + (transform.right * (Mathf.Sin(m_counter * m_shaleSpeed)) * m_shakeScale);
                    m_platform.transform.position = m_platBasePosition + (transform.right * (Mathf.Sin(m_counter * m_shakeSpeed)) * m_shakeScale);
                }
                    

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        
        yield return StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        while(true)
        {
            m_counter += Time.deltaTime;

            if (m_counter >= c_respawnTime)
            {
                m_counter = 0;
                m_rigBod.isKinematic = true;
                m_platform.GetComponent<Rigidbody>().isKinematic = true;
                m_platform.transform.position = m_position;
                transform.position = m_platBasePosition;
                m_fallTriggerStart = false;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
        
    }
    #endregion

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
        m_timeTillFall = m_normalTimeTillFall;
        StartCoroutine(ScaleObject(Vector3.one));
    }
    #endregion
}
