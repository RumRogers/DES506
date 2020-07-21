using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualEffects
{
    public class FreezeEffect : MonoBehaviour
    {
        MaterialPropertyBlock m_matBlock;
        Renderer m_renderer;
        Shader m_originalShader;
        Shader m_blendShader;
        //might want a different texture per object based on how the model is UV wrapped
        [SerializeField] Texture m_iceTexture;
        [SerializeField] Texture m_mainTexture;
        float m_matLerpValue = 0;
        GameCore.Spells.MagicProfile m_magicProfile;
        bool m_lerped = false;

        const float FROZEN_LERP_VALUE = 1.0f;
        const float DEFAULT_LERP_VALUE = 0.0f;
        const float LERP_TIME = 2.0f;
        const string BLEND_SHADER_PATH = "Shaders/Blend/UnlitBlendTexture";
        const string ICE_TEX_DEFAULT_PATH = "Art Assets/Ice Tiles/IceTemp";

        // Start is called before the first frame update
        void Start()
        {
            m_matBlock = new MaterialPropertyBlock();
            if (!TryGetComponent<Renderer>(out m_renderer))
            {
                Debug.LogError("No renderer component attached to object with Frozen visual effect");
            }
            if(!TryGetComponent<GameCore.Spells.MagicProfile>(out m_magicProfile))
            {
                Debug.LogError("No magic profile attached to object with Frozen visual effect");
            }
            //m_iceTexture = (Texture)Resources.Load(ICE_TEXTURE_PATH);
            m_matBlock.SetTexture("_MainTex", m_mainTexture);
            m_matBlock.SetTexture("_BlendTex", m_iceTexture);
            m_renderer.SetPropertyBlock(m_matBlock);
            m_originalShader = m_renderer.material.shader;
            m_blendShader = Resources.Load<Shader>(BLEND_SHADER_PATH);
            m_renderer.material.shader = m_blendShader;

            //if not declaired, set to default ice texture
            if (m_iceTexture == null)
            {
                m_iceTexture = Resources.Load<Texture>(ICE_TEX_DEFAULT_PATH);
            }

            if (m_mainTexture == null)
            {
                m_mainTexture = m_renderer.material.mainTexture;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //if Frozen and we haven't already lerped to frozen
            if (m_magicProfile.GetMagicFingerprint().magicState.temperature == GameCore.Spells.SpellState.COUNTERSPELLED && !m_lerped)
            {
                m_renderer.material.shader = m_blendShader;
                m_lerped = true;
                StopAllCoroutines();
                StartCoroutine(TransitionBetweenStates(FROZEN_LERP_VALUE, LERP_TIME));
            }
            //if not frozen and not already lerped to default
            else if (m_magicProfile.GetMagicFingerprint().magicState.temperature != GameCore.Spells.SpellState.COUNTERSPELLED && m_lerped)
            {
                m_lerped = false;
                StopAllCoroutines();
                StartCoroutine(TransitionBetweenStates(DEFAULT_LERP_VALUE, LERP_TIME));
            }
        }

        IEnumerator TransitionBetweenStates(float lerpTowards, float timeTaken)
        {
            float time = 0;
            float startLerpValue = m_matLerpValue;
            while (true)
            {
                time += Time.deltaTime;

                float perComp = time / timeTaken;

                m_matLerpValue = Mathf.Lerp(startLerpValue, lerpTowards, perComp);

                m_matBlock.SetFloat("_LerpValue", m_matLerpValue);
                m_renderer.SetPropertyBlock(m_matBlock);

                if (perComp > 0.99)
                {
                    m_matLerpValue = lerpTowards;
                    if (lerpTowards == DEFAULT_LERP_VALUE)
                    {
                        m_renderer.material.shader = m_originalShader;
                    }
                    yield break;
                }

                yield return null;
            }
        }
    }
}