using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonIconScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject m_quill;

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_quill.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_quill.SetActive(false);
    }

}