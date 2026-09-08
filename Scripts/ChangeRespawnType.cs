using UnityEngine;

public class ChangeRespawnType : MonoBehaviour
{
    private RespawnHandler RespawnHandler;

    private void Start()
    {
        RespawnHandler = GameObject.Find("Respawn Handler").GetComponent<RespawnHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            RespawnHandler.IndividualSpawn = true;
        }
    }
}
