using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterBaseAttack : MonoBehaviour
{
    [Header("Attack Variables")]
    public float AttackCoolDown;
    public float AttackTimer;
    public float AttackDamage;
    public bool canAttack;

    [Header("AdamChanges")]
    private PlayerInput PlayerInput;
    private PlayerStats PlayerStats;
    [SerializeField] public bool Attacking;
    private List<EnemyStats> EnemiesToDamage;
    [SerializeField] private CooldownUI CooldownUI;

    private void Start()
    {
        PlayerInput = GetComponentInParent<PlayerInput>();
        PlayerStats = GetComponentInParent<PlayerStats>();
        EnemiesToDamage = new List<EnemyStats>();
        AttackTimer = 0.0f;
    }

    public void Update()
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

            if (AttackTimer >= AttackCoolDown)
            {
                canAttack = true;

                if (Attacking)
                {
                    Attacking = false;
                    EnemiesToDamage.Clear();
                }
            }
        }
    }

    public void CallAttack()
    {
        if (canAttack)
        {
            Debug.Log("Attack is being called");
            canAttack = false;
            Attacking = true;
            AttackTimer = 0.0f;
            CooldownUI.UpdateFillAmount(0.0f, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Attacking)
        {
            if (other.tag == "Enemy" && !EnemiesToDamage.Contains(other.GetComponent<EnemyStats>()))
            {
                other.GetComponent<EnemyStats>().TakeDamage(AttackDamage, PlayerInput, PlayerStats.Rumble);
                EnemiesToDamage.Add(other.GetComponent<EnemyStats>());
            }

            if (other.GetComponent<Destructible_Script>() != null && other.tag != "PebblesDestructible")
            {
                other.GetComponent<Destructible_Script>().TakeDamage(AttackDamage);
            }
        }
    }
}
