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
    private SizeState m_size = SizeState.DEFAULT;

    [SerializeField]
    private SizeState m_solutionSize = SizeState.DEFAULT;

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
    public bool IsLarge { get { return m_size == SizeState.LARGE; } }
    public bool IsSmall { get { return m_size == SizeState.SMALL; } }
    public bool IsRotating { get { return m_isRotating; } }

    public Quaternion GlobalRotation { get { return m_globalRotation; }  set { m_globalRotation = value; } }


    private float m_counter = 0;

    //Scale components
    private const float c_scaleTime = 2.0f;
    private Vector3 m_smallScale = Vector3.zero;
    private Vector3 m_largeScale = Vector3.zero;
    private Quaternion m_globalRotation;

    private IEnumerator m_rotationReference;

    public void SetUp(bool direction, bool tick, float smallScaleFactor, float largeScaleFactor, float speed)
    {
        m_isClockwise = direction;
        m_isTicking = tick;
        m_smallScale = transform.localScale * smallScaleFactor;
        m_largeScale = transform.localScale * largeScaleFactor;

        #region Starting Size
        switch (m_size)
        {
            case SizeState.DEFAULT:
                break;

            case SizeState.LARGE:
                transform.localScale = m_largeScale;
                m_size = SizeState.LARGE;
                break;

            case SizeState.SMALL:
                transform.localScale = m_smallScale;
                m_size = SizeState.SMALL;
                break;
        }
        #endregion

        m_rotationReference = Rotate();

        #region Check for starting rotation
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
        #endregion
    }

    protected override void Update()
    {
        base.Update();

        if (m_leftNeighbour != null)
        {
            //Check if current cog is no longer being acted upon
            if (!m_leftNeighbour.IsRotating || m_leftNeighbour.IsSmall)
            {
                StopCoroutine(m_rotationReference);
                m_isRotating = false;
            }
            //If the cog is stationary, check if a change has now allowed it to move
            else if(m_leftNeighbour.IsRotating && !m_leftNeighbour.IsSmall && !m_leftNeighbour.IsLarge && m_size == SizeState.DEFAULT && !m_isRotating)
            {
                StartCoroutine(m_rotationReference);
                m_isRotating = true;
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
        transform.rotation = m_globalRotation;

        while (true)
        {
            if(m_isTicking)
            {

            }
            else
            {
                if (m_isClockwise)
                    transform.RotateAround(transform.position, transform.right, Time.deltaTime * 10);
                else
                    transform.RotateAround(transform.position, transform.right, Time.deltaTime * -10);
            }

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
        //Prevent overlap by checking available neighbours sizes
        if(m_rightNeighbour == null)
        {
            if (m_leftNeighbour.IsSmall)
            {
                StartCoroutine(ScaleObject(m_largeScale));
                m_size = SizeState.LARGE;
            }
        }
        else if(m_leftNeighbour == null)
        {
            if (m_rightNeighbour.IsSmall)
            {
                StartCoroutine(ScaleObject(m_largeScale));
                m_size = SizeState.LARGE;
            }
        }
        else
        {
            if (m_leftNeighbour.IsSmall && m_rightNeighbour.IsSmall)
            {
                StartCoroutine(ScaleObject(m_largeScale));
                m_size = SizeState.LARGE;
            }
        }
    }

    protected override void SpellSizeSmall(Spell spell)
    {
        StartCoroutine(ScaleObject(m_smallScale));
        m_size = SizeState.SMALL;
    }

    protected override void SpellTemperatureHot(Spell spell)
    {
        //if(!m_isRotating)
        m_isFrozen = false;
    }

    protected override void SpellReset(Spell spell)
    {
        m_isFrozen = false;

        Debug.Log("Reset is called");

        if (m_rightNeighbour == null)
        {
            if (!m_leftNeighbour.IsLarge)
            {
                StartCoroutine(ScaleObject(Vector3.one));
                m_size = SizeState.DEFAULT;
            }
        }
        else if (m_leftNeighbour == null)
        {
            if (!m_rightNeighbour.IsLarge)
            {
                StartCoroutine(ScaleObject(Vector3.one));
                m_size = SizeState.DEFAULT;
            }
        }
        else
        {
            if (!m_leftNeighbour.IsLarge && !m_rightNeighbour.IsLarge)
            {
                StartCoroutine(ScaleObject(Vector3.one));
                m_size = SizeState.DEFAULT;
            }
        }
    }
    #endregion

    public bool IsCorrect()
    {
        return m_size == m_solutionSize;
    }

}
