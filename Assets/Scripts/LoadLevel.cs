using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    public string loadScene = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(loadScene);
    }
}
