using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class LineDash : MonoBehaviour
{
    [SerializeField] private float DashDelay, MoveToSplineSpeed, DashSpeed, DashDamage, GroundCheckDistance;
    private SplineAnimate SplineAnimate;
    [SerializeField] private SplineContainer SplineToEdge;
    [SerializeField] private AssignControllers AssignControllers;
    private Vector3 TargetPosition, SpawnPosition, GroundPosition;
    private int DashCounter;
    private ZappyStateMachine StateMachine;
    private bool Started, IsDashing;
    private Transform TargetPlayerPosition;
    [SerializeField] private LayerMask GroundLayer;
    private Animator Animator;

    private void Start()
    {
        SplineAnimate = GetComponent<SplineAnimate>();
        SplineAnimate.Completed += SplineAnimate_Completed;
        SpawnPosition = transform.position;
        StateMachine = GetComponent<ZappyStateMachine>();
        Animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (TargetPlayerPosition != null)
        {
            transform.LookAt(TargetPlayerPosition);
        }
    }

    public IEnumerator InitiateDashPosition()
    {
        if (!Started)
        {
            Started = true;

            while (Vector3.Distance(transform.position, SplineToEdge.transform.position) >= 0.05f)
            {
                MoveToSpline();
                yield return null;
            }

            SplineAnimate.Container = SplineToEdge;
            SplineAnimate.Play();
        }
    }

    private IEnumerator Dashes()
    {
        if (PlayerConfigurationManager.PlayerConfigurationList.Count > 1)
        {
            TargetPlayerPosition = AssignControllers.PlayerInputs[Random.Range(0, AssignControllers.PlayerInputs.Count)].transform;
        }

        else
        {
            foreach (PlayerInput Player in AssignControllers.PlayerInputs)
            {
                if (!Player.transform.GetComponent<CompanionState>().enabled)
                {
                    TargetPlayerPosition = Player.transform;
                    break;
                }
            }
        }

        yield return new WaitForSeconds(DashDelay);
        StartCoroutine(DashAttack());
    }

    private void MoveToSpline()
    {
        transform.LookAt(SplineToEdge.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, SplineToEdge.transform.position, MoveToSplineSpeed * Time.deltaTime);
    }

    private IEnumerator DashAttack()
    {
        DashCounter++;
        Vector3 DirectionToTarget = (TargetPlayerPosition.position - transform.position).normalized;
        TargetPosition = transform.position + DirectionToTarget * 10000000f;
        TargetPlayerPosition = null;
        IsDashing = true;
        Animator.SetTrigger("LineDash");
        yield return new WaitForSeconds(0.5f);

        while (IsDashing)
        {
            if (Physics.Raycast(transform.position, Vector3.down, GroundCheckDistance, GroundLayer))
            {
                GroundPosition = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, DashSpeed * Time.deltaTime);
            }

            else
            {
                transform.position = GroundPosition;
                break;
            }
            
            yield return null;
        }

        IsDashing = false;

        if (DashCounter < 3)
        {
            StartCoroutine(Dashes());
        }

        else
        {
            DashCounter = 0;
            StartCoroutine(MoveBackToSpawn());
        }
    }

    private void SplineAnimate_Completed()
    {
        Vector3 NewPosition = transform.position;
        SplineAnimate.Restart(false);
        SplineAnimate.Container = null;
        transform.position = NewPosition;
        StartCoroutine(Dashes());
    }

    private IEnumerator MoveBackToSpawn()
    {
        while (Vector3.Distance(transform.position, SpawnPosition) >= 0.05f)
        {
            transform.LookAt(SpawnPosition);
            transform.position = Vector3.MoveTowards(transform.position, SpawnPosition, MoveToSplineSpeed * Time.deltaTime);
            yield return null;
        }

        StateMachine.BossStateChange(ZappyStateMachine.States.Projectile);
        Started = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * GroundCheckDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsDashing)
        {
            if (collision.transform.tag == "Player")
            {
                collision.transform.GetComponent<PlayerStats>().TakeDamage(DashDamage);
            }
        }
    }
}
