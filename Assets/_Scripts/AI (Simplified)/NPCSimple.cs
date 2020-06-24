using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSimple : MonoBehaviour
{
    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private string m_dialog;

    [SerializeField]
    private Camera m_playerCamera;

    private bool m_isPlayerTalking = false;
    private const float c_viewRadius = 5.0f;
    private Vector3 c_offset = new Vector3(1, 1, 1);
    private Vector3 playerToTarget;
    private Vector3 test;

    void Start()
    {
        test = transform.forward;

        if (!m_player || !m_playerCamera)
            Debug.LogError(name + " is missing a component");
    }

    void Update()
    {
        switch(m_isPlayerTalking)
        {
            case true:
                PlayerInteractionState();
                break;

            case false:
                DefaultState();
                break;
        }

        if (Input.GetKeyDown(KeyCode.P))
            m_isPlayerTalking = true;
    }

    private void PlayerInteractionState()
    {
        //Camera changes position
        m_playerCamera.transform.position = m_player.transform.localPosition + c_offset;
        m_playerCamera.transform.LookAt(transform);
        //Letter box effect?
        //NPC's dialog 
    }

    public void PlayerInteracts()
    {
        m_isPlayerTalking = true;
    }

    private void DefaultState()
    {
        playerToTarget = m_player.transform.position - this.transform.position;
        playerToTarget.y = 0;

        float angle = Vector3.Angle(transform.forward, playerToTarget);

        if (Vector3.Distance(m_player.transform.position, transform.position) <= c_viewRadius)
        {
            test = Vector3.RotateTowards(test, playerToTarget, Time.deltaTime * (angle / 60), 0.0f);
            transform.rotation = Quaternion.LookRotation(test);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, playerToTarget * 10);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, test * 10);
    }
}
