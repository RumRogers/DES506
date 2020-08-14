using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAudio
{
    [DisallowMultipleComponent]
    public class Enchantable_FMODAdapter : MonoBehaviour
    {        
        private GameObject m_shrinkSpellSFX;
        private GameObject m_enlargeSpellSFX;
        private GameObject m_infernoSpellSFX;
        private GameObject m_coldSpellSFX;

        private Dictionary<SpellType, GameObject> m_spellTypesToSFX;

        public GameObject p_ShrinkSpellSFX { get => m_spellTypesToSFX[SpellType.TRANSFORM_SIZE_SMALL]; }
        public GameObject p_EnlargeSpellSFX { get => m_spellTypesToSFX[SpellType.TRANSFORM_SIZE_BIG]; }
        public GameObject p_InfernoSpellSFX { get => m_spellTypesToSFX[SpellType.TRANSFORM_TEMPERATURE_HOT]; }
        public GameObject p_ColdSpellSFX { get => m_spellTypesToSFX[SpellType.TRANSFORM_TEMPERATURE_COLD]; }

        private void Start()
        {
            m_enlargeSpellSFX = Resources.Load<GameObject>("Prefabs/AudioPrefabs/Enlarge_SFX");
            m_shrinkSpellSFX = Resources.Load<GameObject>("Prefabs/AudioPrefabs/Shrink_SFX");
            m_coldSpellSFX = Resources.Load<GameObject>("Prefabs/AudioPrefabs/Freeze_SFX");

            m_spellTypesToSFX = new Dictionary<SpellType, GameObject>()
            {
                { SpellType.TRANSFORM_SIZE_SMALL, m_shrinkSpellSFX },
                { SpellType.TRANSFORM_SIZE_BIG, m_enlargeSpellSFX },
                { SpellType.TRANSFORM_TEMPERATURE_HOT, m_infernoSpellSFX },
                { SpellType.TRANSFORM_TEMPERATURE_COLD, m_coldSpellSFX }
            };
        }
    }
}