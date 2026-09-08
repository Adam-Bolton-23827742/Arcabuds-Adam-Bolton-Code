using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElectricBaseAttack : MonoBehaviour
{
    [Header("Attack Vars")]
    public float AttackCooldown;
    public float AttackTimer;
    public bool canAttack;
    private bool isAttacking;

    [Header("Detecting Enemies")]
    [SerializeField] private float ViewRadius;
    [SerializeField] private LayerMask EnemyLayer;

    [Header("Misc")]
    [SerializeField] private Transform CharacterPos;
    [SerializeField] private GameObject Projectile;
    private SoundHandler SoundHandler;
    private PlayerStats Stats;
    public Transform EnemyPos;
    [SerializeField] private CooldownUI CooldownUI;
    [SerializeField] private Animator Animator;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip ProjectileSound;

    private void Awake()
    {
        GameObject Moves = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find("Zappy Moves").gameObject;
        CooldownUI = Moves.transform.Find("Zappy Attack").GetComponent<CooldownUI>();
    }

    private void Start()
    {
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
        Stats = GetComponentInParent<PlayerStats>();
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
        if (canAttack)
        {
            canAttack = false;
            Animator.SetTrigger("Attack");
            SoundHandler.PlaySoundEffect(AudioSource, ProjectileSound, null, transform.position);
            Collider[] Enemies = Physics.OverlapSphere(transform.position, ViewRadius, EnemyLayer);
            foreach (Collider Enemy in Enemies)
            {
                EnemyPos = Enemy.transform;
            }

            GameObject NewProjectile = Instantiate(Projectile, CharacterPos.position, transform.rotation);
            NewProjectile.GetComponent<ZappyProjectile>().Char = gameObject;
            AttackCooldown = 0.0f;

            if (CooldownUI != null)
            {
                CooldownUI.UpdateFillAmount(0.0f, false);
            }
        }
    }
}
