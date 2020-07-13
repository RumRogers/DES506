//#define CHECK_FOR_UNLOCKED_SPELLS

using GameCore.Spells;
using GameCore.System;
using GameCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    [Serializable]
    struct SpellSlotData
    {
        public SpellType spellId;
        public string spellName;
        public Sprite spellSprite;
    }

    public class SpellWheel : Automaton
    {
        static Dictionary<Transform, Enchantable> s_gameTransformToEnchantable = new Dictionary<Transform, Enchantable>();
        private static Enchantable s_targetEnchantable = null;
        const string ARROW_PANEL_TAG = "UI_SpellWheel_Arrow";

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
        Dictionary<Transform, SpellType> m_spellSlotToSpellType = new Dictionary<Transform, SpellType>();
        //Dictionary<Transform, Transform> m_spellActiveObj = new Dictionary<Transform, Transform>();
        [SerializeField]
        Enchantable m_targetEnchantable = null;
        [SerializeField]
        Vector2 m_circleCenter;
        float m_originalBGAlpha;
        Transform m_arrowPanel;
        [SerializeField]
        int m_targetSlotIdx = -1;
        List<int> m_availableSlotIndices = new List<int>();
        [SerializeField]
        List<SpellSlotData> m_spellsOrder = new List<SpellSlotData>();
        Dictionary<SpellType, int> m_spellTypeToTextUI = new Dictionary<SpellType, int>()
        {
            { SpellType.TRANSFORM_SIZE_BIG, 0 },
            { SpellType.TRANSFORM_SIZE_SMALL, 1 }
        };
        // Start is called before the first frame update
        void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_bgImage = transform.GetChild(0).GetChild(1).GetComponent<Image>();
            m_originalBGAlpha = m_bgImage.color.a;
            m_circleCenter = m_rectTransform.rect.center;      
            m_circleRadius = (m_bgImage.rectTransform.rect.width - m_circleThickness / 2f) / 2f;
            m_arrowPanel = GameObject.FindGameObjectWithTag(ARROW_PANEL_TAG).transform;
            //m_firstSlotRotation = 2f * GetAngleStep();
            m_firstSlotRotation = .5f * Mathf.PI;
            InitMap();
            InitCircle();

            SetState(new Idle_SpellWheelState(this));
        }

        protected override void Update()
        {
            base.Update();
            m_targetEnchantable = s_targetEnchantable;
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
                spellSlot.transform.GetChild(0).gameObject.name += "_" + i;

                m_spellSlotToSpellType[spellSlot.transform] = m_spellsOrder[i].spellId;
                currAngle -= angleStep;
            }

            InitSpellSlots();
        }

        public void SetVisible(bool visible, bool showSpells = false)
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
                else
                {
                    t.gameObject.SetActive(false);
                }
            }

            m_arrowPanel.gameObject.SetActive(showSpells);
        }

        public void AimAtFirstAvailableSlot()
        {
            m_targetSlotIdx = 0;
            AimAtSlot(m_availableSlotIndices[m_targetSlotIdx]);
        }

        public void AimAtNextSlot()
        {
            m_targetSlotIdx = (m_targetSlotIdx + 1) % m_availableSlotIndices.Count;
            AimAtSlot(m_availableSlotIndices[m_targetSlotIdx]);
        }

        public void AimAtPrevSlot()
        {            
            if(--m_targetSlotIdx < 0)
            {
                m_targetSlotIdx = m_availableSlotIndices.Count - 1;
            }
            AimAtSlot(m_availableSlotIndices[m_targetSlotIdx]);
        }

        private void AimAtSlot(int slotNumber)
        {
            float angleStep = GetAngleStep();
            float thetaDeg = (slotNumber * angleStep - m_firstSlotRotation) * Mathf.Rad2Deg;            
            m_arrowPanel.transform.rotation = Quaternion.Euler(0f, 0f, -90f - thetaDeg);
            
            for (int i = 0; i < m_spellSlots.Count; i++)
            {

                if (i == slotNumber)
                {
                    m_spellSlots[i].GetChild(0).gameObject.SetActive(true);
                    m_spellSlots[i].GetChild(1).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    m_spellSlots[i].GetChild(0).gameObject.SetActive(false);
                    m_spellSlots[i].GetChild(1).GetChild(1).gameObject.SetActive(false);
                }
            }      
        }

        private float GetAngleStep()
        {
            return (2 * Mathf.PI) / m_spellsAmount;
        }

        private void InitMap()
        {
            const string ENCHANTABLE_TAG = "Enchantable";

            var enchantables = GameObject.FindGameObjectsWithTag(ENCHANTABLE_TAG);

            foreach(var enchantableObj in enchantables)
            {
                s_gameTransformToEnchantable[enchantableObj.transform] = enchantableObj.GetComponent<Enchantable>();
            }
        }

        public static Enchantable GetTargetEnchantable()
        {
            return s_targetEnchantable;
        }
        public static void SetTargetEnchantable(Transform enchantableTransform)
        {
            s_targetEnchantable = enchantableTransform != null ? s_gameTransformToEnchantable[enchantableTransform] : null;
        }

        public void PopulateSpellSlots()
        {
            m_availableSlotIndices.Clear();
            m_spellSlotToSpellType.Clear();
            MagicProfile.MagicState magicState = s_targetEnchantable.GetFullMagicState();
            MagicProfile.CastableSpells castableSpells = s_targetEnchantable.GetCastableSpells();


            for(int i = 0; i < m_spellsOrder.Count; ++i)
            {
                m_spellSlots[i].gameObject.SetActive(false);
                SpellType spellType = m_spellsOrder[i].spellId;

#if CHECK_FOR_UNLOCKED_SPELLS
                if (!LevelManager.IsSpellUnlocked(spellType))
                {
                    continue;
                }
#endif
                switch (spellType)
                {
                    case SpellType.TRANSFORM_SIZE_BIG:
                        if(castableSpells.sizeSpell && magicState.size != SpellState.SPELLED)
                        {
                            m_spellSlots[i].gameObject.SetActive(true);
                            m_availableSlotIndices.Add(i);
                            m_spellSlotToSpellType[m_spellSlots[i]] = SpellType.TRANSFORM_SIZE_BIG;
                        }
                        break;
                    case SpellType.TRANSFORM_SIZE_SMALL:
                        if (castableSpells.sizeSpell && magicState.size != SpellState.COUNTERSPELLED)
                        {
                            m_spellSlots[i].gameObject.SetActive(true);
                            m_availableSlotIndices.Add(i);
                            m_spellSlotToSpellType[m_spellSlots[i]] = SpellType.TRANSFORM_SIZE_SMALL;
                        }
                        break;
                    case SpellType.TRANSFORM_TEMPERATURE_HOT:
                        if (castableSpells.temperatureSpell && magicState.temperature != SpellState.SPELLED)
                        {
                            m_spellSlots[i].gameObject.SetActive(true);
                            m_availableSlotIndices.Add(i);
                            m_spellSlotToSpellType[m_spellSlots[i]] = SpellType.TRANSFORM_TEMPERATURE_HOT;
                        }
                        break;
                    case SpellType.TRANSFORM_TEMPERATURE_COLD:
                        if (castableSpells.temperatureSpell && magicState.temperature != SpellState.COUNTERSPELLED)
                        {
                            m_spellSlots[i].gameObject.SetActive(true);
                            m_availableSlotIndices.Add(i);
                            m_spellSlotToSpellType[m_spellSlots[i]] = SpellType.TRANSFORM_TEMPERATURE_COLD;
                        }
                        break;
                }
            }
        }

        private void InitSpellSlots()
        {
            for(int i = 0; i < m_spellsOrder.Count; ++i)
            {
                int textIdx = (int)m_spellsOrder[i].spellId + 1;
                Image spellIcon = m_spellSlots[i].GetChild(1).GetChild(0).GetComponent<Image>();
                Text spellName = m_spellSlots[i].GetChild(1).GetChild(textIdx).GetComponent<Text>();

                for(int j = 1; j <= 3; ++j)
                {                    
                    if (j != textIdx)
                    {
                        Destroy(m_spellSlots[i].GetChild(1).GetChild(j).gameObject);
                    }
                }

                spellIcon.sprite = m_spellsOrder[i].spellSprite;
                spellName.text = m_spellsOrder[i].spellName;
            }
        }

        public void CastSelectedSpell()
        {
            s_targetEnchantable.CastSpell(new Spell(m_spellSlotToSpellType[m_spellSlots[m_availableSlotIndices[m_targetSlotIdx]]]));            
        }
    }
}

