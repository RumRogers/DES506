using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;

public class DebugTestRule : MonoBehaviour
{
    List<string> m_ruleSlots = new List<string>(3) { "", "", "" };
    Vector2 m_topLeft = new Vector2(10, 10);
    Vector2 m_labelSize = new Vector2(100, 20);
    Vector2 m_btnSize = new Vector2(100, 20);
    float m_verticalSpacing = 5f;

    private void OnGUI()
    {
        Vector2 pos = m_topLeft;

        for(int i = 0; i < m_ruleSlots.Count; i++)
        {
            m_ruleSlots[i] = GUI.TextField(new Rect(pos.x, pos.y, m_labelSize.x, m_labelSize.y), m_ruleSlots[i], 25);
            pos.y += m_labelSize.y + m_verticalSpacing;
        }
        if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Apply Rule"))
        {
            Rule r = new Rule(new RuleChunk(RuleChunk.ChunkType.SUBJECT, m_ruleSlots[0]),
                new RuleChunk(RuleChunk.ChunkType.VERB, m_ruleSlots[1]),
                new RuleChunk(RuleChunk.ChunkType.OBJECT, m_ruleSlots[2]));
            r.Apply();
            
        }

        pos.y += m_btnSize.y + m_verticalSpacing;

        if (GUI.Button(new Rect(pos.x, pos.y, m_btnSize.x, m_btnSize.y), "Undo Rule"))
        {
            Rule r = new Rule(new RuleChunk(RuleChunk.ChunkType.SUBJECT, m_ruleSlots[0]),
                new RuleChunk(RuleChunk.ChunkType.VERB, m_ruleSlots[1]),
                new RuleChunk(RuleChunk.ChunkType.OBJECT, m_ruleSlots[2]));
            r.Apply(Rule.ApplicationMode.UNDO);
        }
    }
}
