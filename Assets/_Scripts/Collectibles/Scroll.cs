using GameCore.Spells;
using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCollectibles
{
    
    public class Scroll : Collectible
    {
        
        [SerializeField]
        SpellType m_unlocksSpell;        

        protected override void TriggerHandler(Collider other)
        {
            LevelManager.UnlockSpell(m_unlocksSpell);            
        }        
    }
}