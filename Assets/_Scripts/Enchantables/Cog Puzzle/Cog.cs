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

    //Rotation controls
    private bool m_isRotating; 
    private bool m_iStopIt = false;

    //Cog Behaviour
    private bool m_isTicking;
    private bool m_isClockwise;
    private float m_cogSpeed;

    //Spell Components
    private bool m_isFrozen;

    private float m_counter = 0;

    //Scale components
    private const float c_scaleTime = 2.0f;
    private Vector3 m_smallScale = Vector3.zero;
    private Vector3 m_largeScale = Vector3.zero;
    private Quaternion m_globalRotation;

    private IEnumerator m_rotationReference;
    private IEnumerator m_stutterRotationReference;

    //Public accessors 
    //Should be removed, unless frozen is explicitly needed
    public bool IsFrozen { get { return m_isFrozen; } }
    public bool IsRotating { get { return m_isRotating; } }

    public Quaternion GlobalRotation { get { return m_globalRotation; } set { m_globalRotation = value; } }

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


        if (GetMagicState(SpellType.TRANSFORM_TEMPERATURE_COLD) == SpellState.COUNTERSPELLED)
        {
            m_isFrozen = true;
        }

        m_rotationReference = Rotate();
        m_stutterRotationReference = StutterRotate();
        #region Check for starting rotation
        if (m_leftNeighbour != null)
        {
            if (m_leftNeighbour.IsRotating == true && !m_leftNeighbour.IsFrozen && IsCorrectSize())
            {
                StartCoroutine(m_rotationReference);
                m_isRotating = true;
            }

        }
        else if (m_leftNeighbour == null)
        {
            StartCoroutine(m_stutterRotationReference);
            m_isRotating = false;
        }
        #endregion
    }

    protected override void Update()
    {
        base.Update();

        if (m_leftNeighbour != null)
        {
            //Check if current cog is no longer being acted upon
            if (!m_leftNeighbour.IsRotating || m_leftNeighbour.IsFrozen || !IsCorrectSize() || !m_leftNeighbour.IsCorrectSize())
            {
                StopCoroutine(m_rotationReference);
                m_isRotating = false;
            }
            //If the cog is stationary, check if a change has now allowed it to move
            else if (m_leftNeighbour.IsRotating && m_leftNeighbour.IsCorrect() && IsCorrect() && !m_isRotating)
            {
                StartCoroutine(m_rotationReference);
                m_isRotating = true;
            }
        }

        else if (m_leftNeighbour == null)
        {
            if(!m_rightNeighbour.IsFrozen && !m_isRotating)
            {
                StopCoroutine(m_stutterRotationReference);
               // StopAllCoroutines();
                StartCoroutine(m_rotationReference);
                m_isRotating = true;
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

    IEnumerator Rotate()
    {
      //  transform.rotation = m_globalRotation;

        transform.SetPositionAndRotation(transform.position, m_globalRotation);

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

    IEnumerator StutterRotate()
    {
        transform.rotation = m_globalRotation;
        Quaternion tempRot = transform.rotation;
        float tempCounter = 0;

        while (true)
        {
            if (m_isClockwise)
                transform.RotateAround(transform.position, transform.right, Time.deltaTime * m_cogSpeed);
            else
                transform.RotateAround(transform.position, transform.right, Time.deltaTime * -m_cogSpeed);

            tempCounter += Time.deltaTime;
            
            if(tempCounter >= 0.1f)
            {
                transform.rotation = tempRot;
                tempCounter = 0;
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
        StopAllCoroutines();

        StartCoroutine(ScaleObject(m_largeScale));

        m_size = SizeState.LARGE;
    }

    protected override void SpellSizeSmall(Spell spell)
    {
        StopAllCoroutines();
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

        StopAllCoroutines();

        StartCoroutine(ScaleObject(Vector3.one));

        m_size = SizeState.DEFAULT;

        m_isRotating = false;
    }
    #endregion

    public bool IsCorrectSize()
    {
        if (GetMagicState(SpellType.TRANSFORM_SIZE_BIG) != SpellState.SPELLED 
         && GetMagicState(SpellType.TRANSFORM_SIZE_SMALL) != SpellState.COUNTERSPELLED
         && Vector3.Distance(transform.localScale, Vector3.one) <= 0.1f)
        {
            return true;
        }
        else
            return false;
    }

    public bool IsCorrect()
    {
        return !m_isFrozen;
    }

}
