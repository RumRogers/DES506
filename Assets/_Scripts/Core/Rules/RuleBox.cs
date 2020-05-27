#define SHOW_TEXT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCore.Rules
{

    public class RuleBox : MonoBehaviour
    {
        [SerializeField]
        private RuleChunk.ChunkType m_chunkType;
        [SerializeField]
        private string m_chunkLexeme;

        public RuleChunk.ChunkType p_ChunkType { get => m_chunkType; }
        public string p_ChunkLexeme { get => m_chunkLexeme; }

#if SHOW_TEXT
        private TextMesh m_textMesh;
#endif
        // Start is called before the first frame update
        void Start()
        {
#if SHOW_TEXT
            var textMeshPrefab = Resources.Load<GameObject>("Prefabs/RuleBoxes/TextMesh");
            var textMeshInstance = Instantiate(textMeshPrefab, transform);

            m_textMesh = textMeshInstance.GetComponent<TextMesh>();
            m_textMesh.text = m_chunkLexeme;
#endif
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

