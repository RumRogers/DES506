using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Pushing_PlayerState : GameCore.System.State
    {
        PlayerMovement m_playerMovement;

        bool m_moving = false;

        const float GRID_SIZE = 1.0f;

        public Pushing_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerMovement = (PlayerMovement)owner;
            m_playerMovement.Velocity = Vector3.zero;
        }

        public override void Manage()
        {
            if (!m_moving)
            {
                m_owner.StartCoroutine(Move());
            }
        }

        IEnumerator Move()
        {
            if (Input.GetButtonDown("Interact"))
            {
                m_owner.SetState(new Default_PlayerState(m_owner));
            }
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                m_moving = true;
                Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
                Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
                m_playerMovement.Direction = (forwardMovement + rightMovement).normalized;

                Vector3 finalPosition = m_playerMovement.transform.position + (m_playerMovement.Direction * GRID_SIZE);
                Vector3 currentPosition = m_playerMovement.transform.position;
                Vector3 movableOffset = m_playerMovement.ClosestInteractable.position - m_playerMovement.transform.position;   

                float time = 0;
                float timeToLerp = GRID_SIZE / m_playerMovement.PushSpeed;
                while (true)
                {
                    time += Time.deltaTime;
                    float perComp = time / timeToLerp;
                    float curvedPerComp = m_playerMovement.PushMovementCurve.Evaluate(perComp);

                    if (perComp > 1)
                    {
                        break;
                    }
                    if (m_playerMovement.ClosestInteractable != null)
                        m_playerMovement.ClosestInteractable.position = Vector3.Lerp(currentPosition + movableOffset, finalPosition + movableOffset, curvedPerComp);

                    m_playerMovement.transform.position = Vector3.Lerp(currentPosition, finalPosition, curvedPerComp);
                    yield return null;
                }
                //Hard setting end point incase something messes up and pushable ends up in slightly the wrong place
                m_playerMovement.transform.position = finalPosition;
                m_playerMovement.ClosestInteractable.position = finalPosition + movableOffset;
                m_moving = false;

                m_playerMovement.OnBoxFinishedMoving();

                m_playerMovement.Velocity = Vector3.zero;

                yield break;
            }
        }
    }
}