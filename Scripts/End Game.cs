using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public GameObject WinScreen;

    private void OnDestroy()
    {
        if (WinScreen != null)
        {
            WinScreen.SetActive(true);
        }
    }
}
