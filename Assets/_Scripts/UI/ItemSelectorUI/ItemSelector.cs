using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class ItemSelector : MonoBehaviour
    {        

        static KeyCode m_keySelectQuill = KeyCode.Alpha1;
        static KeyCode m_keySelectEraser = KeyCode.Alpha2;
        Dictionary<KeyCode, PlayerEquipableItems> m_keyToItemMap = new Dictionary<KeyCode, PlayerEquipableItems>()
        {
            { m_keySelectQuill, PlayerEquipableItems.SPELL_QUILL }, { m_keySelectEraser, PlayerEquipableItems.ERASER }
        };
        [SerializeField]
        List<Transform> m_selectedIcons = new List<Transform>();
        [SerializeField]
        List<Transform> m_unselectedIcons = new List<Transform>();
        [SerializeField]
        List<Image> m_numberPanels = new List<Image>();
        [SerializeField]
        List<Text> m_numberText = new List<Text>();
        [SerializeField]
        Color m_numberSelectedColor;
        [SerializeField]
        Color m_numberUnselectedColor;

        PlayerEntity m_playerEntity = null;

        // Start is called before the first frame update
        void Start()
        {
            m_playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
            SelectItem(PlayerEquipableItems.SPELL_QUILL);
        }

        // Update is called once per frame
        void Update()
        {
            ManageInput();
        }

        void ManageInput()
        {
            if (Input.GetKeyDown(m_keySelectQuill))
            {
                SelectItem(m_keyToItemMap[m_keySelectQuill]);
            }
            else if (Input.GetKeyDown(m_keySelectEraser))
            {
                SelectItem(m_keyToItemMap[m_keySelectEraser]);
            }            
        }

        void SelectItem(PlayerEquipableItems item)
        {
            m_playerEntity.EquipedItem = item;
            int itemIdx = (int)item;

            for(int i = (int)PlayerEquipableItems.SPELL_QUILL; i <= (int)PlayerEquipableItems.ERASER; ++i)
            {
                bool active = i == itemIdx;
                Color color = active ? m_numberSelectedColor : m_numberUnselectedColor;
                m_selectedIcons[i].gameObject.SetActive(active);
                m_unselectedIcons[i].gameObject.SetActive(!active);
                m_numberPanels[i].color = m_numberText[i].color = color;
            }
        }
    }
}