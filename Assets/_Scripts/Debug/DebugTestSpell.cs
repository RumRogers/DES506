using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;
using GameCore.Spells;

public class DebugTestSpell : MonoBehaviour
{
    public static Enchantable s_enchantbaleMouseOver = null;
    Vector2 m_topLeft = new Vector2(10, 10);
    Vector2 m_btnSize = new Vector2(100, 20);
    float m_verticalSpacing = 5f;

    private void OnGUI()
    {
        Vector2 pos = m_topLeft;

            if(s_enchantbaleMouseOver != null && s_enchantbaleMouseOver.IsCastable(SpellType.TRANSFORM_SIZE))
            {
                if(s_enchantbaleMouseOver.GetMagicState(SpellType.TRANSFORM_SIZE) != SpellState.SPELLED)
                {
                    if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Make Big"))
                    {
                        s_enchantbaleMouseOver.CastSpell(new Spell(SpellType.TRANSFORM_SIZE));
                        s_enchantbaleMouseOver = null;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Make Small"))
                    {
                        s_enchantbaleMouseOver.CastSpell(new Spell(SpellType.TRANSFORM_SIZE));
                        s_enchantbaleMouseOver = null;
                    }
                }

                pos.y += m_btnSize.y + m_verticalSpacing;
            }
            if (s_enchantbaleMouseOver != null && s_enchantbaleMouseOver.IsCastable(SpellType.TRANSFORM_TEMPERATURE))
            {
                if (s_enchantbaleMouseOver.GetMagicState(SpellType.TRANSFORM_TEMPERATURE) != SpellState.SPELLED)
                {
                    if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Make Hot"))
                    {
                        s_enchantbaleMouseOver.CastSpell(new Spell(SpellType.TRANSFORM_TEMPERATURE));
                        s_enchantbaleMouseOver = null;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Make Cold"))
                    {
                        s_enchantbaleMouseOver.CastSpell(new Spell(SpellType.TRANSFORM_TEMPERATURE));
                        s_enchantbaleMouseOver = null;
                    }
                }

                pos.y += m_btnSize.y + m_verticalSpacing;
            }

            if (s_enchantbaleMouseOver != null && GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Reset Spells"))
            {
                s_enchantbaleMouseOver.CastSpell(new Spell(SpellType.TRANSFORM_RESET));
                s_enchantbaleMouseOver = null;
            }
    }
}
