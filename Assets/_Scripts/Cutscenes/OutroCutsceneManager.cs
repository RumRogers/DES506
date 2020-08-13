using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OutroCutsceneManager : MonoBehaviour
{
    VideoPlayer m_videoPlayer;
    bool m_coroutineStarted = false;
    bool m_videoWasPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        Bus masterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        m_videoPlayer = GetComponent<VideoPlayer>();
        m_videoPlayer.loopPointReached += OnCutsceneEnd;
    }

    private void OnCutsceneEnd(VideoPlayer source)
    {
        m_videoWasPlayed = true;        
    }

    IEnumerator LoadSceneAsynch(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            yield return null;
        }

        //yield return new WaitForSeconds(3f);
    }
    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;

        if (m_videoWasPlayed && !m_coroutineStarted)
        {
            m_coroutineStarted = true;
            m_videoPlayer.enabled = false;
            StartCoroutine(LoadSceneAsynch("Credits"));
        }
    }
}
