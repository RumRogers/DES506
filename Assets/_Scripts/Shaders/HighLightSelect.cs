using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighLightSelect : MonoBehaviour
{
    public Material m_noramlMat;
    public Material m_outlineMat;
    public Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnMouseExit()
    {
        renderer.material = m_noramlMat;
        
    }

    private void OnMouseOver()
    {
        renderer.material = m_outlineMat;
    }
}
