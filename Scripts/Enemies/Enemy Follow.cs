using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private NavMeshAgent NavMeshAgent;
    [HideInInspector] public Transform TargetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void FollowPlayer()
    {
        if (TargetPosition != null)
        {
            if (NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.destination = TargetPosition.position;
            }
        }
    }
}
