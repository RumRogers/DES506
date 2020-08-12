using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroCutsceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_loadingScreen;
    VideoPlayer m_videoPlayer;
    bool m_coroutineStarted = false;
    Vector3 m_offset = new Vector3(10000f, 10000f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        m_loadingScreen.transform.position += m_offset;
        m_videoPlayer = GetComponent<VideoPlayer>();
        m_videoPlayer.loopPointReached += OnCutsceneEnd;
    }

    private void OnCutsceneEnd(VideoPlayer source)
    {
        if(m_coroutineStarted)
        {
            return;
        }

        m_coroutineStarted = true;
        m_loadingScreen.SetActive(true);
        m_loadingScreen.transform.position -= m_offset;
        m_videoPlayer.enabled = false;
        StartCoroutine(LoadSceneAsynch("master_scene"));
    }

    IEnumerator LoadSceneAsynch(string sceneName)
    {
        yield return new WaitForSeconds(3f);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while(!op.isDone)
        {
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnCutsceneEnd(null);
        }
    }
}
