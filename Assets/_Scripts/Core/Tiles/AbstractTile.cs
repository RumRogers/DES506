using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Tiles
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class AbstractTile : MonoBehaviour
    {
        public enum TileType
        {
            GROUND, WATER
        }

        public enum CardinalPoint
        {
            NORTH, SOUTH, WEST, EAST
        }

        // Implicitly static in C#
        public const float s_TILE_SIZE = 1.0f;

        [SerializeField]
        // Only needed for coherency in the editor, so it's visually obvious what type the tile is
        protected TileType m_type;
        private BoxCollider m_triggerBoxCollider;
        // The GameObject that is currently contained within this tile
        protected GameObject m_currentGameObject;
        protected Dictionary<CardinalPoint, AbstractTile> m_neighborTiles;

        private void Start()
        {
            InitTile();
        }

        private void InitTile()
        {
            SetType();
            ForceAlignment();
            InitNeighborTiles();

            TileMapper.AddTileToXMap(this);
            TileMapper.AddTileToZMap(this);
        }

        /// <summary>
        /// Helper function for designers and artists: aligns tiles that aren't perfectly aligned
        /// to the nearest integer for each position coordinate.
        /// </summary>
        private void ForceAlignment()
        {
            Vector3 truncatedPosition = transform.position;
            truncatedPosition.x = Mathf.RoundToInt(truncatedPosition.x);
            truncatedPosition.y = Mathf.RoundToInt(truncatedPosition.y);
            truncatedPosition.z = Mathf.RoundToInt(truncatedPosition.z);
            transform.position = truncatedPosition;
        }

        private void InitNeighborTiles()
        {
            m_neighborTiles = new Dictionary<CardinalPoint, AbstractTile>(4)
            {
                { CardinalPoint.NORTH, null },
                { CardinalPoint.SOUTH, null },
                { CardinalPoint.WEST, null },
                { CardinalPoint.EAST, null }
            };
        }

        public void AddNeighbor(AbstractTile neighbor, CardinalPoint where)
        {
            m_neighborTiles[where] = neighbor;              
        }

        /// <summary>
        /// Each concrete type will have to set its own type by implementing this.
        /// It is useless for the logic but will help visually during level creation
        /// </summary>
        protected abstract void SetType();

        /// <summary>
        /// This is the real deal of the tile system, as each concrete tile will need to specify
        /// what happens when a game entity steps on it.
        /// Please note that "no effect" is just an implementation with an empty method body.
        /// </summary>
        protected abstract void ApplyEffect();

        protected void OnTriggerEnter(Collider other)
        {
            m_currentGameObject = other.gameObject;
            ApplyEffect();
        }

        protected void OnTriggerExit(Collider other)
        {
            // This might not be true when having a TriggerEnter and TriggerExit at the same time
            // (think of boxes stacked horizontally and going through this tile)
            if (other.gameObject == m_currentGameObject)
            {
                m_currentGameObject = null;
            }
        }

        public void DebugDrawNormal(Color c)
        {
            Debug.DrawLine(transform.position, transform.position + transform.up, c);
        }
        private void OnDrawGizmosSelected()
        {
            if (m_neighborTiles == null)
            {
                return;
            }

            List<Color> colors = new List<Color>(){ Color.red, Color.green, Color.blue, Color.yellow };
            int idx = 0;

            foreach(var entry in m_neighborTiles)
            {
                if(entry.Value != null)
                {
                    entry.Value.DebugDrawNormal(colors[idx]);
                }
                ++idx;
            }            
        }
    }
}