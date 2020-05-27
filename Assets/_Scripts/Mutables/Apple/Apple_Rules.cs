using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;

namespace GameMutables
{
    public class Apple_Rules : MutableEntity
    {
        Renderer m_renderer;
        [SerializeField]
        Color m_colorRed = Color.red;
        [SerializeField]
        Color m_colorGreen = Color.green;
        [SerializeField]
        Vector3 m_sizeBig = new Vector3(2f, 2f, 2f);
        [SerializeField]
        Vector3 m_sizeSmall = new Vector3(.5f, .5f, .5f);
        [SerializeField]
        float m_changeSizeSpeed = 1f;

        void Start()
        {
            m_renderer = GetComponent<Renderer>();
        }

        public override void Is(string lexeme)
        {           
            Debug.Log("Apple is " + lexeme); 
            switch(lexeme)
            {
                case "gone":
                    m_renderer.enabled = false;
                    break;
                case "big":
                    StartCoroutine(ChangeSize(m_sizeBig));
                    break;
                case "small":
                    StartCoroutine(ChangeSize(m_sizeSmall));
                    break;
                case "red":
                    StartCoroutine(ChangeColor(m_colorRed));
                    break;
                case "green":
                    StartCoroutine(ChangeColor(m_colorGreen));
                    break;
                default:
                    Debug.Log("This rule makes no sense, dude!");
                    return;
            }
        }        

        private IEnumerator ChangeSize(Vector3 targetScale)
        {
            float t = 0f;
            Vector3 sourceScale = transform.localScale;

            while (t < 1)
            {
                transform.localScale = Vector3.Lerp(sourceScale, targetScale, t);
                t += Time.deltaTime * m_changeSizeSpeed;
                yield return new WaitForSeconds(Time.deltaTime);

            }
        }

        private IEnumerator ChangeColor(Color targetColor)
        {
            float t = 0f;
            Color sourceColor = m_renderer.material.color;

            while(t < 1)
            {
                m_renderer.material.SetColor("_Color", Color.Lerp(sourceColor, targetColor, t));
                t += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
               
            }
        }
    }
}

