using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadscene : MonoBehaviour
{
    [SerializeField] private string SceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
