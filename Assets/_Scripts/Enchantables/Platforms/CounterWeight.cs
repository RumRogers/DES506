using GameCore.Spells;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
        m_smallScale = transform.localScale * m_smallScaleFactor;
        m_largeScale = transform.localScale * m_largeScaleFactor;

        m_rigidBody = GetComponent<Rigidbody>();

        m_defaultMass = m_rigidBody.mass;

        m_renderer = GetComponent<Renderer>();

        if(m_seesaw == null)
        {
            Debug.LogError("The " + transform.name + " is missing it's seesaw reference");
        }
    }

    protected override void Update()
    {
        base.Update();
        m_dimensions = m_renderer.bounds.size;

        RaycastHit m_rayHit;

        float temp = m_seesaw.GetDimensions().y/2 - m_dimensions.y/2;

        //transform.position = new Vector3(transform.position.x, temp, transform.position.z);
        //keep box the distance of half platdim, and half box dim (both in the y) with relation to the up vector
        //float temp = m_dimensions.y - ;
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

    #region Spell Functions

    protected override void SpellSizeBig(Spell spell)
    {
        StartCoroutine(ScaleObject(m_largeScale));

        m_rigidBody.mass = m_largeMass;
    }

    protected override void SpellSizeSmall(Spell spell)
    {
        StartCoroutine(ScaleObject(m_smallScale));

        m_rigidBody.mass = m_smallMass;
    }

    protected override void SpellReset(Spell spell)
    {
        StartCoroutine(ScaleObject(Vector3.one));

        m_rigidBody.mass = m_defaultMass;
    }

    #endregion
}
