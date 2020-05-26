using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTestRule : MonoBehaviour
{
    List<string> m_ruleSlots = new List<string>(5) { "", "", "", "", "" };
    Vector2 m_topLeft = new Vector2(10, 10);
    Vector2 m_labelSize = new Vector2(100, 20);

    private void OnGUI()
    {
        Vector2 pos = m_topLeft;

        for(int i = 0; i < m_ruleSlots.Count; i++)
        {
            m_ruleSlots[i] = GUI.TextField(new Rect(pos.x, pos.y, m_labelSize.x, m_labelSize.y), m_ruleSlots[i], 25);
            pos.y += m_labelSize.y + 5;
        }
    }
}
