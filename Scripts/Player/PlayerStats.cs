using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("PlayerStats")]
    public float MaxHealth;
    public float CurrentHealth;
    public Image[] HeartImages;
    public Image StarImage;
    
    [Header("AdamChanges")]
    public int CollectableCount;
    private PlayerInput PlayerInput;
    public Rumble Rumble;
    public int PlayerNumber;
    public RespawnHandler RespawnHandler;
    [HideInInspector] private AudioSource AudioSource;
    [SerializeField] private AudioClip PlayerDamagedAudio, HurtAudio;
    public SoundHandler SoundHandler;
    private CompanionState CompanionState;
    private NavMeshAgent NavMeshAgent;
    public Sprite HappyPortrait, HurtPortrait, DeadPortrait;
    public Image CharacterPortrait;
    [SerializeField] private float PortraitChangeTimer;
    public TextMeshProUGUI CollectableCountText;
    public Transform Profile, Points, HeartsBackground;
    private Animator Animator;
    private FlashRed FlashRed;

    void Start()
    {
        CurrentHealth = MaxHealth;
        PlayerInput = GetComponent<PlayerInput>();
        AudioSource = GetComponent<AudioSource>();
        CompanionState = GetComponent<CompanionState>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();
        FlashRed = GetComponent<FlashRed>();
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth >= 0)
        {
            CurrentHealth -= damage;
            UpdateHealthVisuals(CurrentHealth);
            SoundHandler.PlaySoundEffect(AudioSource, PlayerDamagedAudio, HurtAudio, transform.position);
            StartCoroutine(FlashRed.Flash());

            if (CurrentHealth <= 0)
            {
                if (Animator != null)
                {
                    Animator.SetBool("Walking", false);
                    Animator.SetBool("Falling", false);
                    Animator.SetBool("JumpingBool", false);
                    Animator.SetTrigger("Death");
                }

                if (PlayerInput.currentControlScheme == "Gamepad")
                {
                    Gamepad Gamepad = PlayerInput.devices[0] as Gamepad;
                    StartCoroutine(Rumble.VibrateController(Gamepad));
                }

                NavMeshAgent.enabled = false;

                if (RespawnHandler.Player1 == gameObject)
                {
                    RespawnHandler.StartRespawn1 = true;
                }

                else if (RespawnHandler.Player2 == gameObject)
                {
                    RespawnHandler.StartRespawn2 = true;
                }

                else if (RespawnHandler.Player3 == gameObject)
                {
                    RespawnHandler.StartRespawn3 = true;
                }
            }

            else
            {
                if (PlayerInput.currentControlScheme == "Gamepad")
                {
                    Gamepad Gamepad = PlayerInput.devices[0] as Gamepad;
                    StartCoroutine(Rumble.VibrateController(Gamepad));
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground" && CurrentHealth > 0 || LayerMask.LayerToName(collision.gameObject.layer) == "Lilypad" && CurrentHealth > 0)
        {
            CompanionState.IsBouncing = false;

            if (CompanionState.enabled)
            {
                NavMeshAgent.enabled = true;
            }
        }
    }

    public void UpdateHealthVisuals(float health)
    {
        if (health == 6)
        {
            StartCoroutine(ChangePortrait(HurtPortrait));

            HeartImages[0].fillAmount = 1f;
            HeartImages[1].fillAmount = 1f;
            HeartImages[2].fillAmount = 1f;
        }
        else if (health == 5)
        {
            StartCoroutine(ChangePortrait(HurtPortrait));

            HeartImages[0].fillAmount = 0.5f;
            HeartImages[1].fillAmount = 1f;
            HeartImages[2].fillAmount = 1f;
        }
        else if (health == 4)
        {
            StartCoroutine(ChangePortrait(HurtPortrait));

            HeartImages[0].fillAmount = 0f;
            HeartImages[1].fillAmount = 1f;
            HeartImages[2].fillAmount = 1f;
        }
        else if (health == 3)
        {
            StartCoroutine(ChangePortrait(HurtPortrait));
            HeartImages[0].fillAmount = 0f;
            HeartImages[1].fillAmount = 0.5f;
            HeartImages[2].fillAmount = 1f;
        }
        else if (health == 2)
        {
            CharacterPortrait.sprite = HurtPortrait;
            HeartImages[0].fillAmount = 0f;
            HeartImages[1].fillAmount = 0f;
            HeartImages[2].fillAmount = 1f;
        }
        else if (health == 1)
        {
            CharacterPortrait.sprite = HurtPortrait;
            HeartImages[0].fillAmount = 0f;
            HeartImages[1].fillAmount = 0f;
            HeartImages[2].fillAmount = 0.5f;
        }
        else if (health <= 0)
        {
            CharacterPortrait.sprite = DeadPortrait;
            HeartImages[0].fillAmount = 0f;
            HeartImages[1].fillAmount = 0f;
            HeartImages[2].fillAmount = 0f;
        }

        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(FlashHearts(HeartImages[i]));
        }
    }

    private IEnumerator ChangePortrait(Sprite Sprite)
    {
        CharacterPortrait.sprite = Sprite;
        yield return new WaitForSeconds(PortraitChangeTimer);
        CharacterPortrait.sprite = HappyPortrait;
    }

    private IEnumerator FlashHearts(Image Heart)
    {
        for (int i = 0; i < 3; i++)
        {
            Heart.color = new Color32(255, 255, 255, 150); //Not full color
            yield return new WaitForSeconds(0.5f);
            Heart.color = new Color32(255, 255, 255, 255); //Full color
            yield return new WaitForSeconds(0.5f);
        }
        Heart.color = new Color32(255, 255, 255, 255);
    }

    public void FlashStar()
    {
        StartCoroutine(FlashStars(StarImage));
    }

    private IEnumerator FlashStars(Image Star)
    {
        for (int i = 0; i < 3; i++)
        {
            Star.color = new Color32(255, 255, 255, 150); //Not full color
            yield return new WaitForSeconds(0.5f);
            Star.color = new Color32(255, 255, 255, 255); //Full color
            yield return new WaitForSeconds(0.5f);
        }
        Star.color = new Color32(255, 255, 255, 255);
    }
}