using GameCore.Spells;
using System.Collections;
using UnityEngine;

public class Platform : Enchantable
{
    enum PlatformType
    {
        HORIZONTAL,
        VERTICAL,
        ROTATION
    }

    #region Parameters
    [Header("Platform Parameters")]
    [SerializeField]
    private float m_motionWidth = 5.0f;

    [SerializeField]
    private float m_platformSpeed = 0.1f;

    [SerializeField]
    private PlatformType m_platType;

    [Header("Scale Factors")]
    [SerializeField]
    private float m_smallScaleFactor;

    [SerializeField]
    private float m_largeScaleFactor;

    //Contained Variables
    private float m_counter = 0;

    private const float c_scaleTime = 2.0f;
    private const float c_distanceFactor = 1.0f;

    private bool m_isMoving = true;

    private Vector3 m_defaultPosition = Vector3.zero;
    private Vector3 m_destination = Vector3.zero;
    private Vector3 m_smallScale = Vector3.zero;
    private Vector3 m_largeScale = Vector3.zero;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Retain original position
        m_defaultPosition = transform.position;

        m_smallScale = transform.localScale * m_smallScaleFactor;
        m_largeScale = transform.localScale * m_largeScaleFactor;

        StartCoroutine(Translate());
    }

    #region Enumerators
    IEnumerator Translate()
    {
        m_isMoving = true;
        while (true)
        {
            switch (m_platType)
            {
                case PlatformType.HORIZONTAL:
                    if (Vector3.Distance(transform.position, m_defaultPosition) <= c_distanceFactor)
                        m_destination = m_defaultPosition + transform.right * m_motionWidth;

                    else if (Vector3.Distance(transform.position, m_defaultPosition + transform.right * m_motionWidth) <= c_distanceFactor)
                        m_destination = m_defaultPosition;

                    transform.position = Vector3.Lerp(transform.position, m_destination, m_platformSpeed/10);

                    break;

                case PlatformType.VERTICAL:
                    if(Vector3.Distance(transform.position, m_defaultPosition) <= c_distanceFactor)                    
                        m_destination = m_defaultPosition + transform.up * m_motionWidth;
                    
                    else if(Vector3.Distance(transform.position, m_defaultPosition + transform.up * m_motionWidth) <= c_distanceFactor)                    
                        m_destination = m_defaultPosition;                    

                    transform.position = Vector3.Lerp(transform.position, m_destination, m_platformSpeed/10);
                    
                    break;

                case PlatformType.ROTATION:
                    transform.RotateAround(transform.position, transform.right, m_platformSpeed);
                    break;
            }
           
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator ScaleObject(Vector3 scale)
    {
        Vector3 originalScale = transform.localScale;
        m_counter = 0;

        while(m_counter < c_scaleTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, scale, m_counter);
            m_counter += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    #endregion

    #region Spell Functions
    protected override void SpellTemperatureCold(Spell spell) 
    {
        if(m_isMoving)
        {
            StopCoroutine(Translate());
            m_isMoving = false;
        }
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
        if (!m_isMoving)
        {
            StartCoroutine(Translate());
        }
    }

    protected override void SpellReset(Spell spell) 
    {
        if (!m_isMoving)
        {
            StartCoroutine(Translate());
        }
            
        StartCoroutine(ScaleObject(Vector3.one));
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(m_defaultPosition + new Vector3(0, 0, m_motionWidth), 0.6f);
        Gizmos.DrawSphere(transform.position + new Vector3(0, 0, m_motionWidth), 0.6f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("TEST");
    }
}
