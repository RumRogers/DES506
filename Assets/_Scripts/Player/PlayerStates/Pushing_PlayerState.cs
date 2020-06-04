﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Pushing_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;

        bool m_moving = false;

        const float GRID_SIZE = 1.0f;

        public Pushing_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            m_playerEntity.Velocity = Vector3.zero;

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.PUSHING);

            //Rotate player to the object they are pushing
            Quaternion newRotation = Quaternion.LookRotation(m_playerEntity.ClosestInteractable.position - m_playerEntity.transform.position);
            //Might be inefficient but eular angles makes this equasion much easier
            Vector3 eularRoation = newRotation.eulerAngles;
            eularRoation.y = Mathf.RoundToInt(eularRoation.y / 90) * 90;
            Debug.Log(eularRoation);
            m_playerEntity.transform.eulerAngles = eularRoation;
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

                Vector3 forwardMovement = new Vector3(m_playerEntity.transform.forward.x, 0, m_playerEntity.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
                Vector3 rightMovement = new Vector3(m_playerEntity.transform.right.x, 0, m_playerEntity.transform.right.z) * Input.GetAxisRaw("Horizontal");

                //locking it from moving on diagonals
                if (forwardMovement.magnitude > rightMovement.magnitude)
                {
                    m_playerEntity.Direction = forwardMovement.normalized;
                }
                else
                {
                    m_playerEntity.Direction = rightMovement.normalized;
                }


                Vector3 finalPosition = m_playerEntity.transform.position + (m_playerEntity.Direction * GRID_SIZE);
                Vector3 currentPosition = m_playerEntity.transform.position;
                Vector3 movableOffset = m_playerEntity.ClosestInteractable.position - m_playerEntity.transform.position;   

                float time = 0;
                float timeToLerp = GRID_SIZE / m_playerEntity.PushSpeed;
                while (true)
                {
                    time += Time.deltaTime;
                    float perComp = time / timeToLerp;
                    float curvedPerComp = m_playerEntity.PushMovementCurve.Evaluate(perComp);

                    if (perComp > 1)
                    {
                        break;
                    }
                    if (m_playerEntity.ClosestInteractable != null)
                        m_playerEntity.ClosestInteractable.position = Vector3.Lerp(currentPosition + movableOffset, finalPosition + movableOffset, curvedPerComp);

                    m_playerEntity.transform.position = Vector3.Lerp(currentPosition, finalPosition, curvedPerComp);
                    yield return null;
                }
                //Hard setting end point incase something messes up and pushable ends up in slightly the wrong place
                m_playerEntity.transform.position = finalPosition;
                m_playerEntity.ClosestInteractable.position = finalPosition + movableOffset;
                m_moving = false;

                m_playerEntity.OnBoxFinishedMoving();

                m_playerEntity.Velocity = Vector3.zero;

                yield break;
            }
        }
    }
}