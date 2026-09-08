using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class BellySlam : MonoBehaviour
{
    [SerializeField] private float SlamForce, UpForce, UpTime, SlamDamage, AttackRadius, SlamCooldown;
    [SerializeField] private LayerMask DamageLayers, LilyPadLayers;
    private Rigidbody Rigidbody;
    [HideInInspector] public bool IsSlamming, IsAttacking;
    private PlayerInput PlayerInput;
    [SerializeField] private AudioClip BounceSound;
    [SerializeField] private SoundHandler SoundHandler;
    private Lillypad Lillypad;
    private GroundCheck GroundCheck;
    private PlayerStats PlayerStats;
    private PlayerMovement PlayerMovement;
    private Animator Animator;
    private float SlamTimer;
    [SerializeField] private CooldownUI CooldownUI;

    [Header("Tom Changes")]
    [SerializeField] private AudioClip BounceOnEnemy;
    [SerializeField] private AudioSource EnemyBounceSource;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        PlayerInput = GetComponent<PlayerInput>();
        GroundCheck = GetComponent<GroundCheck>();
        PlayerStats = GetComponent<PlayerStats>();
        PlayerMovement = GetComponent<PlayerMovement>();
        Animator = GetComponentInChildren<Animator>();
        SlamTimer = SlamCooldown;
    }

    private void Update()
    {
        if (Time.timeScale == 1.0f)
        {
            if (SlamTimer < SlamCooldown)
            {
                SlamTimer += Time.deltaTime;
                CooldownUI.UpdateFillAmount(1.0f / SlamCooldown, false);

                if (PlayerStats.CurrentHealth <= 0)
                {
                    SlamTimer = SlamCooldown;
                    CooldownUI.UpdateFillAmount(1.0f, true);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (GroundCheck.IsGrounded() && IsSlamming && GroundCheck.GroundLayerName != "Lilypad" && PlayerStats.CurrentHealth > 0)
        {
            Collider[] ObjectsToDamage = Physics.OverlapSphere(transform.position, AttackRadius, DamageLayers);

            foreach (Collider Colliders in ObjectsToDamage)
            {
                if (Colliders.transform.tag == "Enemy")
                {
                    Colliders.GetComponent<EnemyStats>().TakeDamage(SlamDamage, PlayerInput, PlayerStats.Rumble);
                    SoundHandler.PlaySoundEffect(EnemyBounceSource, BounceOnEnemy, null, gameObject.transform.position);
                }

                else if (Colliders.GetComponent<Destructible_Script>() && Colliders.transform.tag != "PebblesDestructible")
                {
                    Colliders.GetComponent<Destructible_Script>().TakeDamage(SlamDamage);
                }
            }
            Debug.Log("Running This");

            if (GroundCheck.GroundLayerName != "Button")
            {
                IsSlamming = false;
            }
        }
    }

    public void SpecialAttack()
    {
        if (GroundCheck.IsGrounded() && PlayerStats.CurrentHealth > 0 && SlamTimer >= SlamCooldown)
        {
            StartCoroutine(BellySlamAttack());
        }
    }

    private IEnumerator BellySlamAttack()
    {
        print("Preparing Slam");
        IsAttacking = true;
        SlamTimer = 0.0f;
        CooldownUI.UpdateFillAmount(0.0f, false);
        PlayerMovement.CanPlayFall = false;
        Animator.SetTrigger("Bellyflop");
        Rigidbody.AddForce(Vector3.up * UpForce, ForceMode.Impulse);
        yield return new WaitForSeconds(UpTime);
        print("Initiating Slam");
        IsAttacking = false;
        IsSlamming = true;
        Rigidbody.AddForce(Vector3.down * SlamForce, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bounce" && IsSlamming && PlayerStats.CurrentHealth > 0)
        {
            IsSlamming = false;
            Lillypad = collision.transform.GetComponent<Lillypad>();
            SoundHandler.PlaySoundEffect(Lillypad.AudioSource, BounceSound, null, Lillypad.transform.position);

            foreach (GameObject Character in Lillypad.CharactersOnLilypad)
            {
                Character.GetComponent<CompanionState>().IsBouncing = true;
                Character.GetComponent<NavMeshAgent>().enabled = false;
                StartCoroutine(Character.GetComponent<NavmeshLinkJump>().JumpCurve(Character.transform.position, Lillypad.Trajectory.position, Lillypad.GetComponent<NavMeshLinkCurve>()));
            }

            print("BOUNCEEEE!!!!");
        }

        else if (collision.transform.tag == "BellyFlopLower" && IsSlamming && PlayerStats.CurrentHealth > 0)
        {
            Debug.Log("Hitting the lower target");
            IsSlamming = false;

            BellyFlopLower SlamLower = collision.transform.GetComponent<BellyFlopLower>();

            if (SlamLower.OpenGate && SlamLower.CanOpenGate)
            {
                StartCoroutine(SlamLower.RotateGate());
            }

            SlamLower.Lower();
        }
    }
}
