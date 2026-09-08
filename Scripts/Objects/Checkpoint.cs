using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private RespawnHandler RespawnHandler;
    [SerializeField] Transform OriginalSpawn;

    private void Start()
    {
        RespawnHandler = GameObject.Find("Respawn Handler").GetComponent<RespawnHandler>();
        RespawnHandler.RecentCheckpoint = OriginalSpawn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            RespawnHandler.RecentCheckpoint = transform;
        }
    }
}
