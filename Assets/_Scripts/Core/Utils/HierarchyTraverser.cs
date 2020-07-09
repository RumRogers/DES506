using GameCore.Spells;
using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Utils
{
    public class HierarchyTraverser
    {
        public static Enchantable RetrieveEnchantableComponent(Transform transform)
        {
            Transform t = transform;

            while(t != null)
            {
                if(LevelManager.Instance.IsEnchantable(t))
                {
                    return LevelManager.Instance.GetEnchantable(t);
                }

                t = t.parent;
            }
            return null;
        }

        public static Renderer RetrieveRendererComponent(Enchantable enchantable)
        {
            return enchantable.transform.GetComponentInChildren<Renderer>();
        }
    }
}