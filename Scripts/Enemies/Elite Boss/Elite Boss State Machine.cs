using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EliteBossStateMachine : MonoBehaviour
{
    public enum States
    {
        Idle,
        Follow,
        Attack,
        Slam,
        Projectile,
        Dead
    }

    public States StateMachine = States.Idle;

    private MeleeEnemyAttack MeleeEnemyAttack;
    private EnemyFollow EnemyFollow;
    private NavMeshAgent Agent;
    private Animator Animator;
    private EnemyStats Stats;
    private Slam Slam;
    private ProjectileAttackScript ProjectileAttack;
    [SerializeField] private float StateDuration;
    [SerializeField] private PlayerStats[] Players;
    [SerializeField] private Collider AttackBox;
    private bool IsRunning;
    [SerializeField] private AudioClip MusicClipAfterDeath;
    [SerializeField] private AudioSource[] MusicSources;

    private void Start()
    {
        MeleeEnemyAttack = GetComponent<MeleeEnemyAttack>();
        EnemyFollow = GetComponent<EnemyFollow>();
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();
        Stats = GetComponent<EnemyStats>();
        Slam = GetComponentInChildren<Slam>();
        ProjectileAttack = GetComponent<ProjectileAttackScript>();
    }

    private void Update()
    {
        switch (StateMachine)
        {
            case States.Idle:
                Animator.SetBool("Walking", false);
                break;
            case States.Follow:
                Animator.SetBool("RangedMode", false);
                AttackBox.enabled = true;
                foreach(PlayerStats Player in Players)
                {
                    if (Player.CurrentHealth > 0)
                    {
                        EnemyFollow.TargetPosition = Player.transform;
                        break;
                    }
                }

                Agent.isStopped = false;
                EnemyFollow.FollowPlayer();
                PlayWalkAnim();
                StartCoroutine(BossStateChange(States.Slam, StateDuration));
                break;
            case States.Attack:
                AttackBox.enabled = true;
                MeleeEnemyAttack.Attack();
                break;
            case States.Slam:
                AttackBox.enabled = false;
                Agent.isStopped = true;
                PlayWalkAnim();
                Slam.SlamAttack();
                StartCoroutine(BossStateChange(States.Projectile, StateDuration));
                break;
            case States.Projectile:
                AttackBox.enabled = true;
                transform.LookAt(EnemyFollow.TargetPosition.position);
                Animator.SetBool("RangedMode", true);
                PlayWalkAnim();
                StartCoroutine(ProjectileAttack.CallAttack());
                break;
            case States.Dead:
                AttackBox.enabled = false;
                MeleeEnemyAttack.enabled = false;
                EnemyFollow.enabled = false;

                if (Agent.enabled)
                {
                    Agent.isStopped = true;
                    Agent.enabled = false;
                }

                Stats.enabled = false;

                UpdateMusic.SpawnMusicTransition(MusicSources, MusicClipAfterDeath);

                enabled = false;
                break;
        }
    }

    public IEnumerator BossStateChange(States NewState, float Delay)
    {
        if (!IsRunning)
        {
            IsRunning = true;
            yield return new WaitForSeconds(Delay);
            StateMachine = NewState;
            IsRunning = false;
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
