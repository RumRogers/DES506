﻿using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI.SpellBook
{
    public class Invisible_SpellBookState : State
    {
        SpellBook m_spellBook;
        public Invisible_SpellBookState(SpellBook owner) : base(owner)
        {
            m_spellBook = owner;
            m_spellBook.Hide();
        }


        public override void Manage()
        {
            if(Input.GetKeyDown(m_spellBook.p_keyShowSpellBook))
            {
                m_owner.SetState(new Active_SpellBookState(m_spellBook));
            }
        }
    }
}

