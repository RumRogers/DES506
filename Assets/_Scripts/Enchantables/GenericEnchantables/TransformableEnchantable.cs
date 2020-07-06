using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEnchantables
{
    public class TransformableEnchantable : Enchantable
    {
        Vector3 m_positionNormal;
        Quaternion m_rotationNormal;
        Vector3 m_scaleNormal;

        [Header("Transform parameters for the \"Big\" state")]
        [SerializeField]
        Vector3 m_positionBig;
        [SerializeField]
        Vector3 m_rotationBigEulers;
        [SerializeField]
        Vector3 m_scaleBig;
        Quaternion m_rotationBig;

        [Header("Transform parameters for the \"Small\" state")]
        [SerializeField]
        Vector3 m_positionSmall;
        [SerializeField]
        Vector3 m_rotationSmallEulers;
        [SerializeField]
        Vector3 m_scaleSmall;
        Quaternion m_rotationSmall;

        [Header("Speed of each type of transformation")]
        [SerializeField]
        float m_lerpPositionSpeed = 1f;
        [SerializeField]
        float m_lerpRotationSpeed = 1f;
        [SerializeField]
        float m_lerpScaleSpeed = 1f;

        // Start is called before the first frame update
        void Start()
        {
            m_positionNormal = transform.position;
            m_rotationNormal = transform.rotation;
            m_scaleNormal = transform.localScale;
            m_rotationBig = Quaternion.Euler(m_rotationBigEulers);
            m_rotationSmall = Quaternion.Euler(m_rotationSmallEulers);
        }

        protected override void SpellSizeBig(Spell spell)
        {
            DoTransform(m_positionBig, m_rotationBig, m_scaleBig);
        }

        protected override void SpellSizeSmall(Spell spell)
        {
            DoTransform(m_positionSmall, m_rotationSmall, m_scaleSmall);
        }

        protected override void SpellReset(Spell spell)
        {
            DoTransform(m_positionNormal, m_rotationNormal, m_scaleNormal);
        }

        private void DoTransform(Vector3 targetPosition, Quaternion targetRotation, Vector3 targetScale)
        {
            StartCoroutine(ChangePosition(targetPosition));
            StartCoroutine(ChangeRotation(targetRotation));
            StartCoroutine(ChangeScale(targetScale));
        }

        private IEnumerator ChangePosition(Vector3 targetPosition)
        {
            float t = 0f;
            Vector3 sourcePosition = transform.localPosition;

            while (t < 1)
            {
                transform.localPosition = Vector3.Lerp(sourcePosition, targetPosition, t);
                t += Time.deltaTime * m_lerpPositionSpeed;
                yield return new WaitForSeconds(Time.deltaTime);

            }
        }

        private IEnumerator ChangeRotation(Quaternion targetRotation)
        {
            float t = 0f;
            Quaternion sourceRotation = transform.rotation;

            while (t < 1)
            {
                transform.rotation = Quaternion.Lerp(sourceRotation, targetRotation, t);
                t += Time.deltaTime * m_lerpRotationSpeed;
                yield return new WaitForSeconds(Time.deltaTime);

            }
        }

        private IEnumerator ChangeScale(Vector3 targetScale)
        {
            float t = 0f;
            Vector3 sourceScale = transform.localScale;

            while (t < 1)
            {
                transform.localScale = Vector3.Lerp(sourceScale, targetScale, t);
                t += Time.deltaTime * m_lerpScaleSpeed;
                yield return new WaitForSeconds(Time.deltaTime);

            }
        }
    }
}

