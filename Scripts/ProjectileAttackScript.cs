using System.Collections;
using UnityEngine;

public class ProjectileAttackScript : MonoBehaviour
{
    [Header("Attack Variables")]
    public float AttackCoolDown;
    public float AttackTimer;
    public bool canAttack;
    public Transform ProjectileSpawn;
    public GameObject Projectile;
    private Animator Animator;
    private int Counter, ProjectileCount;
    private ZappyStateMachine ZappyStateMachine;
    private EliteBossStateMachine EliteBossStateMachine;
    private SoundHandler SoundHandler;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip ProjectileSound;

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        ZappyStateMachine = GetComponent<ZappyStateMachine>();
        EliteBossStateMachine = GetComponent<EliteBossStateMachine>();
        AudioSource = GetComponent<AudioSource>();
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
    }

    public void Update()
    {
        if (AttackTimer > 0)
        {
            AttackTimer -= Time.deltaTime;
        }

        if (AttackTimer <= 0)
        {
            canAttack = true;
        }
    }

    public IEnumerator CallAttack()
    {
        Debug.Log("Attack is being called");

        if (Counter == 0)
        {
            ProjectileCount = Random.Range(4, 8);
        }

        if (canAttack && Counter < ProjectileCount)
        {
            canAttack = false;

            if (EliteBossStateMachine != null)
            {
                Animator.SetTrigger("RangedShoot1");
            }

            else
            {
                Animator.SetTrigger("Attacking");
            }

            Counter++;

            if (SoundHandler != null && ProjectileSound != null)
            {
                SoundHandler.PlaySoundEffect(AudioSource, ProjectileSound, null, transform.position);
            }

            Instantiate(Projectile, ProjectileSpawn.position, transform.rotation);
            //transform.DetachChildren();
            AttackTimer = AttackCoolDown;
        }

        else if (Counter == ProjectileCount)
        {
            Counter = 0;

            yield return new WaitForSeconds(2.0f);
            
            if (EliteBossStateMachine != null)
            {
                StartCoroutine(EliteBossStateMachine.BossStateChange(EliteBossStateMachine.States.Follow, 0.0f));
            }
            
            else if (ZappyStateMachine != null)
            {
                ZappyStateMachine.BossStateChange(ZappyStateMachine.States.Flying);
            }
        }
    }
}
