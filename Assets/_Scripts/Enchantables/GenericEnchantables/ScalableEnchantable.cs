using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEnchantables
{
    public class ScalableEnchantable : Enchantable
    {
        Vector3 m_scaleNormal;
        [SerializeField]
        float m_scaleBigMultiplier = 2f;
        [SerializeField]
        float m_scaleSmallMultiplier = .5f;
        [SerializeField]
        float m_changeSizeSpeed = 1f;
        //[SerializeField]
        //bool m_

        // Start is called before the first frame update
        void Start()
        {
            m_scaleNormal = transform.localScale;
        }

        protected override void SpellSizeBig(Spell spell)
        {
            StartCoroutine(ChangeSize(m_scaleNormal * m_scaleBigMultiplier));
        }

        protected override void SpellSizeSmall(Spell spell)
        {
            StartCoroutine(ChangeSize(m_scaleNormal * m_scaleSmallMultiplier));
        }

        protected override void SpellReset(Spell spell)
        {
            StartCoroutine(ChangeSize(m_scaleNormal));
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

