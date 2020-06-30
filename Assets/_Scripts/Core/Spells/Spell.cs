using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameCore.Spells
{
    public enum SpellType
    {
        TRANSFORM_SIZE_BIG,
        TRANSFORM_SIZE_SMALL,
        TRANSFORM_TEMPERATURE_COLD,
        TRANSFORM_RESET,
        NONE,
        TRANSFORM_TEMPERATURE_HOT
    }

    public enum SpellState
    {
        NORMAL = 0, SPELLED, COUNTERSPELLED
    }

    public class Spell
    {
        public readonly SpellType m_type;

        public Spell(SpellType type)
        {
            m_type = type;
        }

        public override string ToString()
        {
            return $"Spell (type: {m_type})";
        }
    }
}

