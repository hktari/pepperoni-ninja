using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;


public class LevelEnd: MonoBehaviour {

    public string loadScene = "";
    public PlayableDirector track;
    public SoundManager m_SoundManager;

    private void Start()
    {
        m_SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_SoundManager?.PlayAudio(m_SoundManager.EndLevelAudio);
        track.Play();
        StartCoroutine(WaitForSceneLoad());
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(3);
     SceneManager.LoadScene(loadScene);

    }
}
