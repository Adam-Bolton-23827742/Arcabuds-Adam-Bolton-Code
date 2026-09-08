using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class RockChargeAttack : MonoBehaviour
{
    [Header("Attack Variables")]
    public float AttackCoolDown;
    public float AttackTimer;
    public float ChargeAttackDamage;
    public float ChargeTime, ChargeMoveSpeed, ChargeTurnSpeed, WallCheckDistance;
    private float NormalMoveSpeed, NormalTurnSpeed;
    private int EnemiesHit;

    public PlayerMovement PlayerMovement;
    public Transform Player;
    public Rigidbody playerRB;
    public BoxCollider Box;
    public bool isCharging;
    private PlayerInput PlayerInput;
    [SerializeField] private Animator Animator;
    private PlayerStats PlayerStats;
    private GroundCheck GroundCheck;
    [SerializeField] private CooldownUI CooldownUI;
    [SerializeField] private LayerMask WallLayers;

    private void Start()
    {
        PlayerInput = GetComponentInParent<PlayerInput>();
        PlayerStats = GetComponentInParent<PlayerStats>();
        GroundCheck = GetComponentInParent<GroundCheck>();
        NormalMoveSpeed = PlayerMovement.MoveSpeed;
        NormalTurnSpeed = PlayerMovement.TurnSpeed;
        AttackTimer = AttackCoolDown;
    }

    private void Update()
    {
        if (Time.timeScale == 1.0f)
        {
            if (AttackTimer < AttackCoolDown)
            {
                AttackTimer += Time.deltaTime;
                CooldownUI.UpdateFillAmount(1.0f / AttackCoolDown, false);

                if (PlayerStats.CurrentHealth <= 0)
                {
                    AttackTimer = AttackCoolDown;
                    CooldownUI.UpdateFillAmount(1.0f, true);
                }
            }

            if (!GroundCheck.IsGrounded() && isCharging)
            {
                Box.enabled = false;
                isCharging = false;
                PlayerMovement.MoveSpeed = NormalMoveSpeed;
                PlayerMovement.TurnSpeed = NormalTurnSpeed;
                PlayerMovement.EndCharge();
                AttackTimer = AttackCoolDown;
                CooldownUI.UpdateFillAmount(1.0f, false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 1.0f && isCharging)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit Hit, WallCheckDistance, WallLayers))
            {
                print(Hit.transform.name);
                Box.enabled = false;
                isCharging = false;
                PlayerMovement.MoveSpeed = NormalMoveSpeed;
                PlayerMovement.TurnSpeed = NormalTurnSpeed;
                PlayerMovement.EndCharge();
            }
        }
    }

    public void CallAttack()
    {
        if (!isCharging && AttackTimer >= AttackCoolDown)
        {
            StartCoroutine(ChargeAttack());
        }
    }

    public IEnumerator ChargeAttack()
    {
        PlayerMovement.MoveSpeed = ChargeMoveSpeed;
        PlayerMovement.TurnSpeed = ChargeTurnSpeed;
        isCharging = true;
        Box.enabled = true;
        Animator.SetTrigger("ChargeStart");
        Animator.SetBool("Charging", true);
        EnemiesHit = 0;
        AttackTimer = 0.0f;
        CooldownUI.UpdateFillAmount(0.0f, false);
        yield return new WaitForSeconds(ChargeTime);
        if (isCharging)
        {
            Box.enabled = false;
            isCharging = false;
            PlayerMovement.MoveSpeed = NormalMoveSpeed;
            PlayerMovement.TurnSpeed = NormalTurnSpeed;
            PlayerMovement.EndCharge();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyStats>() != null)
        {
            other.GetComponent<EnemyStats>().TakeDamage(ChargeAttackDamage, PlayerInput, PlayerStats.Rumble);
            EnemiesHit++;
            if (EnemiesHit == 3 || other.GetComponent<EliteBossStateMachine>() != null)
            {
                Box.enabled = false;
                isCharging = false;
                PlayerMovement.MoveSpeed = NormalMoveSpeed;
                PlayerMovement.TurnSpeed = NormalTurnSpeed;
                PlayerMovement.EndCharge();
            }
        }
        else if (other.GetComponent<Destructible_Script>() != null)
        {
            if (other.CompareTag("PebblesDestructible"))
            {
                other.GetComponent<Destructible_Script>().TakeDamage(99999);
            }
            else
            {
                other.GetComponent<Destructible_Script>().TakeDamage(ChargeAttackDamage);
            }
        }
        else if (other.GetComponentInChildren<Destructible_Script>() != null)
        {
            if (other.CompareTag("PebblesDestructible"))
            {
                other.GetComponentInChildren<Destructible_Script>().TakeDamage(99999);
            }
            else
            {
                other.GetComponentInChildren<Destructible_Script>().TakeDamage(ChargeAttackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * WallCheckDistance);
    }
}
