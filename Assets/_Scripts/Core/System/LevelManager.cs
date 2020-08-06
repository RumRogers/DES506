using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;
using GameCore.Spells;
using GameUI.SpellBook;
using GameCore.Utils;
using GameUI;

namespace GameCore.System
{
    public class LevelManager
    {
        static SpellBook s_spellBook = null;
        static int s_playerSpells = 0;
        static Dictionary<Transform, Enchantable> m_transformToEnchantableMap;
        static Dictionary<Enchantable, Renderer> m_enchantableToRendererMap;
        static List<GameObject> s_enchantableParticles = new List<GameObject>();
        static LevelManager s_instance = null;
        public static SpellWheel s_spellWheel = null;
        public static ItemSelector s_itemSelector = null;

        const string SPELLBOOK_TAG = "SpellBook";
        const string ENCHANTABLE_TAG = "Enchantable";
        const string UI_ITEM_SELECTOR_TAG = "UI_ItemSelector";
        

        public static int p_PlayerSpells { get => s_playerSpells; }
        public static Transform p_LastCheckpoint { get; set; }

        public static LevelManager Instance 
        { 
            get 
            {
                if(s_instance == null)
                {
                    s_instance = new LevelManager();
                }

                return s_instance;
            }
        }

        private LevelManager()
        {
            Init();
        }

        public static void UnlockSpell(SpellType spellType, bool showSpellBook = true)
        {  
            if(s_spellBook == null)
            {
                s_spellBook = GameObject.FindGameObjectWithTag(SPELLBOOK_TAG).GetComponent<SpellBook>();
            }
            Utils.Bits.SetBit(ref s_playerSpells, (int)spellType);
            s_spellBook.UnlockSpell(spellType);
            if(spellType == SpellType.TRANSFORM_RESET)
            {
                s_itemSelector.SetSlotsVisible(true);
            }

            if(showSpellBook)
            {
                s_spellBook.SetState(new Active_SpellBookState(s_spellBook));
                s_spellBook.SetSelectedSpell(spellType);
            }
        }

        public static bool IsSpellUnlocked(SpellType spellType)
        {
            return Utils.Bits.GetBit(s_playerSpells, (int)spellType);
        }

        public bool IsEnchantable(Transform t)
        {
            return m_transformToEnchantableMap.ContainsKey(t);
        }

        public bool IsEnchantable(GameObject gameObj)
        {
            return IsEnchantable(gameObj.transform);
        }

        public Enchantable GetEnchantable(Transform t)
        {
            if(m_transformToEnchantableMap == null)
            {
                Init();
            }

            if(IsEnchantable(t))
            {
                return m_transformToEnchantableMap[t];
            }

            return null;
        }
        public Renderer GetRenderer(Enchantable enchantable)
        {
            if(m_enchantableToRendererMap.ContainsKey(enchantable))
            {
                return m_enchantableToRendererMap[enchantable];
            }

            return null;
        }

        public Enchantable GetEnchantable(GameObject gameObj)
        {
            return GetEnchantable(gameObj.transform);
        }

        //public Enchantable 
        private void Init()
        {

            s_itemSelector = GameObject.FindGameObjectWithTag(UI_ITEM_SELECTOR_TAG).GetComponent<ItemSelector>();

            m_transformToEnchantableMap = new Dictionary<Transform, Enchantable>();
            m_enchantableToRendererMap = new Dictionary<Enchantable, Renderer>();

            var enchantables = GameObject.FindGameObjectsWithTag(ENCHANTABLE_TAG);

            foreach(var gameObj in enchantables)
            {
                Enchantable enchantable = gameObj.GetComponent<Enchantable>();
                if(enchantable != null)
                {
                    m_transformToEnchantableMap.Add(gameObj.transform, enchantable);

                    Renderer renderer = HierarchyTraverser.RetrieveRendererComponent(enchantable);
                    if(renderer != null)
                    {
                        m_enchantableToRendererMap.Add(enchantable, renderer);
                    }
                }
            }
        }

        public static void AddEnchantableParticles(GameObject particles)
        {

            s_enchantableParticles.Add(particles);
        }

        public static void ShowEnchantableParticles(bool show)
        {
            foreach(var particle in s_enchantableParticles)
            {
                particle.SetActive(show);
            }
        }

        public static void ClearEnchantableParticles()
        {
            s_enchantableParticles.Clear();
        }
        public static bool HasPlayerUnlockedAtLeastASpell()
        {
            return s_playerSpells != 0;
        }

        public static void ForceSpellWheelClose()
        {
            s_spellWheel.SetState(new Idle_SpellWheelState(s_spellWheel));
        }

        // All of this became obsolete when we changed core mechanic
        /*
        public static Dictionary<string, List<MutableEntity>> s_mapSubjectToMutable = new Dictionary<string, List<MutableEntity>>();
        
        private void Start()
        {
            PrepareMutables();
        }

        static void PrepareMutables()
        {
            // I hate to do this, but GameObject.FindGameObjectsWithTag will throw an exception if the tag does not exist
            // or if a null string is passed as argument.
            try
            {
                var sceneMutables = GameObject.FindGameObjectsWithTag(MutableEntity.s_MutableTag);
                foreach (var mutableGameObj in sceneMutables)
                {
                    var mutableScript = mutableGameObj.GetComponent<MutableEntity>();
                    string subject = null;
                    if (mutableScript != null)
                    {
                        subject = mutableScript.p_ReactsToSubject;
                        if(subject == null)
                        {
                            continue;
                        }

                        subject = subject.ToLower();
                        if (!s_mapSubjectToMutable.ContainsKey(subject))
                        {
                            s_mapSubjectToMutable.Add(subject, new List<MutableEntity>());
                        }
                    }
                    s_mapSubjectToMutable[subject].Add(mutableScript);
                }
            }
            catch (UnityException ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        public static List<MutableEntity> GetMutablesFromSubject(string subject)
        {
            subject = subject.ToLower();

            if(s_mapSubjectToMutable.ContainsKey(subject))
            {
                return s_mapSubjectToMutable[subject];
            }

            return null;
        }*/
    }
}

