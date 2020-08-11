using GameCore.Camera;
using GameCore.Spells;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameEnchantables
{
    public class MagicBook : Enchantable
    {
        delegate void LerpDelegate(float t);

        Shader m_shader;

        [Header("Timing variables")]
        [SerializeField]
        float m_timeToWaitBeforeStartingCutscene = 0f;
        [SerializeField]
        float m_timeToBlendTexturesInSeconds = 2f;
        [SerializeField]
        float m_timeToFadeToBlackInSeconds = 1f;

        Image m_blackScreenImage;
        Image m_finalImage;
        List<GameObject> m_gameObjectsToDisable = new List<GameObject>();

        PlayerEntity m_playerEntity;
        PlayerMoveCamera m_playerMoveCamera;

        Renderer m_renderer;

        bool m_gameOver = false;

        LerpDelegate m_textureBlend;
        LerpDelegate m_fadeToBlack;

        // We shouldn't use names but tags instead. However it's kinda late for this, deadline's in a week so... whatever.
        const string BLACK_SCREEN_IMAGE_OBJECT_NAME = "BlackScreen";
        const string FINAL_IMAGE_OBJECT_NAME = "FinalImage";
        const string TIPS_OBJECT_NAME = "Tips";
        const string TIP_PANELS_OBJECT_NAME = "Canvas";
        const string GAME_HUD_OBJECT_NAME = "UI";
        const string PAUSE_MENU_TAG_NAME = "PauseMenu";

        private void Start()
        {
            m_renderer = GetComponent<Renderer>();
            m_shader = m_renderer.material.shader;
            m_playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
            m_playerMoveCamera = Camera.main.GetComponent<PlayerMoveCamera>();
            m_blackScreenImage = GameObject.Find(BLACK_SCREEN_IMAGE_OBJECT_NAME).GetComponent<Image>();
            m_finalImage = GameObject.Find(FINAL_IMAGE_OBJECT_NAME).GetComponent<Image>();

            m_gameObjectsToDisable.Add(GameObject.Find(TIPS_OBJECT_NAME));
            m_gameObjectsToDisable.Add(GameObject.Find(TIP_PANELS_OBJECT_NAME));
            m_gameObjectsToDisable.Add(GameObject.Find(GAME_HUD_OBJECT_NAME));
            m_gameObjectsToDisable.Add(GameObject.FindGameObjectWithTag(PAUSE_MENU_TAG_NAME));

            m_textureBlend = t => m_renderer.material.SetFloat("_Opacity", 1 - t);
            m_fadeToBlack = t => 
            {
                Color c = m_blackScreenImage.color;
                c.a = t;
                m_blackScreenImage.color = c;
            };
        }
        protected override void SpellReset(Spell spell)
        {
            // The game's ending scene should start right here.
            // Probably need to take disable player controls.
            // Probably need to take control of the camera.

            TakeFullControl();
            StartCoroutine(EndingSequence());
        }

        protected override void Update()
        {
            base.Update();
            if(m_gameOver && Input.anyKeyDown)
            {
                // Ain't working for some reason
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;

                SceneManager.LoadScene("MainMenuScreen");
            }
        }

        private void TakeFullControl()
        {
            m_renderer.material.shader = m_shader;
            m_playerEntity.SetState(new Default_PlayerState(m_playerEntity));
            m_playerMoveCamera.SetState(new Idle_CameraState(m_playerMoveCamera));
            m_playerEntity.enabled = false;
            m_playerMoveCamera.enabled = false;

            foreach(var gameObject in m_gameObjectsToDisable)
            {
                gameObject.SetActive(false);
            }
        }

        IEnumerator EndingSequence()
        {            
            yield return StartCoroutine(TimeBasedLerp(m_timeToBlendTexturesInSeconds, m_textureBlend));
            yield return StartCoroutine(TimeBasedLerp(m_timeToFadeToBlackInSeconds, m_fadeToBlack));
            m_finalImage.enabled = true;

            m_gameOver = true;
        }

        IEnumerator TimeBasedLerp(float seconds, LerpDelegate lerpDelegate)
        {
            float startTime = Time.time;
            float endTime = startTime + seconds;
            float t = 0f;

            while (t < 1f)
            {
                lerpDelegate(t);
                float now = Time.time;
                t = Mathf.InverseLerp(startTime, endTime, now);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            lerpDelegate(1f);


            yield return null;
        }


    }
}
