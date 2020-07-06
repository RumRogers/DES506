using GameCore.Spells;
using System.Collections;
using UnityEngine;

public class Platform : Enchantable
{
    #region Parameters
    [Header("Platform Parameters")]
    [SerializeField]
    [Range(0.1f, 12.0f)]
    private float m_motionWidth = 1.0f;

    [SerializeField]
    [Range(0.1f, 10.0f)]
    private float m_platformSpeed = 0.1f;

    //Contained Variables
    private float m_counter = 0;

    private const float c_scaleTime = 2.0f;

    private bool m_isMoving = true;

    private Vector3 m_position = Vector3.zero;   
    private Vector3 m_smallScale = new Vector3(0.2f, 0.2f, 0.2f);
    private Vector3 m_largeScale = new Vector3(2.0f, 2.0f, 2.0f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Retain original position
        m_position = transform.position;
    }
    
    #region Enumerators
    IEnumerator Translate()
    {
        m_isMoving = true;
        while (true)
        {
            m_counter += Time.deltaTime * m_platformSpeed;
            transform.position = m_position + (transform.right * (Mathf.Sin(m_counter)) * m_motionWidth);
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
        StartCoroutine(Translate());
    }

    protected override void SpellReset(Spell spell) 
    {
        StartCoroutine(Translate());
        StartCoroutine(ScaleObject(Vector3.one));
    }
#endregion
}
