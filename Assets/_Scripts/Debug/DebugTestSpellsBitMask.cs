using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using GameCore.Utils;
using GameCore.System;

public class DebugTestSpellsBitMask : MonoBehaviour
{
    [SerializeField]
    Vector2 m_labelTopLeft;
    [SerializeField]
    Vector2 m_labelSize;

    private void OnGUI()
    {
        GUI.Label(new Rect(m_labelTopLeft.x, m_labelTopLeft.y, m_labelSize.x, m_labelSize.y), Bits.GetBinaryString(LevelManager.p_PlayerSpells));
    }
}
