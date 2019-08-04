using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;


public class LevelEnd: MonoBehaviour {

    public string loadScene = "";
    public PlayableDirector track;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        track.Play();
        StartCoroutine(WaitForSceneLoad());
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(3);
     SceneManager.LoadScene(loadScene);

    }
}
