using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEnchantables
{
    public class MagicBook : Enchantable
    {
        [SerializeField]
        float m_timeToFadeOutInSeconds = 2f;

        Renderer m_renderer;

        private void Start()
        {
            m_renderer = GetComponent<Renderer>();
        }
        protected override void SpellReset(Spell spell)
        {
            // The game's ending scene should start right here.
            // Probably need to take disable player controls.
            // Probably need to take control of the camera.
            StartCoroutine(FadeOut(m_timeToFadeOutInSeconds));

        }

        IEnumerator FadeOut(float seconds)
        {
            float startTime = Time.time;
            float endTime = startTime + seconds;
            float t = 0f;

            while(t < 1f)
            {
                m_renderer.material.SetFloat("_Opacity", 1 - t);
                float now = Time.time;
                t = Mathf.InverseLerp(startTime, endTime, now);
                //print($"{now - startTime} -> {t}");
                yield return new WaitForSeconds(Time.deltaTime);
            }


            yield return null;
        }
    }
}
