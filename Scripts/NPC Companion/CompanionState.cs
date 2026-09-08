
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class CompanionState : MonoBehaviour
{
    public Transform PlayerToFollow;
    [SerializeField] private float IdleDistance;
    private NavMeshAgent NavMeshAgent;
    [HideInInspector] public bool IsBouncing;
    private PlayerStats PlayerStats;
    private Animator Animator;
    private GroundCheck GroundCheck;

    public enum CompanionStates
    {
        Idle,
        Follow
    }

    public CompanionStates CompanionStateMachine;

    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        PlayerStats = GetComponent<PlayerStats>();
        Animator = GetComponentInChildren<Animator>();
        GroundCheck = GetComponent<GroundCheck>();
    }

    private void CheckCompanionDistance()
    {
        if (NavMeshAgent.enabled)
        {
            if (Vector3.Distance(PlayerToFollow.position, transform.position) > IdleDistance)
            {
                CompanionStateMachine = CompanionStates.Follow;
            }

            else
            {
                CompanionStateMachine = CompanionStates.Idle;
            }
        }
    }

    private void Update()
    {
        if (PlayerStats.CurrentHealth > 0)
        {
            switch (CompanionStateMachine)
            {
                case CompanionStates.Idle:
                    CheckCompanionDistance();

                    if (NavMeshAgent.enabled)
                    {
                        NavMeshAgent.isStopped = true;
                    }

                    break;
                case CompanionStates.Follow:
                    CheckCompanionDistance();

                    if (NavMeshAgent.enabled)
                    {
                        NavMeshAgent.isStopped = false;
                        NavMeshAgent.destination = PlayerToFollow.position;
                    }

                    if (GroundCheck.IsGrounded() && NavMeshAgent.velocity != Vector3.zero && Animator != null)
                    {
                        Animator.SetBool("Walking", true);
                    }

                    else if (GroundCheck.IsGrounded() && NavMeshAgent.velocity == Vector3.zero && Animator != null)
                    {
                        Animator.SetBool("Walking", false);
                    }

                    if (NavMeshAgent.velocity.y <= 0.0f && !GroundCheck.IsGrounded() && Animator != null && !Animator.GetBool("Falling"))
                    {
                        Animator.SetBool("Walking", false);
                        Animator.SetBool("Falling", true);
                    }

                    if (GroundCheck.IsGrounded() && Animator != null && Animator.GetBool("Falling") == true)
                    {
                        Animator.SetBool("Falling", false);
                        Animator.SetBool("Landing", true);
                    }

                    break;
            }
        }
    }
}
