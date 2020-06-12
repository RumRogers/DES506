using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameCore.Spells
{
    [DisallowMultipleComponent]
    public class MagicProfile : MonoBehaviour
    {
        [Serializable]
        public struct CastableSpells
        {
            public bool sizeSpell;
            public bool temperatureSpell;
        }

        [Serializable]
        public struct MagicState
        {
            public SpellState size;
            public SpellState temperature;
        }

        public struct MagicFingerprint
        {
            public CastableSpells castableSpells;
            public MagicState magicState;
        }

        [SerializeField]
        protected CastableSpells m_castableSpells;
        [SerializeField]
        protected MagicState m_magicState;

        public MagicFingerprint GetMagicFingerprint()
        {
            MagicFingerprint fingerprint;
            fingerprint.castableSpells = m_castableSpells;
            fingerprint.magicState = m_magicState;

            return fingerprint;
        }

        public void ResetMagicState()
        {
            m_magicState.size = m_magicState.temperature = SpellState.NORMAL;
        }

        public void SetSizeMagicState(SpellState spellState)
        {
            m_magicState.size = spellState;
        }

        public void SetTemperatureMagicState(SpellState spellState)
        {
            m_magicState.temperature = spellState;
        }
    }
}