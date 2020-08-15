using GameCore.Camera;
using GameCore.Spells;
using GameCore.System;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.SpellBook
{
    public class SpellBook : Automaton
    {
        [SerializeField]
        private KeyCode m_keyShowSpellBook = KeyCode.F1;

        [SerializeField]
        Transform m_mainContainer;       

        [SerializeField]
        List<Transform> m_lockedSpellSlots = new List<Transform>();
        [SerializeField]
        List<Transform> m_unlockedSpellSlots = new List<Transform>();
        [SerializeField]
        List<Transform> m_selectionIcons = new List<Transform>();
        [SerializeField]
        List<Transform> m_spellDescriptions = new List<Transform>();
        [SerializeField]
        List<Text> m_spellCaptions = new List<Text>();

        [SerializeField]
        Color m_selectedTextColor;
        [SerializeField]
        Color m_unselectedTextColor;

        PlayerEntity m_playerEntity;
        PlayerMoveCamera m_playerMoveCamera;
        State m_previousPlayerState = null;
        State m_previousCameraState = null;

        bool m_isActive = false;
        public bool p_IsActive { get => m_isActive; }
        public KeyCode p_keyShowSpellBook { get => m_keyShowSpellBook; }

        SpellType m_currentlySelectedSpell = SpellType.NONE;
        public SpellType p_CurrentlySelectedSpell { get => m_currentlySelectedSpell; }

        private void Start()
        {
            m_playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
            m_playerMoveCamera = Camera.main.GetComponent<PlayerMoveCamera>();

            for(int i = (int)SpellType.TRANSFORM_SIZE_BIG; i <= (int)SpellType.TRANSFORM_RESET; ++i)
            {
                m_lockedSpellSlots[i].gameObject.SetActive(true);
                m_unlockedSpellSlots[i].gameObject.SetActive(false);
                m_spellDescriptions[i].gameObject.SetActive(false);
                m_selectionIcons[i].gameObject.SetActive(false);
                m_spellDescriptions[i].gameObject.SetActive(false);
            }
           
            SetState(new Invisible_SpellBookState(this));            
        }

        public void Display()
        {
            m_isActive = true;
            m_previousPlayerState = m_playerEntity.GetState();
            m_previousCameraState = m_playerMoveCamera.GetState();
            m_playerEntity.SetState(new Idle_PlayerState(m_playerEntity));
            m_playerMoveCamera.SetState(new Idle_CameraState(m_playerMoveCamera));
            m_mainContainer.gameObject.SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/SpellbookOpen");
            if (LevelManager.IsSpellUnlocked(SpellType.TRANSFORM_SIZE_BIG))
            {
                SetSelectedSpell(SpellType.TRANSFORM_SIZE_BIG);
            }
        }

        public void Hide()
        {
            m_isActive = false;
            m_mainContainer.gameObject.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/SpellbookClose");
            if (m_previousPlayerState != null)
            {
                m_playerEntity.SetState(m_previousPlayerState);
            }
            if (m_previousCameraState != null)
            {
                m_playerMoveCamera.SetState(m_previousCameraState);
            }
        }
        
        public void UnlockSpell(SpellType spellType)
        {
            m_lockedSpellSlots[(int)spellType].gameObject.SetActive(false);
            m_unlockedSpellSlots[(int)spellType].gameObject.SetActive(true);
        }

        public void SetSelectedSpell(SpellType spellType)
        {
            m_currentlySelectedSpell = spellType;
            int selectedIdx = (int)spellType;
            for(int i = (int)SpellType.TRANSFORM_SIZE_BIG; i <= (int)SpellType.TRANSFORM_RESET; ++i)
            {
                bool active = i == selectedIdx;
                m_selectionIcons[i].gameObject.SetActive(active);
                m_spellDescriptions[i].gameObject.SetActive(active);
                m_spellCaptions[i].color = active ? m_selectedTextColor : m_unselectedTextColor;
            }
        }

        public bool CanBeOpened()
        {
            return m_playerEntity.IsGrounded();
        }
    }
}