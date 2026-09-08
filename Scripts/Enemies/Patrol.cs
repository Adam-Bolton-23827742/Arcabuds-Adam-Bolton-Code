using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] private Transform[] PathPoints;
    private NavMeshAgent Agent;
    private int PointIndex = 1;
    [SerializeField] private float MaxDistanceFromPoint;
    private MeleeEnemyStateMachine MeleeEnemyStateMachine;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        MeleeEnemyStateMachine = GetComponent<MeleeEnemyStateMachine>();

        if (MeleeEnemyStateMachine.StateMachine == MeleeEnemyStateMachine.States.Patrol)
        {
            transform.position = PathPoints[0].position;
            Agent.destination = PathPoints[PointIndex].position;
        }
    }

    public void PatrolPath()
    {
        if (Vector3.Distance(transform.position, Agent.destination) < MaxDistanceFromPoint)
        {
            if (PointIndex == PathPoints.Length - 1)
            {
                PointIndex = 0;
            }

            else
            {
                PointIndex++;
            }
                
            Agent.destination = new Vector3(PathPoints[PointIndex].position.x, transform.position.y, PathPoints[PointIndex].position.z);
        }
    }
}
