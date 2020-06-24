using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class which handles navigating the menu, 
/// Not very high-tech stuff but it'll do.
/// 
/// Feel free to add more menus as necessary, just use the index as the param for DisplayMenu() 
/// on the button you want's OnClick(). If you don't want to hide the previous menu for
/// whatever reason you can call DisplayMenuOnly() instead.
/// </summary>

public class MenuButtonScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_menus;

    [SerializeField]
    Text playButtonText; //Used to determine whether the play button should say "begin" or "continue"

   //Array index (set in editor):
   //   0 -- Splash Screen
   //   1 -- Main Menu
   //   2 -- Options Menu
    
    /// <summary>
    /// Hides all other menus and displays the menu at the index.
    /// </summary>
    /// <param name="index"></param>
    public void DisplayMenu(int index)
    {
        HideAllMenus();

        m_menus[index].SetActive(true);
    }

    /// <summary>
    /// Displays the menu at index without hiding other menus.
    /// </summary>
    /// <param name="index"></param>
    public void DisplayMenuOnly(int index)
    {
        m_menus[index].SetActive(true);
    }

    void HideAllMenus()
    {
        foreach(GameObject g in m_menus)
        {
            g.SetActive(false);
        }
    }

    private void Awake()
    {
        //Just in case the others are left enabled in the editor, hard set only the splash screen active on start
        DisplayMenu(0);

        if (PlayerPrefs.HasKey("LevelReached"))
        {
            playButtonText.text = "Continue"; //Probably don't hard code these
        }
        else
        {
            playButtonText.text = "Begin";
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
