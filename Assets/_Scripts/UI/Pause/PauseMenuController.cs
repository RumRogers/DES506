using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region tips
[System.Serializable]
public class Tip
{
    public string text;
}

[System.Serializable]
public class Tips
{
    public Tip[] tips;
}
#endregion

public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_menus;

    Tips tipObj;

    [SerializeField]
    TextAsset tipsJSON;

    //Index:
    //0 -- Main Pause Menu

    private void Start()
    {
        tipObj = JsonUtility.FromJson<Tips>(tipsJSON.text);
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

