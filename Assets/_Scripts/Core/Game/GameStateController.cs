using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.GameState
{

    public class GameStateController : GameCore.System.Automaton
    {
        [SerializeField]
        GameObject m_pauseMenu;

        [SerializeField]
        GameObject[] m_OtherUIsToHide;

        //[SerializeField]
        //Texture2D m_mouseCursor;


        GameCore.System.State m_prevState = null;
        GameCore.System.State m_prevCameraState = null;

        void Awake()
        {
            SetState(new Playing_State(this));

            //if (m_mouseCursor)
            //{
            //    //Manually offset the whitespace for now
            //    Cursor.SetCursor(m_mouseCursor, new Vector2(10, 0), CursorMode.Auto);
            //}
        }

        override protected void Update()
        {
            m_state.Manage();
        }

        public GameObject GetPauseMenu()
        {
            return m_pauseMenu;
        }

        //Ugly as all hell but you can't just call SetState straight from the button apparently, so we have
        //to deal with this ugliness.
        public void ResumeButtonPress() 
        {
            SetState(new Playing_State(this, m_prevState, m_prevCameraState));
        }

        public bool IsPaused()
        {
            return typeof(Paused_State).IsInstanceOfType(m_state);
        }

        public void SetPrevState(GameCore.System.State playerState, GameCore.System.State cameraState) 
        {
            m_prevState = playerState;
            m_prevCameraState = cameraState;            
        }

        public void SetIngameUIActive(bool active) //temp, disabling everything will probably break stuff!
        {
            foreach (GameObject go in m_OtherUIsToHide)
            {
                go.SetActive(active);
            }
        }
    }
}