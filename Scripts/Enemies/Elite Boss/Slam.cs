using System.Collections;
using UnityEngine;

public class Slam : MonoBehaviour
{
    private bool CanSlam = true;
    [SerializeField] private float SlamDelay, ShockwaveRadius, ShockwaveDamage;
    private Animator Animator;
    [SerializeField] private LayerMask PlayerLayer;
    private EliteBossStateMachine StateMachine;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        StateMachine = GetComponentInParent<EliteBossStateMachine>();
    }

    public void SlamAttack()
    {
        if (CanSlam)
        {
            StartCoroutine(StartSlam());
        }
    }

    private IEnumerator StartSlam()
    {
        CanSlam = false;
        Animator.SetTrigger("GroundSlam");
        yield return new WaitForSeconds(SlamDelay);
        CanSlam = true;
    }

    public void SlamDamage()
    {
        if (StateMachine.StateMachine == EliteBossStateMachine.States.Slam)
        {
            Collider[] Players = Physics.OverlapSphere(transform.position, ShockwaveRadius, PlayerLayer);

            foreach (Collider Player in Players)
            {
                GroundCheck GroundCheck = Player.GetComponent<GroundCheck>();

                if (GroundCheck != null && GroundCheck.IsGrounded())
                {
                    Player.transform.GetComponent<PlayerStats>().TakeDamage(ShockwaveDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ShockwaveRadius);
    }
}
