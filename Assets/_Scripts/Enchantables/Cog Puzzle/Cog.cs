using GameCore.Spells;
using System.Collections;
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

    //Rotation controls
    private bool m_isRotating; 
    private bool m_iStopIt = false;

    //Cog Behaviour
    private bool m_isTicking;
    private bool m_isClockwise;
    private float m_cogSpeed;

    //Spell Components
    private bool m_isFrozen;
    private bool m_isLarge = false;
    private bool m_isSmall = false;

    private float m_counter = 0;

    //Scale components
    private const float c_scaleTime = 2.0f;
    private Vector3 m_smallScale = Vector3.zero;
    private Vector3 m_largeScale = Vector3.zero;
    private Quaternion m_globalRotation;

    private IEnumerator m_rotationReference;

    //Public accessors 
    //Should be removed, unless frozen is explicitly needed
    public bool IsFrozen { get { return m_isFrozen; } }
    public bool IsLarge { get { return m_size == SizeState.LARGE; } }
    public bool IsSmall { get { return m_size == SizeState.SMALL; } }
    public bool IsRotating { get { return m_isRotating; } }

    public Quaternion GlobalRotation { get { return m_globalRotation; } set { m_globalRotation = value; } }

    private IEnumerator m_scaleLargeReference;
    private IEnumerator m_scaleSmallReference;
    private IEnumerator m_scaleDefaultReference;
    /// <summary>
    /// The initialiser function for the cog, to allow for shared information
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="tick"></param>
    /// <param name="smallScaleFactor"></param>
    /// <param name="largeScaleFactor"></param>
    /// <param name="speed"></param>
    public void SetUp(bool direction, bool tick, float smallScaleFactor, float largeScaleFactor, float speed)
    {
        m_isClockwise = direction;
        m_isTicking = tick;
        m_smallScale = transform.localScale * smallScaleFactor;
        m_largeScale = transform.localScale * largeScaleFactor;
        m_cogSpeed = speed;

        m_scaleLargeReference = ScaleObject(m_largeScale);
        m_scaleSmallReference = ScaleObject(m_smallScale);
        m_scaleDefaultReference = ScaleObject(Vector3.one);

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
            if (!m_leftNeighbour.IsRotating || m_leftNeighbour.IsSmall || !IsCorrect())
            {
                StopCoroutine(m_rotationReference);
                m_isRotating = false;
            }
            //If the cog is stationary, check if a change has now allowed it to move
            else if(m_leftNeighbour.IsRotating && m_leftNeighbour.IsCorrect() && IsCorrect() && !m_isRotating)
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
                //Tick version probably not needed
            }
            else
            {
                if (m_isClockwise)
                    transform.RotateAround(transform.position, transform.right, Time.deltaTime * m_cogSpeed);
                else
                    transform.RotateAround(transform.position, transform.right, Time.deltaTime * -m_cogSpeed);
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
        if(m_scaleDefaultReference != null)
            StopCoroutine(m_scaleDefaultReference);

        if(m_scaleSmallReference != null)
            StopCoroutine(m_scaleSmallReference);

        StartCoroutine(m_scaleLargeReference);

        m_size = SizeState.LARGE;
    }

    protected override void SpellSizeSmall(Spell spell)
    {
        if (m_scaleDefaultReference != null)
            StopCoroutine(m_scaleDefaultReference);

        if (m_scaleLargeReference != null)
            StopCoroutine(m_scaleLargeReference);

        StartCoroutine(m_scaleSmallReference);

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

        if (m_scaleSmallReference != null)
            StopCoroutine(m_scaleSmallReference);

        if (m_scaleLargeReference != null)
            StopCoroutine(m_scaleLargeReference);

        StartCoroutine(m_scaleDefaultReference);

        m_size = SizeState.DEFAULT;
    }
    #endregion

    public bool IsCorrect()
    {
        if (Vector3.Distance(transform.localScale, Vector3.one) < 0.2f && m_size == m_solutionSize)
            return true;

        return false;
        //return m_size == m_solutionSize;
    }

}
