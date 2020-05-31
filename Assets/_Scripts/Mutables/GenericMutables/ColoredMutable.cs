using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMutables
{
    public class ColoredMutable : MonoBehaviour
    {
        Renderer m_renderer;
        [SerializeField]
        Color m_colorRed = Color.red;
        [SerializeField]
        Color m_colorGreen = Color.green;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

