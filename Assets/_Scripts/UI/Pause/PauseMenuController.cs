using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region tips classes
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

    Tips m_tipObj;

    int m_currTipNum;
    int m_tipsAmount;

    [SerializeField]
    TextAsset m_tipsJSON;

    [SerializeField]
    TMPro.TextMeshProUGUI m_tipText;
    [SerializeField]
    TMPro.TextMeshProUGUI m_tipNumText;

    //Index:
    //0 -- Main Pause Menu


    private void Awake() //On opening the menu, display a random tip
    {
        m_tipObj = JsonUtility.FromJson<Tips>(m_tipsJSON.text);

        m_tipsAmount = m_tipObj.tips.Length;
        Debug.Log(m_tipsAmount);
    }

    public void UpdateTipText()
    {
        System.Random rnd = new System.Random();

        m_currTipNum = rnd.Next(m_tipsAmount);
        Debug.Log(m_currTipNum);
        SetTipText(m_currTipNum);
    }

    public void SetTipText(int index)
    {
        if(m_tipsAmount >= index - 1)
        {
            m_tipText.text = m_tipObj.tips[index].text;
        }
        else
        {
            Debug.Log("That tip number don't exist");
        }
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

