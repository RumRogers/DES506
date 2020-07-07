using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;
using GameCore.Spells;
using GameUI.SpellBook;

namespace GameCore.System
{
    public class LevelManager : MonoBehaviour
    {
        static SpellBook s_spellBook = null;
        static int s_playerSpells = 0;
        static Dictionary<Transform, Enchantable> m_transformToEnchantableMap;

        const string SPELLBOOK_TAG = "SpellBook";
        const string ENCHANTABLE_TAG = "Enchantable";

        public static int p_PlayerSpells { get => s_playerSpells; }
        public static Transform p_LastCheckpoint { get; set; }

        public static void UnlockSpell(SpellType spellType)
        {  
            if(s_spellBook == null)
            {
                s_spellBook = GameObject.FindGameObjectWithTag(SPELLBOOK_TAG).GetComponent<SpellBook>();
            }
            Utils.Bits.SetBit(ref s_playerSpells, (int)spellType);
            s_spellBook.UnlockSpell(spellType);
        }

        public static bool IsSpellUnlocked(SpellType spellType)
        {
            return Utils.Bits.GetBit(s_playerSpells, (int)spellType);
        }

        public static bool IsEnchantable(Transform t)
        {
            return m_transformToEnchantableMap.ContainsKey(t);
        }

        public static bool IsEnchantable(GameObject gameObj)
        {
            return IsEnchantable(gameObj.transform);
        }

        public static Enchantable GetEnchantable(Transform t)
        {
            if(IsEnchantable(t))
            {
                return m_transformToEnchantableMap[t];
            }

            return null;
        }

        public static Enchantable GetEnchantable(GameObject gameObj)
        {
            return GetEnchantable(gameObj.transform);
        }

        //public Enchantable 
        private void Init()
        {
            m_transformToEnchantableMap = new Dictionary<Transform, Enchantable>();

            var enchantables = GameObject.FindGameObjectsWithTag(ENCHANTABLE_TAG);

            foreach(var gameObj in enchantables)
            {
                Enchantable enchantable = gameObj.GetComponent<Enchantable>();
                if(enchantable != null)
                {
                    m_transformToEnchantableMap.Add(gameObj.transform, enchantable);
                }
            }
        }

        private void Start()
        {
            Init();
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

