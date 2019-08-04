using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;


public class LevelEnd: MonoBehaviour {

    public string loadScene = "";
    public PlayableDirector track;
    public SoundManager m_SoundManager;
    public bool finalLevel = false;
    public Animation camFinal;
    public Transform disableUIOnDone;

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
        if (finalLevel)
        {
            disableUIOnDone.gameObject.SetActive(false);
            camFinal.Play("FinishFinalLevel");
            yield return new WaitForSeconds(10);
            SceneManager.LoadScene(loadScene);
        }
        else SceneManager.LoadScene(loadScene);
    }
}
