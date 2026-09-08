using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float GroundCheckRadius, GroundCheckDistance;
    [SerializeField] private LayerMask GroundLayers;
    [HideInInspector] public string GroundLayerName;
    private bool CanPlayLandSound;
    public SoundHandler SoundHandler;
    private Animator Animator;
    private PlayerMovement PlayerMovement;

    private void Start()
    {
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
        Animator = GetComponentInChildren<Animator>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    public bool IsGrounded()
    {
        if (Physics.SphereCast(transform.position, GroundCheckRadius, -transform.up, out RaycastHit HitInfo, GroundCheckDistance, GroundLayers))
        {
            GroundLayerName = LayerMask.LayerToName(HitInfo.transform.gameObject.layer);
            return true;
        }

        else
        {
            return false;
        }
    }

    public void PlayLandSound(AudioSource AudioSource, AudioClip LandSound)
    {
        if (IsGrounded())
        {
            if (CanPlayLandSound)
            {
                CanPlayLandSound = false;
                SoundHandler.PlaySoundEffect(AudioSource, LandSound, null, transform.position);

                if (Animator != null && PlayerMovement != null)
                {
                    if (PlayerMovement.CanPlayFall)
                    {
                        Animator.SetBool("Falling", false);
                        Animator.SetTrigger("Landing");
                    }

                    else
                    {
                        PlayerMovement.CanPlayFall = true;
                    }
                }
            }
        }

        else
        {
            CanPlayLandSound = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, GroundCheckRadius);
        Gizmos.DrawLine(transform.position, transform.position - transform.up * GroundCheckDistance);
    }
}
