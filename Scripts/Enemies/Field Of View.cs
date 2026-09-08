using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float ViewRadius, ViewAngle;
    [SerializeField] private LayerMask ObstacleLayers, PlayerLayers;
    private Vector3 DirectionToPlayer;
    [SerializeField] private bool ShowFOVEditor;
    private float DistanceToPlayer;
    private Alert Alert;

    private void Start()
    {
        Alert = GetComponent<Alert>();
    }

    public void SearchForPlayer()
    {
        DistanceToPlayer = 0;

        Collider[] PlayerColliders = Physics.OverlapSphere(transform.position, ViewRadius, PlayerLayers);

        foreach (Collider Player in PlayerColliders)
        {
            DirectionToPlayer = (Player.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, DirectionToPlayer) < ViewAngle / 2 && Player.GetComponent<PlayerStats>() != null && Player.GetComponent<PlayerStats>().CurrentHealth > 0)
            {
                DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
                print("InCone");
                if (!Physics.Raycast(transform.position, DirectionToPlayer, DistanceToPlayer, ObstacleLayers))
                {
                    StartCoroutine(Alert.StartAlert(Player));
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (ShowFOVEditor)
        {
            Gizmos.DrawWireSphere(transform.position, ViewRadius);

            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, ViewAngle / 2, 0) * transform.forward * ViewRadius);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -ViewAngle / 2, 0) * transform.forward * ViewRadius);

            Gizmos.DrawLine(transform.position, transform.position + DirectionToPlayer * DistanceToPlayer);
        }
    }
}
