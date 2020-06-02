//#define DEBUG_TILE_NEIGHBORS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;

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
        [SerializeField]
        // The GameObject that is currently contained within this tile
        protected GameObject m_currentGameObject;
        protected Dictionary<CardinalPoint, AbstractTile> m_neighborTiles;
        public GameObject p_ContainedGameObject { get => m_currentGameObject; }

#if DEBUG_TILE_NEIGHBORS
        [SerializeField]
        private AbstractTile m_neighborN;
        [SerializeField]
        private AbstractTile m_neighborS;
        [SerializeField]
        private AbstractTile m_neighborW;
        [SerializeField]
        private AbstractTile m_neighborE;
#endif

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
#if DEBUG_TILE_NEIGHBORS           
            switch (where)
            {
                case CardinalPoint.NORTH:
                    m_neighborN = neighbor;
                    break;
                case CardinalPoint.SOUTH:
                    m_neighborS = neighbor;
                    break;
                case CardinalPoint.WEST:
                    m_neighborW = neighbor;
                    break;
                case CardinalPoint.EAST:
                    m_neighborE = neighbor;
                    break;
            }
#endif
        }

        public AbstractTile GetNeighborAtCardinalPoint(CardinalPoint where)
        {
            return m_neighborTiles[where];
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
            // Do not update the current object if the player enters the tile but the tile contains something else already
            if(m_currentGameObject != null && other.CompareTag("Player"))
            {
                return;
            }
            m_currentGameObject = other.gameObject;

            if (other.CompareTag("Movable"))
            {
                ManageRuleChange();
            }
            //Debug.Log($"Object {other.name} with tag {other.tag} just entered {this}.");
            else
            {
                ApplyEffect();
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            // This might not be true when having a TriggerEnter and TriggerExit at the same time
            // (think of boxes stacked horizontally and going through this tile)
            if (other.gameObject == m_currentGameObject)
            {
                if(other.CompareTag("Movable"))
                {
                    ManageRuleChange(Rule.ApplicationMode.UNDO);
                }
                m_currentGameObject = null;
                //Debug.Log($"Object {other.name} with tag {other.tag} just left {this}.");
            }
        }

        private AbstractTile GetFurthestTileContainingRuleBoxAt(CardinalPoint direction)
        {              
            AbstractTile ptr = this;

            // Proceed examining tiles in the specified direction until encountering either an empty tile
            // or a tile containing something that is not a RuleBox.
            do
            {
                ptr = ptr.m_neighborTiles[direction];
            }
            while (ptr.m_currentGameObject != null && ptr.m_currentGameObject.CompareTag("Movable"));

            return ptr;
        }

        private void ManageRuleChange(Rule.ApplicationMode mode = Rule.ApplicationMode.APPLY)
        {
            RuleBox ruleHSubject = null, ruleHVerb = null, ruleHObject = null;
            RuleBox ruleVSubject = null, ruleVVerb = null, ruleVObject = null;

            var thisRuleBox = m_currentGameObject.GetComponent<RuleBox>();

            // There's some terrible code duplication here for X alignment and Z alignment...
            // Sorry about that, I will refactor it to make it more elegant in the near future.
            switch(thisRuleBox.p_ChunkType)
            {
                case RuleChunk.ChunkType.SUBJECT:
                    {
                        {
                            AbstractTile eastTile = GetNeighborAtCardinalPoint(CardinalPoint.EAST);
                            // Keep checking only if the eastern tile exists, and contains a rule box...
                            if (eastTile != null && eastTile.ContainsRuleBox())
                            {
                                AbstractTile eastEastTile = eastTile.GetNeighborAtCardinalPoint(CardinalPoint.EAST);
                                // Same for the tile that is two tiles away to the east from this one
                                if (eastEastTile != null && eastEastTile.ContainsRuleBox())
                                {
                                    ruleHSubject = thisRuleBox;
                                    ruleHVerb = eastTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                    ruleHObject = eastEastTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                }
                            }
                        }

                        {
                            AbstractTile southTile = GetNeighborAtCardinalPoint(CardinalPoint.SOUTH);
                            // Keep checking only if the southern tile exists, and contains a rule box...
                            if (southTile != null && southTile.ContainsRuleBox())
                            {
                                AbstractTile southSouthTile = southTile.GetNeighborAtCardinalPoint(CardinalPoint.SOUTH);
                                // Same for the tile that is two tiles away to the east from this one
                                if (southSouthTile != null && southSouthTile.ContainsRuleBox())
                                {
                                    ruleVSubject = thisRuleBox;
                                    ruleVVerb = southTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                    ruleVObject = southSouthTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                }
                            }
                        }
                    }
                    break;
                case RuleChunk.ChunkType.VERB:
                    {
                        {
                            AbstractTile westTile = GetNeighborAtCardinalPoint(CardinalPoint.WEST);
                            AbstractTile eastTile = GetNeighborAtCardinalPoint(CardinalPoint.EAST);

                            if (westTile != null && eastTile != null && westTile.ContainsRuleBox() && eastTile.ContainsRuleBox())
                            {
                                ruleHSubject = westTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                ruleHVerb = thisRuleBox;
                                ruleHObject = eastTile.p_ContainedGameObject.GetComponent<RuleBox>();
                            }
                        }

                        {
                            AbstractTile northTile = GetNeighborAtCardinalPoint(CardinalPoint.NORTH);
                            AbstractTile southTile = GetNeighborAtCardinalPoint(CardinalPoint.SOUTH);

                            if (northTile != null && southTile != null && northTile.ContainsRuleBox() && southTile.ContainsRuleBox())
                            {
                                ruleVSubject = northTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                ruleVVerb = thisRuleBox;
                                ruleVObject = southTile.p_ContainedGameObject.GetComponent<RuleBox>();
                            }
                        }
                    }
                    break;
                case RuleChunk.ChunkType.OBJECT:
                    {
                        {
                            AbstractTile westTile = GetNeighborAtCardinalPoint(CardinalPoint.WEST);

                            if (westTile != null && westTile.ContainsRuleBox())
                            {
                                AbstractTile westWestTile = westTile.GetNeighborAtCardinalPoint(CardinalPoint.WEST);
                                if (westWestTile != null && westWestTile.ContainsRuleBox())
                                {
                                    ruleHSubject = westWestTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                    ruleHVerb = westTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                    ruleHObject = thisRuleBox;
                                }
                            }
                        }

                        {
                            AbstractTile northTile = GetNeighborAtCardinalPoint(CardinalPoint.NORTH);

                            if (northTile != null && northTile.ContainsRuleBox())
                            {
                                AbstractTile northNorthTile = northTile.GetNeighborAtCardinalPoint(CardinalPoint.NORTH);
                                if (northNorthTile != null && northNorthTile.ContainsRuleBox())
                                {
                                    ruleVSubject = northNorthTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                    ruleVVerb = northTile.p_ContainedGameObject.GetComponent<RuleBox>();
                                    ruleVObject = thisRuleBox;
                                }
                            }
                        }
                        
                    }
                    break;
                default:
                    throw new UnityException($"Invalid type of rule box: {thisRuleBox}");
            }
            
            if(ruleHSubject != null && ruleHVerb != null && ruleHObject != null)
            {
                if (Rule.IsValidRule(ruleHSubject.p_RuleChunk, ruleHVerb.p_RuleChunk, ruleHObject.p_RuleChunk))
                {
                    Rule rule = new Rule(ruleHSubject.p_RuleChunk, ruleHVerb.p_RuleChunk, ruleHObject.p_RuleChunk);
                    rule.Apply(mode);
                }
                else
                {
                    Debug.Log("Rule was not valid...");
                }
            }
            if (ruleVSubject != null && ruleVVerb != null && ruleVObject != null)
            {
                if (Rule.IsValidRule(ruleVSubject.p_RuleChunk, ruleVVerb.p_RuleChunk, ruleVObject.p_RuleChunk))
                {
                    Rule rule = new Rule(ruleVSubject.p_RuleChunk, ruleVVerb.p_RuleChunk, ruleVObject.p_RuleChunk);
                    rule.Apply(mode);
                }
                else
                {
                    Debug.Log("Rule was not valid...");
                }
            }
        }

        public bool ContainsRuleBox()
        {
            return m_currentGameObject != null && m_currentGameObject.CompareTag("Movable");
        }
        public override string ToString()
        {
            return $"Tile ({transform.position.x},{transform.position.z}) of type {m_type})";
        }

        #region EDITOR_DEBUGGING

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

        private void OnDrawGizmos()
        {
            ForceAlignment();
        }

        #endregion

        
    }
}