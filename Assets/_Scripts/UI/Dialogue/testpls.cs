using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testpls : MonoBehaviour
{
    Player.PlayerEntity m_player = null;

    bool canTalk = false;
    public bool p_isTalking { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canTalk = true;
     
            if (!m_player)
            {
                m_player = other.gameObject.GetComponent<Player.PlayerEntity>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canTalk = false;
        }
    }

    private void Update()
    {
        if (canTalk && !p_isTalking)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                p_isTalking = true;
                m_player.SetState(new Player.Dialogue_PlayerState(m_player, this.gameObject));
            }
        }
    }
}
