using System.Collections;
using System.Collections.Generic;
using GameUI.Dialogue;
using UnityEngine;

public class NPCSimple : MonoBehaviour
{
    #region
    [Header("NPC Behaviour")]
    [SerializeField]
    private bool m_isPlayerTalking = false;

    [SerializeField]
    private LetterBox m_letterBox;

    [Header("Animations")]
    [SerializeField]
    private Animation m_defaultAnimation;

    [SerializeField]
    private Animation m_talkingAnimation;

    private Dialogue m_dialogue;

    public int checkingThisInt = 0;

    private NPCState m_state;
    #endregion

    void Start()
    {
       // m_state = new NPCStateDefault(m_defaultAnimation, m_rotationSpeed, m_player, m_viewRadius, m_letterBox, transform);
    }

    void Update()
    {
        m_state.StateUpdate();
    }


    /// <summary>
    /// Inverts a bool to handle player interaction
    /// </summary>
    public void PlayerInteracts()
    {
        m_isPlayerTalking = !m_isPlayerTalking;
    }
}
