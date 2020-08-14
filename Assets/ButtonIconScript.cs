using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonIconScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject m_quill;
    [SerializeField]
    Text m_text;


    public void OnPointerEnter(PointerEventData eventData)
    {
        m_quill.SetActive(true);
        m_text.color = new Color32(50, 50, 50, 255);
       // m_text.color = Color.blue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_quill.SetActive(false);
        m_text.color = new Color(1, 1, 1, 1);
    }

}