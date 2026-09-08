using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MeleeEnemyAttack : MonoBehaviour
{
    [HideInInspector] public bool CanAttack = true, CanAnimate = true;
    [SerializeField] private float AttackDamage, AttackCooldown, AttackForce, KnockBackTime;
    private Vector3 KnockbackDirection;
    public List<PlayerStats> PlayersToDamage;
    private MeleeEnemyStateMachine MeleeEnemyStateMachine;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [SerializeField] private AssignControllers AssignControllers;
    private EnemyFollow EnemyFollow;
    [SerializeField] private bool ReturnToPatrolAfterBothPlayersDead;
    private EliteBossStateMachine EliteBossStateMachine;
    private ZappyStateMachine ZappyStateMachine;
    private bool ReturnToIdleOrPatrol;
    [HideInInspector] public EliteBossStateMachine.States PreviousEliteState;
    [HideInInspector] public ZappyStateMachine.States PreviousZappyState;

    private void Start()
    {
        PlayersToDamage = new List<PlayerStats>();
        MeleeEnemyStateMachine = GetComponent<MeleeEnemyStateMachine>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        EnemyFollow = GetComponent<EnemyFollow>();
        EliteBossStateMachine = GetComponent<EliteBossStateMachine>();
        ZappyStateMachine = GetComponent<ZappyStateMachine>();

        if (EliteBossStateMachine != null)
        {
            PreviousEliteState = EliteBossStateMachine.States.Idle;
        }

        else if (ZappyStateMachine != null)
        {
            PreviousZappyState = ZappyStateMachine.States.Idle;
        }
    }

    public void Attack()
    {
        if (CanAttack)
        {
            CanAttack = false;

            foreach (PlayerStats Player in PlayersToDamage)
            {
                Rigidbody Rigidbody = Player.GetComponent<Rigidbody>();
                Rigidbody.linearVelocity = new Vector3(0.0f, Rigidbody.linearVelocity.y, 0.0f);
                Knockback PlayerKnockback = Player.GetComponent<Knockback>();
                PlayerKnockback.CurrentKnockBackTime = 0;
                KnockbackDirection = (PlayerKnockback.Rigidbody.transform.position - transform.position).normalized;
                KnockbackDirection.y = 0;
                PlayerKnockback.PlayerMovement.enabled = false;

                if (PlayerKnockback.CompanionState.enabled)
                {
                    PlayerKnockback.NavMeshAgent.enabled = false;
                    PlayerKnockback.CompanionState.CompanionStateMachine = CompanionState.CompanionStates.Idle;
                }

                Player.TakeDamage(AttackDamage);
                PlayerKnockback.KnockBackTime = KnockBackTime;
                PlayerKnockback.KnockBackDirection = KnockbackDirection;
                PlayerKnockback.KnockBackForce = AttackForce;
                PlayerKnockback.CanKnockBack = true;
                NavMeshAgent.velocity = new Vector3(0.0f, NavMeshAgent.velocity.y, 0.0f);
            }

            PlayersToDamage.Clear();
            StartCoroutine(MeleeAttackCooldown());
        }
    }

    public IEnumerator MeleeAttackCooldown()
    {
        print("Attack");

        if (EliteBossStateMachine == null && ZappyStateMachine == null)
        {
            foreach (PlayerInput Player in AssignControllers.PlayerInputs)
            {
                if (Player.GetComponent<PlayerStats>().CurrentHealth <= 0)
                {
                    ReturnToIdleOrPatrol = true;
                }

                else
                {
                    ReturnToIdleOrPatrol = false;
                    EnemyFollow.TargetPosition = Player.transform;
                    break;
                }
            }

            if (ReturnToIdleOrPatrol)
            {
                EnemyFollow.TargetPosition = null;

                if (ReturnToPatrolAfterBothPlayersDead && MeleeEnemyStateMachine.StateMachine != MeleeEnemyStateMachine.States.Dead)
                {
                    MeleeEnemyStateMachine.StateMachine = MeleeEnemyStateMachine.States.Patrol;
                }

                else if (MeleeEnemyStateMachine.StateMachine != MeleeEnemyStateMachine.States.Dead)
                {
                    NavMeshAgent.isStopped = true;
                    MeleeEnemyStateMachine.StateMachine = MeleeEnemyStateMachine.States.Idle;
                }

                ReturnToIdleOrPatrol = false;
            }
        }

        yield return new WaitForSeconds(AttackCooldown);

        if (EnemyFollow.TargetPosition != null && MeleeEnemyStateMachine != null && MeleeEnemyStateMachine.StateMachine != MeleeEnemyStateMachine.States.Dead)
        {
            MeleeEnemyStateMachine.StateMachine = MeleeEnemyStateMachine.States.Follow;
        }

        else if (EliteBossStateMachine != null && EliteBossStateMachine.StateMachine != EliteBossStateMachine.States.Dead)
        {
            EliteBossStateMachine.StateMachine = PreviousEliteState;
        }

        else if (ZappyStateMachine != null && ZappyStateMachine.State != ZappyStateMachine.States.Dead)
        {
            ZappyStateMachine.State = PreviousZappyState;
        }

        CanAttack = true;
        CanAnimate = true;
    }
}
