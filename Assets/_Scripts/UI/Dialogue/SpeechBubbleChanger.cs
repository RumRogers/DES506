using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleChanger : MonoBehaviour
{
    [Header("Far and close sprites")]
    [SerializeField]
    Sprite m_farSprite;
    [SerializeField]
    Sprite m_closeSprite;
    [Header("Threshold distance to change to bubble speech icon")]
    [SerializeField]
    float m_distanceThreshold = 2;

    SpriteRenderer m_spriteRenderer;
    Transform m_playerTransform;
    float m_distance;

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;   
    }

    // Update is called once per frame
    void Update()
    {
        print(Vector3.Distance(transform.position, m_playerTransform.position));
        if(Vector3.Distance(transform.position, m_playerTransform.position) <= m_distanceThreshold)
        {
            m_spriteRenderer.sprite = m_closeSprite;
        }
        else
        {
            m_spriteRenderer.sprite = m_farSprite;
        }
    }
}
