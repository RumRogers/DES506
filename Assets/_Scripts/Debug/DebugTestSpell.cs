using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;
using GameCore.Spells;

public class DebugTestSpell : MonoBehaviour
{
    static GameObject s_enchantbaleGameObjMouseOver = null;
    static List<Enchantable> s_enchantables = new List<Enchantable>();
    Vector2 m_topLeft = new Vector2(10, 10);
    Vector2 m_btnSize = new Vector2(100, 20);
    float m_verticalSpacing = 5f;

    private void OnGUI()
    {
        if (s_enchantbaleGameObjMouseOver == null)
        {
            return;
        }

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
        }

        if(clicked)
        {
            SetHighlighted(null);
        }
    }

    public static void SetHighlighted(GameObject gameObject)
    {
        if(gameObject != s_enchantbaleGameObjMouseOver)
        {
            s_enchantbaleGameObjMouseOver = gameObject;
            s_enchantables.Clear();
        }
    }

    public static void RegisterEnchantable(Enchantable enchantableComponent)
    {
        s_enchantables.Add(enchantableComponent);
    }
}
