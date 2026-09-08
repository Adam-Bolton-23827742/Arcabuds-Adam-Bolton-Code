using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    [HideInInspector] public float CurrentKnockBackTime, KnockBackTime, KnockBackForce;
    [HideInInspector] public Rigidbody Rigidbody;
    [HideInInspector] public CompanionState CompanionState;
    [HideInInspector] public PlayerMovement PlayerMovement;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public bool CanKnockBack;
    [HideInInspector] public Vector3 KnockBackDirection;
    private PlayerStats Stats;
    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        CompanionState = GetComponent<CompanionState>();
        PlayerMovement = GetComponent<PlayerMovement>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Stats = GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        if (CanKnockBack && Stats.CurrentHealth > 0)
        {
            KnockBackPlayer();
        }
    }

    private void KnockBackPlayer()
    {
        if (CurrentKnockBackTime < KnockBackTime)
        {
            Rigidbody.AddForce(KnockBackDirection * KnockBackForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
            CurrentKnockBackTime += Time.fixedDeltaTime;
        }

        else
        {
            CanKnockBack = false;

            if (!CompanionState.enabled)
            {
                PlayerMovement.enabled = true;
            }

            else
            {
                NavMeshAgent.enabled = true;
            }

            Rigidbody.linearVelocity = Vector3.zero;
        }
    }
}
