using UnityEngine;

public class ResetControllers : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerConfigurationManager.PlayerConfigurationList != null)
        {
            PlayerConfigurationManager.PlayerConfigurationList.Clear();
        }
    }
}
