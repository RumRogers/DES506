using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testpls : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Pls");

            Player.PlayerEntity m_player = other.gameObject.GetComponent<Player.PlayerEntity>();

            m_player.SetState(new Player.Dialogue_PlayerState(m_player, this.gameObject));
        }
    }
}
