using GameCore.Spells;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class CounterWeight : Enchantable
{
    [Header("Large Scale")]
    [SerializeField]
    private float m_largeScaleFactor;
    [SerializeField]
    private float m_largeMass;

    [Header("Small Scale")]
    [SerializeField]
    private float m_smallScaleFactor;
    [SerializeField]
    private float m_smallMass;

    [Header("Force")]
    [SerializeField]
    private float m_dropForce;

    [Header("Vital Component")]
    [SerializeField]
    private SeeSaw m_seesaw;

    public Vector3 m_dimensions;
    

    private Vector3 m_smallScale;
    private Vector3 m_largeScale;

    private float m_counter = 0;
    private float m_defaultMass;
    
    private const float c_scaleTime = 2.0f;
    
    private Rigidbody m_rigidBody;
    private Renderer m_renderer;

    private void Start()
    {
        //Set scale types based on objects original scale & scale factors
        m_smallScale = transform.localScale * m_smallScaleFactor;
        m_largeScale = transform.localScale * m_largeScaleFactor;

        //Required components
        m_rigidBody = GetComponent<Rigidbody>();
        m_renderer = GetComponent<Renderer>();

        m_defaultMass = m_rigidBody.mass;

        if(m_seesaw == null || m_rigidBody == null || m_renderer == null)
        {

            Debug.LogError("The " + transform.name + " is missing vital component");
        }
    }

    protected override void Update()
    {
        base.Update();

        Vector3 m_size = m_renderer.bounds.size;

        //Check if object is falling, ensures added speed without messing with weight
        if (!Physics.Raycast(transform.position, Vector3.down, m_size.y + 0.07f))
        {
           m_rigidBody.AddForceAtPosition(Vector3.down * m_dropForce, transform.position);
        }
    }

    IEnumerator ScaleObject(Vector3 scale)
    {
        Vector3 originalScale = transform.localScale;
        m_counter = 0;

        while (m_counter < c_scaleTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, scale, m_counter/2);
            m_counter += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    #region Spell Functions

    protected override void SpellSizeBig(Spell spell)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(m_largeScale));

        m_rigidBody.mass = m_largeMass;
    }

    protected override void SpellSizeSmall(Spell spell)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(m_smallScale));

        m_rigidBody.mass = m_smallMass;
    }

    protected override void SpellReset(Spell spell)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(Vector3.one));

        m_rigidBody.mass = m_defaultMass;
    }

    #endregion
}
