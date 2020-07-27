using System.Collections;
using System.Collections.Generic;
using GameUI.Dialogue;
using UnityEngine;

public class NPCSimple : MonoBehaviour
{
    #region
    [Header("Required Components")]
    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private Camera m_playerCamera;

    [Header("NPC Behaviour")]
    [SerializeField]
    private string m_dialog; //To be added

    [SerializeField]
    private bool m_isPlayerTalking = false;

    [SerializeField]
    private float m_rotationSpeed;

    [SerializeField]
    private float m_viewRadius;

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
        m_state = new NPCStateDefault(m_defaultAnimation, m_rotationSpeed, m_player, m_viewRadius, m_letterBox, transform);
    }

    void Update()
    {
        m_state.StateUpdate();
    }


    private void PlayerInteractionState()
    {
        m_player.transform.LookAt(new Vector3(transform.position.x, m_player.transform.position.y, transform.position.z));    
        
        m_playerCamera.transform.LookAt(transform);

        //Translate camera into position
        //once camera is in place start dialog
        //move letterboxes into place 
    }

    /// <summary>
    /// Inverts a bool to handle player interaction
    /// </summary>
    public void PlayerInteracts()
    {
        m_isPlayerTalking = !m_isPlayerTalking;
    }
}
