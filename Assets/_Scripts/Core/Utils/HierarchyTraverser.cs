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
                if(LevelManager.IsEnchantable(t))
                {
                    return LevelManager.GetEnchantable(t);
                }

                t = t.parent;
            }
            return null;
        }
    }
}