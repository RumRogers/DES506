using GameCore.Spells;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CounterWeight : Enchantable
{
    [Header("Large Scale")]
    [SerializeField]
    private float m_largeScaleFactor;

    [Header("Small Scale")]
    [SerializeField]
    private float m_smallScaleFactor;
    
    private Vector3 m_smallScale;
    private Vector3 m_largeScale;

    private Renderer m_renderer;

    float m_counter = 0;

    private const float c_scaleTime = 2.0f;

    public bool IsCorrectSize()
    {
        if (GetMagicState(SpellType.TRANSFORM_SIZE_BIG) == SpellState.SPELLED)
            return true;
        
        else
            return false;
    }
    private void Start()
    {
        m_largeScale = Vector3.one * m_largeScaleFactor;
        m_smallScale = Vector3.one * m_smallScaleFactor;

        m_renderer = GetComponent<Renderer>();
    }

    protected override void Update()
    {

    }

    IEnumerator ScaleObject(Vector3 scale)
    {
        Vector3 originalScale = transform.localScale;
        m_counter = 0;

        //Cast ray
        RaycastHit m_rayInfo;
        Physics.Raycast(transform.position, Vector3.down, out m_rayInfo);

        //Point of collision is the point of reference for scaling

        while (m_counter < c_scaleTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, scale, m_counter/2);

            Physics.Raycast(transform.position, Vector3.down, out m_rayInfo);

            Vector3 m_pos = m_rayInfo.point;

            m_pos += new Vector3(0, (m_renderer.bounds.size.y/2.6f), 0);

            transform.SetPositionAndRotation(m_pos, transform.rotation);

            m_counter += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    #region Spell Functions

    protected override void SpellSizeBig(Spell spell)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(m_largeScale));
    }

    protected override void SpellSizeSmall(Spell spell)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(m_smallScale));

    }

    protected override void SpellReset(Spell spell)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(Vector3.one));
    }

    #endregion
}
