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


        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                m_controller.NextTipButtonPress(false);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                m_controller.NextTipButtonPress(true);
            }

        }

    }

}