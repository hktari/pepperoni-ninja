using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;


public class LevelEnd: MonoBehaviour {

    public string loadScene = "";
    public PlayableDirector track;
    public bool finalLevel = false;
    public Animation camFinal;
    public Transform disableUIOnDone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        }
        else SceneManager.LoadScene(loadScene);
    }
}
