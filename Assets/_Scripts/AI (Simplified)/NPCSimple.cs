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

    //Contained members
    private Vector3 m_playerToTarget = Vector3.zero;
    private Vector3 m_targetVec = Vector3.zero;
    private Vector3 m_offset  = new Vector3( 1, 1, 0 );
    private bool m_isPlayerInView = true;

    //Defaults
    private Vector3 m_defaultPos = Vector3.zero;
    private Vector3 m_defaultDirection = Vector3.zero;
    private Quaternion m_defaultRot;
    private Dialogue m_dialogue;
    #endregion

    void Start()
    {
        m_targetVec = transform.forward;

        m_defaultDirection = transform.forward;
        m_defaultPos = m_playerCamera.transform.position;
        m_defaultRot = m_playerCamera.transform.rotation;

        m_dialogue = GetComponent<Dialogue>();

        if (!m_player || !m_playerCamera || !m_dialogue || !m_letterBox)
            Debug.LogError(name + " is missing a component");

        //m_dialogue.StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            m_isPlayerTalking = true;

        switch(m_isPlayerTalking)
        {
            case true:

                m_letterBox.TurnOn();

                if (m_isPlayerInView)
                {
                    Vector3 m_isPlayerInViewVec = m_player.transform.position + (m_player.transform.forward * -2) + m_offset;
                    m_playerCamera.transform.position = m_player.transform.position + (m_player.transform.forward * -2) + m_offset;
                }
                
                m_isPlayerInView = false;

                PlayerInteractionState();
                break;

            case false:

                m_letterBox.TurnOff();


                if (!m_isPlayerInView)
                {
                    m_playerCamera.transform.position = Vector3.Lerp(m_playerCamera.transform.position, m_defaultPos, 0.1f);// m_defaultPos;
                    m_playerCamera.transform.rotation = Quaternion.Lerp(m_playerCamera.transform.rotation, m_defaultRot, 0.1f);
                }

                if (Vector3.Distance(m_playerCamera.transform.position, m_defaultPos) < 0.05f)
                    m_isPlayerInView = true;

                DefaultState();
                break;
        }
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

    private void DefaultState()
    {
        if(m_isPlayerInView)
        {
            m_defaultPos = m_playerCamera.transform.position;
            m_defaultRot = m_playerCamera.transform.rotation;
        }

        m_playerToTarget = m_player.transform.position - transform.position;
        m_playerToTarget.y = 0;

        float angle = Vector3.Angle(transform.forward, m_playerToTarget); //Needed for the turning speed

        if (Vector3.Distance(m_player.transform.position, transform.position) <= m_viewRadius)
        {
            m_targetVec = Vector3.RotateTowards(m_targetVec, m_playerToTarget, Time.deltaTime * (angle / 20) * m_rotationSpeed, 0.0f);
            transform.rotation = Quaternion.LookRotation(m_targetVec);
        }
        else
        {
            //Update turning speed to be inverse of angle, as it will be moving away, and as such start slow
            m_targetVec = Vector3.RotateTowards(m_targetVec, m_defaultDirection, Time.deltaTime * (angle / 20) * m_rotationSpeed, 0.0f);
            transform.rotation = Quaternion.LookRotation(m_targetVec);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, m_playerToTarget * 10);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, m_targetVec * 10);
    }
}
