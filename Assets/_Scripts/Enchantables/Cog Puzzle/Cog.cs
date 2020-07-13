using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cog : Enchantable
{
    enum SizeState { DEFAULT, LARGE, SMALL };

    [Header("Components")]
    [SerializeField]
    private Cog m_leftNeighbour;

    [SerializeField]
    private Cog m_rightNeighbour;

    [SerializeField]
    private SizeState m_startingSize = SizeState.DEFAULT;

    [SerializeField]
    private bool m_isRotating; //Only exposed temperarily 

    [SerializeField] 
    private bool m_iStopIt = false; //Only exposed temperarily 

    //Cog Behaviour
    private bool m_isTicking;
    private bool m_isClockwise;
    private float m_cogSpeed;


    //Spell Components
    private bool m_isFrozen;
    private bool m_isLarge = false;
    private bool m_isSmall = false;

    public bool IsFrozen { get { return m_isFrozen; } }
    public bool IsLarge { get { return m_isLarge; } }
    public bool IsSmall { get { return m_isSmall; } }
    public bool IsRotating { get { return m_isRotating; } }


    private float m_counter = 0;

    //Scale components
    private const float c_scaleTime = 2.0f;
    private Vector3 m_smallScale = Vector3.zero;
    private Vector3 m_largeScale = Vector3.zero;

    private IEnumerator m_rotationReference;

    public Cog(bool direction, bool tick, float smallScaleFactor, float largeScaleFactor, float speed)
    {
        m_isClockwise = direction;
        m_isTicking = tick;
        m_smallScale = transform.localScale * smallScaleFactor;
        m_largeScale = transform.localScale * largeScaleFactor;


    }

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

        //Set starting size
        switch (m_startingSize)
        {
            case SizeState.DEFAULT:

                break;

            case SizeState.LARGE:
                transform.localScale = m_largeScale;
                m_isLarge = true;
                break;

            case SizeState.SMALL:
                transform.localScale = m_smallScale;
                m_isSmall = true;
                break;
        }

        m_rotationReference = Rotate();

        if (m_leftNeighbour != null)
        {
            if (m_leftNeighbour.IsRotating == true && !m_leftNeighbour.IsSmall)
            {
                StartCoroutine(m_rotationReference);
                m_isRotating = true;
            }
        }
        else if (m_leftNeighbour == null)
        {
            StartCoroutine(m_rotationReference);
            m_isRotating = true;
        }
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

        if (m_leftNeighbour != null)
        {
            if (m_leftNeighbour.IsFrozen || !m_leftNeighbour.IsRotating || m_leftNeighbour.IsSmall)
            {
                StopCoroutine(m_rotationReference);
                m_isRotating = false;
            }
        }

        if(m_isRotating && m_iStopIt) //Temp solution for testing
        {
            StopCoroutine(m_rotationReference);
            m_isRotating = false;
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

    IEnumerator Rotate()
    {
        while (true)
        {
            if (m_isClockwise)
                transform.RotateAround(transform.position, transform.right, Time.deltaTime * 10);
            else
                transform.RotateAround(transform.position, transform.right, Time.deltaTime * -10);

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
        if(m_rightNeighbour == null)
        {
            if (m_rightNeighbour.IsSmall)
                StartCoroutine(ScaleObject(m_largeScale));
        }
        else if(m_leftNeighbour == null)
        {
            if (m_leftNeighbour.IsSmall)
                StartCoroutine(ScaleObject(m_largeScale));
        }
        else
        {
            if (m_leftNeighbour.IsSmall && m_rightNeighbour.IsSmall)
                StartCoroutine(ScaleObject(m_largeScale));
        }
    }

    protected override void SpellSizeSmall(Spell spell)
    {
        StartCoroutine(ScaleObject(m_smallScale));
        m_isSmall = true;
    }

    protected override void SpellTemperatureHot(Spell spell)
    {
        //if(!m_isRotating)
        m_isFrozen = false;
    }

    protected override void SpellReset(Spell spell)
    {
        m_isFrozen = false;

        if (m_rightNeighbour == null)
        {
            if (!m_rightNeighbour.IsLarge)
                StartCoroutine(ScaleObject(Vector3.one));
        }
        else if (m_leftNeighbour == null)
        {
            if (!m_leftNeighbour.IsLarge)
                StartCoroutine(ScaleObject(Vector3.one));
        }
        else
        {
            if (!m_leftNeighbour.IsLarge && !m_rightNeighbour.IsLarge)
                StartCoroutine(ScaleObject(Vector3.one));
        }
    }
    #endregion

}
