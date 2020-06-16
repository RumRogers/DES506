using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    bool m_isPlaying = true;

    GameObject[] m_menus;
    //Index:
    //0 -- Main Pause Menu

    public void TempTogglePause()
    {
        m_isPlaying = !m_isPlaying;

        Time.timeScale = m_isPlaying ? 0 : 1;
    }

    private void Awake()
    {
        HideAllMenus();
        m_isPlaying = true;
    }

#region Sub-Menu Handling

    public void DisplayMenu(int index)
    {
        HideAllMenus();

        m_menus[index].SetActive(true);
    }

    public void DisplayMenuOnly(int index)
    {
        m_menus[index].SetActive(true);
    }

    void HideAllMenus()
    {
        foreach (GameObject g in m_menus)
        {
            g.SetActive(false);
        }
    }

#endregion
}
