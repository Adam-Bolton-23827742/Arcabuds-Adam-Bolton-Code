using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public string SceneToLoad;

    void Awake()
    {
        StartCoroutine(WinGame());
    }

    private IEnumerator WinGame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneToLoad);
    }
}
