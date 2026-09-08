using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyStateMachine : MonoBehaviour
{
    public enum States
    {
        Idle,
        Follow,
        Attack,
        Patrol,
        Dead
    }

    public States StateMachine = States.Idle;

    private MeleeEnemyAttack MeleeEnemyAttack;
    private EnemyFollow EnemyFollow;
    private FieldOfView FieldOfView;
    private Patrol Patrol;
    private NavMeshAgent Agent;
    private Animator Animator;
    private EnemyStats Stats;

    private void Start()
    {
        MeleeEnemyAttack = GetComponent<MeleeEnemyAttack>();
        EnemyFollow = GetComponent<EnemyFollow>();
        FieldOfView = GetComponent<FieldOfView>();
        Patrol = GetComponent<Patrol>();
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();
        Stats = GetComponent<EnemyStats>();
    }

    private void Update()
    {
        switch (StateMachine)
        {
            case States.Idle:
                Animator.SetBool("Walking", false);
                FieldOfView.SearchForPlayer();
                break;
            case States.Follow:

                if (Agent.enabled)
                {
                    Agent.isStopped = false;
                }
                
                EnemyFollow.FollowPlayer();
                PlayWalkAnim();
                break;
            case States.Attack:
                MeleeEnemyAttack.Attack();
                break;
            case States.Patrol:
                Agent.isStopped = false;
                Patrol.PatrolPath();
                FieldOfView.SearchForPlayer();
                PlayWalkAnim();
                break;
            case States.Dead:
                MeleeEnemyAttack.enabled = false;
                FieldOfView.enabled = false;
                EnemyFollow.enabled = false;

                if (Agent.enabled)
                {
                    Agent.isStopped = true;
                    Agent.enabled = false;
                }
                
                Patrol.enabled = false;
                Stats.enabled = false;
                break;
        }
    }

    private void PlayWalkAnim()
    {
        if (Agent.velocity != Vector3.zero)
        {
            Animator.SetBool("Walking", true);
        }

        else
        {
            Animator.SetBool("Walking", false);
        }
    }
}
