﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;

namespace GameCore.System
{
    public class LevelManager : MonoBehaviour
    {
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
        }
    }
}
