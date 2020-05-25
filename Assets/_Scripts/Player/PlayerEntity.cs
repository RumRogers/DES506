using System.Collections;
using System.Collections.Generic;
using GameCore.Rules;
using UnityEngine;

namespace Player
{
    public enum PlayerEntityProperties
    {
        MOVEABLE    = 1 << 1,
        PLAYABLE    = 2 << 1,
        DYING       = 3 << 1,
        CAN_JUMP    = 4 << 1,
        JUMP_NORMAL = 5 << 1,
        JUMP_HIGH   = 6 << 1,
        CAN_DROWN   = 7 << 1
    }

    public class PlayerEntity : GameCore.Rules.IMutableEntity
    {
        PlayerEntityProperties m_entityProperties;

        public void Can(ref string ruleObject)
        {
            Debug.LogError("IMPLEMENT PLAYER CAN (Josh hasn't impmented this yet)");
        }

        public void Has(ref string ruleObject)
        {
            Debug.LogError("IMPLEMENT PLAYER HAS (Josh hasn't impmented this yet)");
        }

        public void Is(ref string ruleObject)
        {
            Debug.LogError("IMPLEMENT PLAYER IS (Josh hasn't impmented this yet)");
        }

        //CHANGING / CHECKING PROPERTIES

        public void ClearEntityProperties()
        {
            m_entityProperties = 0;
        }

        /// <summary>
        /// Uses bit mask logic. Adds entity property to the player entity, Does not remove other properties when a new one is assigned. Player can have more than one property.
        /// </summary>
        /// <param name="property"></param>
        public void AddEntityProperty(PlayerEntityProperties property)
        {
            m_entityProperties |= property;
        }

        /// <summary>
        /// Uses bit mask logic. Removes entity property to the player entity, Does not modify other properties when one is removed. Player can have more than one property.
        /// </summary>
        /// <param name="property"></param>
        public void RemoveEntityProperty(PlayerEntityProperties property)
        {
            m_entityProperties &= ~property;
        }

        /// <summary>
        /// Uses bit mask logic. Removes to remove and adds to add, does not modify other properties. Player can have more than one property.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <param name="toAdd"></param>
        public void ReplaceEntityProperty(PlayerEntityProperties toRemove, PlayerEntityProperties toAdd)
        {
            m_entityProperties &= ~toRemove;
            m_entityProperties |= toAdd;
        }

        public bool HasProperty(PlayerEntityProperties property)
        {
            return (m_entityProperties & property) == property;
        }
    }
}
