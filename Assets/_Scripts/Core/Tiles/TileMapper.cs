using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Tiles
{
    public static class TileMapper
    {
        private static Dictionary<float, List<AbstractTile>> m_mapXtoTile = new Dictionary<float, List<AbstractTile>>();
        private static Dictionary<float, List<AbstractTile>> m_mapZtoTile = new Dictionary<float, List<AbstractTile>>();

        public static void AddTileToXMap(AbstractTile tile)
        {
            float xPos = tile.transform.position.x;

            // Has this X key been added before?
            if(!m_mapXtoTile.ContainsKey(xPos))
            {
                m_mapXtoTile[xPos] = new List<AbstractTile>();
            }
           
            m_mapXtoTile[xPos].Add(tile);
        }

        public static void AddTileToZMap(AbstractTile tile)
        {
            float zPos = tile.transform.position.z;

            // Has this Y key been added before?
            if (!m_mapZtoTile.ContainsKey(zPos))
            {
                m_mapZtoTile[zPos] = new List<AbstractTile>();
            }

            m_mapZtoTile[zPos].Add(tile);
        }

        public static void SetAllNeighbors()
        {
            foreach(var entry in m_mapXtoTile)
            {
                foreach(var tile in entry.Value)
                {
                    AbstractTile northernNeighbor = GetNeighborAtCardinalPoint(tile, AbstractTile.CardinalPoint.NORTH);
                    AbstractTile southernNeighbor = GetNeighborAtCardinalPoint(tile, AbstractTile.CardinalPoint.SOUTH);

                    if(northernNeighbor != null)
                    {
                        tile.AddNeighbor(northernNeighbor, AbstractTile.CardinalPoint.NORTH);
                        northernNeighbor.AddNeighbor(tile, AbstractTile.CardinalPoint.SOUTH);
                    }
                    if(southernNeighbor != null)
                    {
                        tile.AddNeighbor(southernNeighbor, AbstractTile.CardinalPoint.SOUTH);
                        southernNeighbor.AddNeighbor(tile, AbstractTile.CardinalPoint.NORTH);
                    }                    
                }
            }

            foreach (var entry in m_mapZtoTile)
            {
                foreach (var tile in entry.Value)
                {
                    AbstractTile westernNeighbor = GetNeighborAtCardinalPoint(tile, AbstractTile.CardinalPoint.WEST);
                    AbstractTile easternNeighbor = GetNeighborAtCardinalPoint(tile, AbstractTile.CardinalPoint.EAST);

                    if(westernNeighbor != null)
                    {
                        tile.AddNeighbor(westernNeighbor, AbstractTile.CardinalPoint.WEST);
                        westernNeighbor.AddNeighbor(tile, AbstractTile.CardinalPoint.EAST);
                    }
                    if(easternNeighbor)
                    {
                        tile.AddNeighbor(easternNeighbor, AbstractTile.CardinalPoint.EAST);
                        easternNeighbor.AddNeighbor(tile, AbstractTile.CardinalPoint.WEST);
                    }
                }
            }

        }

        private static AbstractTile GetNeighborAtCardinalPoint(AbstractTile tile, AbstractTile.CardinalPoint where)
        {
            Vector3 tilePos = tile.transform.position;

            float key;

            switch(where)
            {
                case AbstractTile.CardinalPoint.NORTH:
                    key = tilePos.z + AbstractTile.s_TILE_SIZE;
                    if(m_mapZtoTile.ContainsKey(key))
                    {
                        foreach (var possibleNeighbor in m_mapZtoTile[key])
                        {
                            if (possibleNeighbor.transform.position.x == tilePos.x && possibleNeighbor.transform.position.y == tilePos.y)
                            {
                                return possibleNeighbor;
                            }
                        }
                    }                   
                    break;
                case AbstractTile.CardinalPoint.SOUTH:
                    key = tilePos.z - AbstractTile.s_TILE_SIZE;
                    if (m_mapZtoTile.ContainsKey(key))
                    {
                        foreach (var possibleNeighbor in m_mapZtoTile[key])
                        {
                            if (possibleNeighbor.transform.position.x == tilePos.x && possibleNeighbor.transform.position.y == tilePos.y)
                            {
                                return possibleNeighbor;
                            }
                        }
                    }
                    break;
                case AbstractTile.CardinalPoint.WEST:
                    key = tilePos.x - AbstractTile.s_TILE_SIZE;
                    if (m_mapXtoTile.ContainsKey(key))
                    {
                        foreach (var possibleNeighbor in m_mapXtoTile[key])
                        {
                            if (possibleNeighbor.transform.position.z == tilePos.z && possibleNeighbor.transform.position.y == tilePos.y)
                            {
                                return possibleNeighbor;
                            }
                        }
                    }
                    break;
                case AbstractTile.CardinalPoint.EAST:
                    key = tilePos.x + AbstractTile.s_TILE_SIZE;
                    if (m_mapXtoTile.ContainsKey(key))
                    {
                        foreach (var possibleNeighbor in m_mapXtoTile[key])
                        {
                            if (possibleNeighbor.transform.position.z == tilePos.z && possibleNeighbor.transform.position.y == tilePos.y)
                            {
                                return possibleNeighbor;
                            }
                        }
                    }
                    break;
            }

            return null;
        }

    }
}