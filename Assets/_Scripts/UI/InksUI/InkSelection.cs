using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public enum Ink
    {
        RED = 0, GREEN, BLUE
    };

    public class InkSelection : MonoBehaviour
    {
        [SerializeField]
        Sprite m_redInk;
        [SerializeField]
        Sprite m_greenInk;
        [SerializeField]
        Sprite m_blueInk;
        List<Sprite> m_inkSprites;

        Image m_uiSlotInkImage_1;
        Image m_uiSlotInkImage_2;
        Image m_uiSlotInkImage_3;
        List<Image> m_slotInkImages;

        Image m_uiSlotNumberPanel_1;
        Image m_uiSlotNumberPanel_2;
        Image m_uiSlotNumberPanel_3;
        List<Image> m_slotNumberPanels;

        Text m_uiSlotNumberText_1;
        Text m_uiSlotNumberText_2;
        Text m_uiSlotNumberText_3;
        List<Text> m_slotNumberTexts;

        float m_selectedAlpha = 255f;
        float m_unselectedAlpha = 100f;

        public Ink p_SelectedInk { get; private set; }
        // Start is called before the first frame update
        void Start()
        {
            m_inkSprites = new List<Sprite>()
            {
                m_redInk, m_greenInk, m_blueInk
            };

            Transform uiContainer_InkSlots = transform.GetChild(0).GetChild(0);
            m_uiSlotInkImage_1 = uiContainer_InkSlots.GetChild(0).GetChild(0).GetComponent<Image>();
            m_uiSlotInkImage_2 = uiContainer_InkSlots.GetChild(1).GetChild(0).GetComponent<Image>();
            m_uiSlotInkImage_3 = uiContainer_InkSlots.GetChild(2).GetChild(0).GetComponent<Image>();

            m_slotInkImages = new List<Image>()
            {
                m_uiSlotInkImage_1, m_uiSlotInkImage_2, m_uiSlotInkImage_3
            };

            Transform uiContainer_Numbers = transform.GetChild(0).GetChild(1);
            m_uiSlotNumberPanel_1 = uiContainer_Numbers.GetChild(0).GetComponent<Image>();
            m_uiSlotNumberPanel_2 = uiContainer_Numbers.GetChild(1).GetComponent<Image>();
            m_uiSlotNumberPanel_3 = uiContainer_Numbers.GetChild(2).GetComponent<Image>();

            m_slotNumberPanels = new List<Image>()
            {
                m_uiSlotNumberPanel_1, m_uiSlotNumberPanel_2, m_uiSlotNumberPanel_3
            };

            m_uiSlotNumberText_1 = uiContainer_Numbers.GetChild(0).GetChild(0).GetComponent<Text>();
            m_uiSlotNumberText_2 = uiContainer_Numbers.GetChild(1).GetChild(0).GetComponent<Text>();
            m_uiSlotNumberText_3 = uiContainer_Numbers.GetChild(2).GetChild(0).GetComponent<Text>();

            m_slotNumberTexts = new List<Text>()
            {
                m_uiSlotNumberText_1, m_uiSlotNumberText_2, m_uiSlotNumberText_3
            };

            SelectInk(Ink.RED);
        }

        // Update is called once per frame
        void Update()
        {
            ManageInput();
        }

        void ManageInput()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectInk(Ink.RED);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectInk(Ink.GREEN);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectInk(Ink.BLUE);
            }
        }

        void SelectInk(Ink ink)
        {
            p_SelectedInk = ink;
            int inkIdx = (int)ink;
            HighlightInkImage(inkIdx);
            HighlightInkNumber(inkIdx);

            // TODO: add functions to actually CHANGE available spells to the player
        }
        void HighlightInkImage(int n)
        {
            foreach(Image image in m_slotInkImages)
            {
                SetImageAlpha(image, m_unselectedAlpha);
            }

            // 0 -> 0   1 -> 0   2 -> 0
            // 1 -> 1   2 -> 1   0 -> 1
            // 2 -> 2   0 -> 2   1 -> 2
            for(int i = 0; i < m_slotInkImages.Count; ++i)
            {
                m_slotInkImages[i].sprite = m_inkSprites[(n + i) % m_inkSprites.Count];
            }

            SetImageAlpha(m_slotInkImages[0], m_selectedAlpha);
        }

        void HighlightInkNumber(int n)
        {
            for (int i = 0; i < m_slotNumberPanels.Count; ++i)
            {
                if(i == n)
                {
                    SetImageAlpha(m_slotNumberPanels[i], m_selectedAlpha);
                    SetTextAlpha(m_slotNumberTexts[i], m_selectedAlpha);
                }
                else
                {
                    SetImageAlpha(m_slotNumberPanels[i], m_unselectedAlpha);
                    SetTextAlpha(m_slotNumberTexts[i], m_unselectedAlpha);
                }
            }
        }
        void SetImageAlpha(Image sprite, float alpha)
        {            
            Color c = sprite.color;
            c.a = alpha / 255f;
            sprite.color = c;
        }

        void SetTextAlpha(Text text, float alpha)
        {
            Color c = text.color;
            c.a = alpha / 255f;
            text.color = c;
        }


    }
}