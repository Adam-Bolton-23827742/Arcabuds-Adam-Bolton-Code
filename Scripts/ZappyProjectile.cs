using UnityEngine;
using UnityEngine.InputSystem;

public class ZappyProjectile : MonoBehaviour
{
    public Transform EnemyPos;
    private Rigidbody rb;
    [SerializeField] private float AttackDamage;
    public float ProjectileSpeed;
    public float LifeTime;
    private bool hasEnemyPos;
    private SoundHandler SoundHandler;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip ProjectileSound;
    [HideInInspector] public GameObject Char;

    [Header("Attack needs")]
    public PlayerInput PlayerInput;
    public PlayerStats PlayerStats;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        PlayerInput = GetComponentInParent<PlayerInput>();
        PlayerStats = GetComponentInParent<PlayerStats>();

        if (Char.GetComponentInChildren<ElectricBaseAttack>().EnemyPos != null)
        {
            EnemyPos = Char.GetComponentInChildren<ElectricBaseAttack>().EnemyPos;
        }

        transform.parent = null;

        if (EnemyPos == null)
        {
            rb.AddForce(transform.forward * ProjectileSpeed, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(EnemyPos.position - transform.position, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        if (LifeTime > 0)
        {
            LifeTime -= Time.deltaTime;
        }

        if (LifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyStats>().TakeDamage(AttackDamage, PlayerInput, null);
            }

            else if (other.tag != "PebblesDestructible" && other.GetComponent<Destructible_Script>() != null)
            {
                other.GetComponent<Destructible_Script>().TakeDamage(AttackDamage);
            }

            SoundHandler.PlaySoundEffect(AudioSource, ProjectileSound, null, transform.position);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 100);
    }
}
