using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Alert : MonoBehaviour
{
    private Animator Animator;
    private NavMeshAgent Agent;
    private EnemyFollow EnemyFollow;
    private MeleeEnemyStateMachine MeleeEnemyStateMachine;
    private EliteBossStateMachine EliteBossStateMachine;

    private void Start()
    {
        EnemyFollow = GetComponent<EnemyFollow>();
        MeleeEnemyStateMachine = GetComponent<MeleeEnemyStateMachine>();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        EliteBossStateMachine = GetComponent<EliteBossStateMachine>();
    }

    public IEnumerator StartAlert(Collider Player)
    {
        Agent.isStopped = true;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("Alerted");
        yield return new WaitUntil(() => !Animator.GetCurrentAnimatorStateInfo(0).IsName("Alerted"));
        EnemyFollow.TargetPosition = Player.transform;

        if (MeleeEnemyStateMachine != null && MeleeEnemyStateMachine.StateMachine != MeleeEnemyStateMachine.States.Dead)
        {
            MeleeEnemyStateMachine.StateMachine = MeleeEnemyStateMachine.States.Follow;
        }

        else if (EliteBossStateMachine != null && EliteBossStateMachine.StateMachine != EliteBossStateMachine.States.Dead)
        {
            EliteBossStateMachine.StateMachine = EliteBossStateMachine.States.Follow;
        }
    }
}
