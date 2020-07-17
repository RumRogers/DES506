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
    private const float c_distanceFactor = 0.1f;

    private bool m_isMoving = true;

    private Vector3 m_defaultPosition = Vector3.zero;
    private Vector3 m_destination = Vector3.zero;
    private Vector3 m_smallScale = Vector3.zero;
    private Vector3 m_largeScale = Vector3.zero;

    private int m_direction = 1;
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

    private void FixedUpdate()
    {
        if (m_isMoving)
        {
            switch (m_platType)
            {
                case PlatformType.HORIZONTAL:

                    TranslatePosition(transform.right);
                    break;

                case PlatformType.VERTICAL:

                    TranslatePosition(transform.up);

                    break;

                case PlatformType.ROTATION:
                    transform.RotateAround(transform.position, transform.right, m_platformSpeed);
                    break;
            }
        }
    }

    #region Enumerators
    IEnumerator Translate()
    {
        m_isMoving = true;
        while (true)
        {
           
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

    private void TranslatePosition(Vector3 direction)
    {
        m_destination = m_defaultPosition + direction * m_motionWidth;

        if (Vector3.Distance(transform.position, m_defaultPosition) <= c_distanceFactor)
        {
            m_direction = 1;
        }

        else if (Vector3.Distance(transform.position, m_destination) <= c_distanceFactor)
        {
            m_direction = -1;
        }

        if (Physics.Raycast(transform.position, direction * m_direction, c_distanceFactor + 1 * (transform.localScale.z / 2)))
        {
            m_direction = -m_direction;
        }

        transform.position += direction * m_direction * (Time.deltaTime * m_platformSpeed);
    }
}
