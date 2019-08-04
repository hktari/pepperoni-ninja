using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;


public class LevelEnd: MonoBehaviour {

    public string loadScene = "";
    public PlayableDirector track;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        track.Play();
        SceneManager.LoadScene(loadScene);
    }
}
