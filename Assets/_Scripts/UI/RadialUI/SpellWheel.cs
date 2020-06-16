using GameCore.Spells;
using GameCore.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class SpellWheel : Automaton
    {
        const string ARROW_PANEL_TAG = "UI_SpellWheel_Arrow";
        const string SPELL_SLOT_EMPTY_TAG = "UI_SpellSlot_Empty";
        const string SPELL_SLOT_FULL_TAG = "UI_SpellSlot_Full";

        RectTransform m_rectTransform;
        Image m_bgImage;
        float m_firstSlotRotation;
        [SerializeField]
        float m_circleRadius;
        [SerializeField]
        float m_circleThickness;
        [SerializeField]
        GameObject m_prefabEmptySpellSlot;
        [SerializeField]
        GameObject m_prefabSpellSlot;
        [SerializeField]
        int m_spellsAmount = 10;        
        [SerializeField]
        List<Transform> m_emptySlots = new List<Transform>();
        List<Transform> m_spellSlots = new List<Transform>();
        [SerializeField]
        Enchantable m_targetEnchantable = null;
        [SerializeField]
        Vector2 m_circleCenter;
        float m_originalBGAlpha;
        Transform m_arrowPanel;
        int m_targetSlot = 0;
        public int p_TargetSlot { get => m_targetSlot; }

        // Start is called before the first frame update
        void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_bgImage = GetComponent<Image>();
            m_originalBGAlpha = m_bgImage.color.a;
            m_circleCenter = m_rectTransform.rect.center;      
            m_circleRadius = (m_bgImage.rectTransform.rect.width - m_circleThickness / 2f) / 2f;
            m_arrowPanel = GameObject.FindGameObjectWithTag(ARROW_PANEL_TAG).transform;
            m_firstSlotRotation = 2f * GetAngleStep();
            InitCircle();

            SetState(new Idle_SpellWheelState(this));
        }

        void InitCircle()
        {
            foreach (var emptySlot in m_emptySlots)
            {
                Destroy(emptySlot.gameObject);
            }
            m_emptySlots.Clear();

            foreach (var spellSlot in m_spellSlots)
            {
                Destroy(spellSlot.gameObject);
            }
            m_spellSlots.Clear();

            float angleStep = GetAngleStep();
            float currAngle = m_firstSlotRotation;

            for (int i = 0; i < m_spellsAmount; ++i)
            {
                var emptySpellSlot = Instantiate(m_prefabEmptySpellSlot, m_rectTransform, false);
                float slotX = m_circleRadius * Mathf.Cos(currAngle);
                float slotY = m_circleRadius * Mathf.Sin(currAngle);
                emptySpellSlot.transform.localPosition = new Vector3(slotX + m_circleCenter.x, slotY + m_circleCenter.y, 0f);
                m_emptySlots.Add(emptySpellSlot.transform);

                var spellSlot = Instantiate(m_prefabSpellSlot, m_rectTransform, false);
                spellSlot.transform.localPosition = emptySpellSlot.transform.localPosition;
                m_spellSlots.Add(spellSlot.transform);
                spellSlot.SetActive(false);

                currAngle -= angleStep;
            }
        }

        public void SetVisible(bool visible)
        {
            Color c = m_bgImage.color;
            c.a = visible ? m_originalBGAlpha : 0f;
            m_bgImage.color = c;

            foreach(Transform t in transform)
            {
                if(!t.CompareTag(SPELL_SLOT_FULL_TAG))
                {
                    t.gameObject.SetActive(visible);
                }
            }
        }

        public void AimAtSlot(int slotNumber)
        {
            slotNumber = slotNumber % m_spellsAmount;
            float angleStep = GetAngleStep();
            float thetaDeg = (slotNumber * angleStep - m_firstSlotRotation) * Mathf.Rad2Deg;
            m_targetSlot = slotNumber;
            m_arrowPanel.transform.rotation = Quaternion.Euler(0f, 0f, -90f - thetaDeg);
        }

        private float GetAngleStep()
        {
            return (2 * Mathf.PI) / m_spellsAmount;
        }
        private void OnGUI()
        {
            if(GUI.Button(new Rect(10, 10, 150, 50), "Init SpellWheel"))
            {
                InitCircle();
            }
        }
    }
}

