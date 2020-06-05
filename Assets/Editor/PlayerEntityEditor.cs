
using UnityEngine;
using UnityEditor;

public class PlayerEntityEditor : EditorWindow
{
    int m_propertyCheckWidth = 100;
    int m_propertyCheckHeight = 25;

    int m_buttonWidth = 100;
    int m_buttonHeight = 20;

    const int STATUS_PER_ROW = 2;
    const int BUTTONS_PER_ROW = 2;

    Player.PlayerEntity m_playerEntity;

    [MenuItem("Debug tools for musky fools/Player Entity Debug")]
    static void Init()
    {
        GetWindow<PlayerEntityEditor>("Player Entity Debug");
    }


    private void OnGUI()
    {
        if (Selection.activeTransform != null && Selection.activeTransform.TryGetComponent<Player.PlayerEntity>(out m_playerEntity))
        {

            EditorGUILayout.LabelField("Status");

            int yPos = 20;
            int xPos = 5;
            //Coloured status boxes
            for (int i = 0; i < System.Enum.GetNames(typeof(Player.PlayerEntityProperties)).Length; ++i)
            {
                if (i % STATUS_PER_ROW == 0 && i != 0)
                {
                    yPos += m_propertyCheckHeight + 3;
                    xPos = 5;
                }
                else if (i != 0)
                {
                    xPos += m_propertyCheckWidth + 1;
                }
                if (m_playerEntity.HasProperty((Player.PlayerEntityProperties)(1 << i)))
                {
                    GUI.color = Color.green;
                    GUI.Box(new Rect(xPos, yPos, m_propertyCheckWidth, m_propertyCheckHeight), System.Enum.GetName(typeof(Player.PlayerEntityProperties), (Player.PlayerEntityProperties)(1 << i)));
                }
                else
                {
                    GUI.color = Color.red;
                    GUI.Box(new Rect(xPos, yPos, m_propertyCheckWidth, m_propertyCheckHeight), System.Enum.GetName(typeof(Player.PlayerEntityProperties), (Player.PlayerEntityProperties)(1 << i)));
                }
            }

            xPos = 5;
            yPos += m_buttonHeight + 8;

            GUI.color = Color.white;
            EditorGUILayout.BeginHorizontal();

            //Toggle buttons
            for (int i = 0; i < System.Enum.GetNames(typeof(Player.PlayerEntityProperties)).Length; ++i)
            {
                if (i % STATUS_PER_ROW == 0 && i != 0)
                {
                    yPos += m_buttonHeight + 3;
                    xPos = 5;
                }
                else if (i != 0)
                {
                    xPos += m_buttonWidth + 1;
                }

                if (GUI.Button(new Rect(xPos, yPos, m_buttonWidth, m_buttonHeight) ,System.Enum.GetName(typeof(Player.PlayerEntityProperties), (Player.PlayerEntityProperties)(1 << i))))
                {
                    if (m_playerEntity.HasProperty((Player.PlayerEntityProperties)(1 << i)))
                        m_playerEntity.RemoveEntityProperty((Player.PlayerEntityProperties)(1 << i));
                    else
                        m_playerEntity.AddEntityProperty((Player.PlayerEntityProperties)(1 << i));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        Repaint();
        GUI.FocusWindow(1);
    }

}
