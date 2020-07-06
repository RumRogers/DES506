using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTestSpell_Manual : MonoBehaviour
{
    [SerializeField]
    Enchantable m_enchantable;

    private void OnGUI()
    {
        if (m_enchantable == null)
        {
            return;
        }
        /*
        bool clicked = false;
        Vector2 pos = m_topLeft;

        if (s_enchantables[0].IsCastable(SpellType.TRANSFORM_SIZE_BIG))
        {
            if (s_enchantables[0].GetMagicState(SpellType.TRANSFORM_SIZE_BIG) != SpellState.SPELLED)
            {
                if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Make Big"))
                {
                    clicked = true;
                    foreach (var enchantable in s_enchantables)
                    {
                        enchantable.CastSpell(new Spell(SpellType.TRANSFORM_SIZE_BIG));
                    }
                }

                pos.y += m_btnSize.y + m_verticalSpacing;
            }
            if (s_enchantables[0].GetMagicState(SpellType.TRANSFORM_SIZE_SMALL) != SpellState.COUNTERSPELLED)
            {
                if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Make Small"))
                {
                    clicked = true;
                    foreach (var enchantable in s_enchantables)
                    {
                        enchantable.CastSpell(new Spell(SpellType.TRANSFORM_SIZE_SMALL));
                    }
                }

                pos.y += m_btnSize.y + m_verticalSpacing;
            }
            if (s_enchantables[0].GetMagicState(SpellType.TRANSFORM_TEMPERATURE_HOT) != SpellState.SPELLED)
            {
                if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Make Hot"))
                {
                    clicked = true;
                    foreach (var enchantable in s_enchantables)
                    {
                        enchantable.CastSpell(new Spell(SpellType.TRANSFORM_TEMPERATURE_HOT));
                    }
                }

                pos.y += m_btnSize.y + m_verticalSpacing;
            }
            if (s_enchantables[0].GetMagicState(SpellType.TRANSFORM_TEMPERATURE_COLD) != SpellState.COUNTERSPELLED)
            {
                if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Make Cold"))
                {
                    clicked = true;
                    foreach (var enchantable in s_enchantables)
                    {
                        enchantable.CastSpell(new Spell(SpellType.TRANSFORM_TEMPERATURE_COLD));
                    }
                }

                pos.y += m_btnSize.y + m_verticalSpacing;
            }
            if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Reset"))
            {
                clicked = true;
                foreach (var enchantable in s_enchantables)
                {
                    enchantable.CastSpell(new Spell(SpellType.TRANSFORM_RESET));
                }
            }
        }*/
    }
}
