using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Health Values")]
    public float MaxHealth;
    public float CurrentHealth;
    public float Speed;
    public bool hitByZappySpecial;

    [Header("Adam Changes")]
    private MeleeEnemyStateMachine MeleeEnemyStateMachine;
    private EliteBossStateMachine EliteBossStateMachine;
    private ZappyStateMachine ZappyStateMachine;
    private EnemyFollow EnemyFollow;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip EnemyDamagedAudio, LandSound;
    [SerializeField] private SoundHandler SoundHandler;
    private Animator Animator;
    [SerializeField] private BossHealthBar BossHealth;
    private FlashRed FlashRed;

    private void Start()
    {
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
        CurrentHealth = MaxHealth;
        MeleeEnemyStateMachine = GetComponent<MeleeEnemyStateMachine>();
        EnemyFollow = GetComponent<EnemyFollow>();
        AudioSource = GetComponent<AudioSource>();
        Animator = GetComponentInChildren<Animator>();
        EliteBossStateMachine = GetComponent<EliteBossStateMachine>();
        FlashRed = GetComponent<FlashRed>();
        ZappyStateMachine = GetComponent<ZappyStateMachine>();
    }

    public void TakeDamage(float damage, PlayerInput PlayerInput, Rumble PlayerRumble)
    {
        if (MeleeEnemyStateMachine != null && MeleeEnemyStateMachine.StateMachine != MeleeEnemyStateMachine.States.Dead
            || EliteBossStateMachine != null && EliteBossStateMachine.StateMachine != EliteBossStateMachine.States.Dead
            || ZappyStateMachine != null && ZappyStateMachine.State != ZappyStateMachine.States.Dead)
        {
            CurrentHealth -= damage;
            SoundHandler.PlaySoundEffect(AudioSource, EnemyDamagedAudio, null, transform.position);
            StartCoroutine(FlashRed.Flash());

            if (BossHealth != null)
            {
                BossHealth.UpdateHealthSlider();
            }

            if (MeleeEnemyStateMachine != null)
            {
                if (PlayerInput != null)
                {
                    EnemyFollow.TargetPosition = PlayerInput.transform;
                }
            }

            if (CurrentHealth <= 0)
            {
                Debug.Log("dead");
                GetComponent<Collider>().enabled = false;

                if (BossHealth != null)
                {
                    BossHealth.transform.parent.gameObject.SetActive(false);
                }

                if (PlayerInput != null && PlayerInput.currentControlScheme == "Gamepad" && PlayerInput.gameObject.GetComponent<PlayerStats>().Rumble != null)
                {
                    SoundHandler.RunSoundFromScript = true;
                    Gamepad Gamepad = PlayerInput.devices[0] as Gamepad;
                    StartCoroutine(PlayerInput.gameObject.GetComponent<PlayerStats>().Rumble.VibrateController(Gamepad));
                }

                if (MeleeEnemyStateMachine != null)
                {
                    MeleeEnemyStateMachine.StateMachine = MeleeEnemyStateMachine.States.Dead;
                }

                else if (EliteBossStateMachine != null)
                {
                    EliteBossStateMachine.StateMachine = EliteBossStateMachine.States.Dead;
                }

                else if (ZappyStateMachine != null)
                {
                    ZappyStateMachine.State = ZappyStateMachine.States.Dead;
                    ZappyStateMachine.GetComponent<SplineAnimate>().Pause();
                }
                
                Animator.SetBool("Walking", false);
                Animator.SetTrigger("Death");
            }

            else if (PlayerInput != null && PlayerInput.currentControlScheme == "Gamepad" && PlayerInput.gameObject.GetComponent<PlayerStats>().Rumble != null)
            {
                Gamepad Gamepad = PlayerInput.devices[0] as Gamepad;
                StartCoroutine(PlayerInput.gameObject.GetComponent<PlayerStats>().Rumble.VibrateController(Gamepad));
            }
        }
    }

    public void Slow()
    {
        StartCoroutine(SlowEnemy());
    }

    public IEnumerator SlowEnemy()
    {
        Speed -= 3;
        yield return new WaitForSeconds(3);
        Speed += 3;
    }

    public void CallFunction()
    {
        hitByZappySpecial = true;
        StartCoroutine(RemoveZappyLockout());
    }

    public IEnumerator RemoveZappyLockout()
    {
        yield return new WaitForSeconds(3f);
        hitByZappySpecial = false;
    }
}
