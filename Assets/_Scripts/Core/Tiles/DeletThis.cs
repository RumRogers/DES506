using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Tiles
{
    public class DeletThis : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CreateMap());
        }

        private IEnumerator CreateMap()
        {
            yield return new WaitForSeconds(5f);
            TileMapper.SetAllNeighbors();
        }
    }
}