using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;

namespace GameMutables
{

    public class ScalableMutable : MutableEntity
    {
        Vector3 m_scaleNormal;
        [SerializeField]
        float m_scaleBigMultiplier = 2f;
        [SerializeField]
        float m_scaleSmallMultiplier = .5f;
        [SerializeField]
        float m_changeSizeSpeed = 1f;

        // Start is called before the first frame update
        void Start()
        {
            m_scaleNormal = transform.localScale;
        }

        public override void Is(string lexeme)
        {
            base.Is(lexeme);
            switch(lexeme)
            {
                case "big": 
                    StartCoroutine(ChangeSize(m_scaleNormal * m_scaleBigMultiplier));
                    break;
                case "small":
                    StartCoroutine(ChangeSize(m_scaleNormal * m_scaleSmallMultiplier));
                    break;
                default:
                    break;
            }            
        }

        public override void UndoIs(string lexeme)
        {
            base.Is(lexeme);
            switch (lexeme)
            {
                case "big":
                case "small":
                    StartCoroutine(ChangeSize(m_scaleNormal));
                    break;
                default:
                    break;
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
    }
}