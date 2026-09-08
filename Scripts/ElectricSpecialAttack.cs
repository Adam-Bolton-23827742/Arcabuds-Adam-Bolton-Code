using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElectricSpecialAttack : MonoBehaviour
{
    [Header("Attack Vars")]
    public float AttackCooldown;
    public float AttackTimer;
    public float AttackDamage;
    public bool canAttack;
    public float MovementSpeed; //For the attack
    public float TimeBetweenAttacks;

    [Header("Misc")]
    [SerializeField] private PlayerInput PlayerInput;
    private PlayerStats Stats;
    [SerializeField] private CooldownUI CooldownUI;
    [SerializeField] private Animator Animator;
    private SoundHandler SoundHandler;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip SpecialSound;
    [SerializeField] private float EnemySearchRadius;
    private Transform Target;
    [SerializeField] private LayerMask EnemyLayer;

    private void Awake()
    {
        GameObject Moves = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find("Zappy Moves").gameObject;
        CooldownUI = Moves.transform.Find("Zappy Special").GetComponent<CooldownUI>();
    }

    private void Start()
    {
        PlayerInput = GetComponent<PlayerInput>(); //Remember to change to in parent if needed.
        Stats = GetComponent<PlayerStats>();
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
    }
    private void Update()
    {
        if (Time.timeScale == 1.0f)
        {
            if (AttackCooldown < AttackTimer)
            {
                AttackCooldown += Time.deltaTime;

                if (CooldownUI != null)
                {
                    CooldownUI.UpdateFillAmount(1.0f / AttackTimer, false);
                }

                if (Stats.CurrentHealth <= 0)
                {
                    AttackCooldown = AttackTimer;

                    if (CooldownUI != null)
                    {
                        CooldownUI.UpdateFillAmount(1.0f, true);
                    }
                }
            }

            if (AttackCooldown >= AttackTimer)
            {
                canAttack = true;
            }
        }
    }

    public void CallAttack()
    {
        StartCoroutine(Attack());
    }

    public IEnumerator Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            Animator.SetBool("SpecialAttacking", true);
            SoundHandler.PlaySoundEffect(AudioSource, SpecialSound, null, transform.position);

            for (int i = 0; i < 3; i++)
            {
                Collider[] EnemyColliders = Physics.OverlapSphere(transform.position, EnemySearchRadius, EnemyLayer);

                foreach (Collider Collider in EnemyColliders)
                {
                    if (!Collider.GetComponent<EnemyStats>().hitByZappySpecial)
                    {
                        if (Target == null || Vector3.Distance(transform.position, Collider.transform.position) < Vector3.Distance(transform.position, Target.position))
                        {
                            Target = Collider.transform;
                        }
                    }
                }

                if (Target != null)
                {
                    while (Vector3.Distance(transform.position, Target.position) >= 0.05f && Target != null)
                    {
                        transform.LookAt(Target.position);
                        transform.position = Vector3.MoveTowards(transform.position, Target.position, MovementSpeed * Time.deltaTime);
                        Target.GetComponent<EnemyStats>().hitByZappySpecial = true;
                        Target.GetComponent<EnemyStats>().CallFunction();
                        Target.GetComponent<EnemyStats>().TakeDamage(AttackDamage, PlayerInput, Stats.Rumble);
                        yield return null;
                    }

                    Target = null;
                }
            }

            Animator.SetBool("SpecialAttacking", false);
            AttackCooldown = 0.0f;

            if (CooldownUI != null)
            {
                CooldownUI.UpdateFillAmount(0.0f, false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, EnemySearchRadius);
    }
}
