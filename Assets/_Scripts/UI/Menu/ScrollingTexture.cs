using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingTexture : MonoBehaviour
{
    [Header("Animation Attributes")]
    [SerializeField]
    private Vector2 m_direction;

    [SerializeField]
    private float m_speed;

    [Header("Required Elements")]
    [Tooltip("If you get stuck with the set up, message Alastair")]
    [SerializeField]
    private Material m_material;

    private Vector2 m_offset = Vector2.zero;
    // Start is called before the first frame update
    void Awake()
    {
        if (!m_material)
            Debug.LogError(name + " is missing it's material");

        //Ensure the material will loop
        m_material.mainTexture.wrapMode = TextureWrapMode.Repeat;

        //To prevent change in speed from direction variable
        m_direction = m_direction.normalized;
    }

    private void Update()
    {
        m_offset += new Vector2((Time.unscaledDeltaTime * m_speed) * m_direction.x, (Time.unscaledDeltaTime * m_speed) * m_direction.y);

        m_material.mainTextureOffset = m_offset;
    }
}
