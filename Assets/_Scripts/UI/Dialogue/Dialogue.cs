using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Dialogue
{
    [System.Serializable]
    public class Line //going to set this up this way so we can extend the line class to include additional info, i.e. Speaker, options, etc.
    {
        public string text;
    }
    [System.Serializable]
    public class Lines
    {
        public Line[] lines;
    }

    public class Dialogue : MonoBehaviour
    {
        Transform m_player;
        Transform m_talkingNPC;

        [SerializeField]
        GameObject m_TextUI;
        [SerializeField]
        Text m_DialogueText;

        [SerializeField]
        TextAsset m_convoJSON;

        Lines m_lines;

        int m_lineIndex = 0;
        [SerializeField]
        float m_scrollSpeed = 0.05f;

        public bool m_lastLineReached = false;
        bool m_dialogueHasStarted = false;

        //FMOD stuff for dialogue audio
        [FMODUnity.EventRef]
        public string fmodDialogueEvent;
        [FMODUnity.EventRef]
        public string fmodSecondDialogueEvent;
        FMOD.Studio.EventInstance fmodDialogue;
        FMOD.Studio.PARAMETER_ID dialogueBoxNumber;
        FMOD.Studio.PARAMETER_ID snapshotIntensity; 
        FMOD.Studio.EventInstance dialogueMix;

        private void Awake()
        {
            // m_lines = JsonUtility.FromJson<Lines>(m_convoJSON.text);

        }

        public void StartDialogue()
        {
            m_lines = JsonUtility.FromJson<Lines>(m_convoJSON.text);

            m_lineIndex = 0;
            m_TextUI.SetActive(true);
            StartFillingLine(m_lineIndex);

            try
            {
                fmodDialogue = FMODUnity.RuntimeManager.CreateInstance(fmodDialogueEvent);
                //dialogueMix = FMODUnity.RuntimeManager.CreateInstance("snapshot:/DialogueMix");
                fmodDialogue.start();
                //dialogueMix.start();
                //dialogueMix.setParameterByName("snapshotIntensity", 100);
            }
            catch(FMODUnity.EventNotFoundException)
            {
                Debug.LogError("Invalid FMOD event for this dialogue! Check it out.");
            }

            finally
            {
                if (m_lineIndex < m_lines.lines.Length - 1)
                {
                    ++m_lineIndex;
                }
                else
                {
                    m_lastLineReached = true;
                    m_lineIndex = 0;
                }

                m_dialogueHasStarted = true;
            }
        }

        public void SetNewDialogue(TextAsset dialogue)
        {
            m_convoJSON = dialogue;
            fmodDialogueEvent = fmodSecondDialogueEvent;
        }

        private void Update()
        {
            fmodDialogue.setParameterByName("dialogueBoxNumber", m_lineIndex);
        }

        public void FillNextLine()
        {
            StartFillingLine(m_lineIndex);
            if (m_lineIndex < m_lines.lines.Length - 1)
            {
                m_lineIndex++;
            }
            else
            {
                m_lastLineReached = true;
                m_lineIndex = 0;
            }
        }

        public void DisableUI()
        {
            m_TextUI.SetActive(false);
            fmodDialogue.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        public bool GetDialogueHasStarted()
        {
            return m_dialogueHasStarted;
        }

        public bool GetLastLineReached()
        {
            return m_lastLineReached;
        }

        public void ResetDialogue()
        {
            m_dialogueHasStarted = false;
            m_lastLineReached = false;
            //m_lineIndex = 0;
        }

        void StartFillingLine(int index)
        {
            //StartCoroutine(GameUI.Dialogue.StringHelpers.FillDialogueBox(m_DialogueText, m_lines.lines[index].text, m_scrollSpeed));
            m_DialogueText.text = m_lines.lines[index].text;
        }

    }
}
