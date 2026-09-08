using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private MeleeEnemyAttack MeleeEnemyAttack;
    private MeleeEnemyStateMachine MeleeEnemyStateMachine;
    private EliteBossStateMachine EliteBossStateMachine;
    private ZappyStateMachine ZappyStateMachine;
    [SerializeField] private Animator Animator;
    [SerializeField] private AttackAnims Anims;

    private void Start()
    {
        MeleeEnemyAttack = GetComponentInParent<MeleeEnemyAttack>();
        MeleeEnemyStateMachine = GetComponentInParent<MeleeEnemyStateMachine>();
        EliteBossStateMachine = GetComponentInParent<EliteBossStateMachine>();
        ZappyStateMachine = GetComponentInParent<ZappyStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tag == "Attack")
        {
            PlayerStats Stats = other.GetComponent<PlayerStats>();

            if (Stats != null && !MeleeEnemyAttack.PlayersToDamage.Contains(Stats) && Stats.CurrentHealth > 0)
            {
                MeleeEnemyAttack.PlayersToDamage.Add(Stats);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (tag == "Attack")
        {
            PlayerStats Stats = other.GetComponent<PlayerStats>();

            if (Stats != null && Stats.CurrentHealth > 0)
            {
                if (MeleeEnemyStateMachine != null && MeleeEnemyStateMachine.StateMachine != MeleeEnemyStateMachine.States.Dead)
                {
                    MeleeEnemyStateMachine.StateMachine = MeleeEnemyStateMachine.States.Attack;
                }

                else if (EliteBossStateMachine != null && EliteBossStateMachine.StateMachine != EliteBossStateMachine.States.Dead)
                {
                    if (EliteBossStateMachine.StateMachine != EliteBossStateMachine.States.Attack)
                    {
                        MeleeEnemyAttack.PreviousEliteState = EliteBossStateMachine.StateMachine;
                    }
                    
                    EliteBossStateMachine.StateMachine = EliteBossStateMachine.States.Attack;
                }
            }
        }

        else if (other.tag == "Player" && MeleeEnemyAttack.CanAnimate && MeleeEnemyStateMachine != null && MeleeEnemyStateMachine.StateMachine != MeleeEnemyStateMachine.States.Dead
            || other.tag == "Player" && MeleeEnemyAttack.CanAnimate && EliteBossStateMachine != null && EliteBossStateMachine.StateMachine != EliteBossStateMachine.States.Dead
            || other.tag == "Player" && MeleeEnemyAttack.CanAnimate && ZappyStateMachine != null && ZappyStateMachine.State != ZappyStateMachine.States.Dead)
        {
            if (other.GetComponent<PlayerStats>().CurrentHealth > 0)
            {
                MeleeEnemyAttack.CanAnimate = false;

                if (MeleeEnemyStateMachine != null)
                {
                    Anims.PlayAttackAnim(Animator, "AttackBodyslam", "AttackTailwhip");
                }

                else if (EliteBossStateMachine != null || ZappyStateMachine != null)
                {
                    Anims.PlayAttackAnim(Animator, "MeleeAttack", null);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tag == "Attack")
        {
            PlayerStats Stats = other.GetComponent<PlayerStats>();

            if (Stats != null && MeleeEnemyAttack.PlayersToDamage.Contains(Stats))
            {
                MeleeEnemyAttack.PlayersToDamage.Remove(Stats);
            }
        }
    }
}
