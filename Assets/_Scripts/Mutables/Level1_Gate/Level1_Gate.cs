using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;

namespace GameMutables
{
    public class Level1_Gate : MutableEntity
    {
        Animator m_animator;
        [SerializeField]
        string m_openTriggerName;
        [SerializeField]
        string m_closeTriggerName;
        [SerializeField]
        List<Light> m_lights;
        [SerializeField]
        Light m_eye;
        [SerializeField]
        float m_secondsBetweenLightsOn;
        [SerializeField]
        float m_targetLightIntensity = 1f;
        [SerializeField]
        float m_targetEyeIntensity = 7f;

        // Start is called before the first frame update
        void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        public override void Is(string lexeme)
        {
            base.Is(lexeme);
            switch(lexeme)
            {
                case "open":
                    StartCoroutine(OpenGate());
                    break;
                case "lit":
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
                case "open":
                    CloseGate();
                    break;
                default:
                    break;

            }
        }

        IEnumerator OpenGate()
        {
            foreach(var light in m_lights)
            {
                light.intensity = m_targetLightIntensity;
                yield return new WaitForSeconds(m_secondsBetweenLightsOn);
            }
            m_eye.intensity = m_targetEyeIntensity;

            yield return new WaitForSeconds(.3f);
            m_animator.SetTrigger(m_openTriggerName);
        }

        void CloseGate()
        {

            foreach (var light in m_lights)
            {
                light.intensity = 0f;
            }
            m_eye.intensity = 0f;
            m_animator.SetTrigger(m_closeTriggerName);
        }
    }
}

