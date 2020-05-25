using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Tiles
{
    public class GroundTile : AbstractTile
    {
        protected override void SetType()
        {
            m_type = TileType.GROUND;
        }

        protected override void ApplyEffect() { }
    }
}