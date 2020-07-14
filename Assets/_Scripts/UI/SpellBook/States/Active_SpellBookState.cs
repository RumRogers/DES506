using GameCore.Spells;
using GameCore.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace GameUI.SpellBook
{
    public class Active_SpellBookState : State
    {
        public static KeyCode m_keyHideSpellBook = KeyCode.Escape;
        
        public static KeyCode m_keyUp = KeyCode.UpArrow;
        public static KeyCode m_keyDown = KeyCode.DownArrow;
        public static KeyCode m_keyLeft = KeyCode.LeftArrow;
        public static KeyCode m_keyRight = KeyCode.RightArrow;

        private SpellBook m_spellBook;
        private SpellType m_lastSelectedQuillSpell = SpellType.NONE;

        public Active_SpellBookState(SpellBook owner) : base(owner)
        {
            m_spellBook = owner;
            m_spellBook.Display();
        }

        public override void Manage()
        {
            if(Input.GetKeyDown(m_spellBook.p_keyShowSpellBook))
            {
                m_owner.SetState(new Invisible_SpellBookState(m_spellBook));
                return;
            }

            if(Input.GetKeyDown(m_keyUp) || Input.GetKeyDown(m_keyDown))
            {
                if(LevelManager.IsSpellUnlocked(SpellType.TRANSFORM_RESET))
                {
                    if(m_spellBook.p_CurrentlySelectedSpell != SpellType.TRANSFORM_RESET)
                    {
                        m_lastSelectedQuillSpell = m_spellBook.p_CurrentlySelectedSpell;
                        m_spellBook.SetSelectedSpell(SpellType.TRANSFORM_RESET);
                    }
                    else
                    {
                        m_spellBook.SetSelectedSpell(m_lastSelectedQuillSpell);
                        m_lastSelectedQuillSpell = SpellType.NONE;
                    }                    
                }
            }
            else if(Input.GetKeyDown(m_keyLeft) || Input.GetKeyDown(m_keyRight))
            {
                int targetSelectedSpellIdx = (int)m_spellBook.p_CurrentlySelectedSpell + (Input.GetKeyDown(m_keyLeft) ? -1 : 1);
                targetSelectedSpellIdx = Mathf.Clamp(targetSelectedSpellIdx, (int)SpellType.TRANSFORM_SIZE_BIG, (int)SpellType.TRANSFORM_RESET);
                if(LevelManager.IsSpellUnlocked((SpellType)targetSelectedSpellIdx))
                {
                    m_spellBook.SetSelectedSpell((SpellType)targetSelectedSpellIdx);
                }
            }
        }
    }
}