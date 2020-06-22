using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{

    public class TipTextScript : MonoBehaviour
    {
        PauseMenuController m_controller;

        private void OnEnable()
        {
            m_controller = GetComponentInParent<PauseMenuController>(); //Not really what I want, will fix later

            m_controller.UpdateTipText();
        }
    }

}