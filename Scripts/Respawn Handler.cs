using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class RespawnHandler : MonoBehaviour
{
    [SerializeField] private float RespawnDelay;
    [HideInInspector] public bool StartRespawn1, StartRespawn2, StartRespawn3, IsGameOver;
    [HideInInspector] public Vector3 SpawnPoint;
    private Vector3 IndividualSpawnPoint1, IndividualSpawnPoint2, IndividualSpawnPoint3;
    public Transform SpawnPoint1, SpawnPoint2, SpawnPoint3;
    public GameObject Player1, Player2, Player3;
    [HideInInspector] public Transform RecentCheckpoint;
    [HideInInspector] public PlayerStats Stats1, Stats2, Stats3;
    private InputDevice InputDevice;
    private bool CanSetCheckpoint = true;
    [HideInInspector] public GroundCheck GroundCheck1, GroundCheck2, GroundCheck3;
    public bool IndividualSpawn;

    [Header("Tom Changes")]
    [SerializeField] private Image GameOverBackground;
    [SerializeField] private Sprite[] GameOverImageStorage;
    [SerializeField] private Image GameOverSprite;

    private void Start()
    {
        Stats1 = Player1.GetComponent<PlayerStats>();
        Stats2 = Player2.GetComponent<PlayerStats>();
        GroundCheck1 = Player1.GetComponent<GroundCheck>();
        GroundCheck2 = Player2.GetComponent<GroundCheck>();

        if (Player3 != null)
        {
            Stats3 = Player3.GetComponent<PlayerStats>();
            GroundCheck3 = Player3.GetComponent<GroundCheck>();
        }
    }

    private void Update()
    {
        if (IndividualSpawn)
        {
            if (CanSetCheckpoint)
            {
                StartCoroutine(SetCheckpoint());
            }

            if (Stats1.CurrentHealth <= 0 && Stats2.CurrentHealth <= 0 && !IsGameOver && PlayerConfigurationManager.PlayerConfigurationList != null && PlayerConfigurationManager.PlayerConfigurationList.Count > 0)
            {
                if (Player3 != null && Stats3.CurrentHealth <= 0)
                {
                    StartRespawn1 = false;
                    StartRespawn2 = false;
                    StartRespawn3 = false;
                    StopAllCoroutines();
                    CanSetCheckpoint = true;
                    StartCoroutine(GameOver());
                }

                else if (Player3 == null)
                {
                    StartRespawn1 = false;
                    StartRespawn2 = false;
                    StopAllCoroutines();
                    CanSetCheckpoint = true;
                    StartCoroutine(GameOver());
                }
            }

            else if (StartRespawn1)
            {
                RespawnPlayer1(IndividualSpawnPoint1, null);
            }

            else if (StartRespawn2)
            {
                RespawnPlayer2(IndividualSpawnPoint2, null);
            }

            else if (StartRespawn3 && Player3 != null)
            {
                RespawnPlayer3(IndividualSpawnPoint3, null);
            }
        }

        else
        {
            if (Stats1.CurrentHealth <= 0 && Stats2.CurrentHealth <= 0 && !IsGameOver && PlayerConfigurationManager.PlayerConfigurationList != null && PlayerConfigurationManager.PlayerConfigurationList.Count > 0)
            {
                if (Player3 != null && Stats3.CurrentHealth <= 0)
                {
                    StartRespawn1 = false;
                    StartRespawn2 = false;
                    StartRespawn3 = false;
                    StopAllCoroutines();
                    StartCoroutine(GameOver());
                }

                else if (Player3 == null)
                {
                    StartRespawn1 = false;
                    StartRespawn2 = false;
                    StopAllCoroutines();
                    StartCoroutine(GameOver());
                }
            }

            else if (StartRespawn1)
            {
                if (Stats2.CurrentHealth > 0)
                {
                    RespawnPlayer1(SpawnPoint2.position, SpawnPoint2);
                }

                else if (Player3 != null && Stats3.CurrentHealth > 0)
                {
                    RespawnPlayer1(SpawnPoint3.position, SpawnPoint3);
                }
            }

            else if (StartRespawn2)
            {
                if (Stats1.CurrentHealth > 0)
                {
                    RespawnPlayer2(SpawnPoint1.position, SpawnPoint1);
                }

                else if (Player3 != null && Stats3.CurrentHealth > 0)
                {
                    RespawnPlayer2(SpawnPoint3.position, SpawnPoint3);
                }
            }

            else if (StartRespawn3)
            {
                if (Stats1.CurrentHealth > 0)
                {
                    RespawnPlayer3(SpawnPoint1.position, SpawnPoint1);
                }

                else if (Stats2.CurrentHealth > 0)
                {
                    RespawnPlayer3(SpawnPoint2.position, SpawnPoint2);
                }
            }
        }
    }

    public void RespawnPlayer1(Vector3 SpawnVector, Transform SpawnTransform)
    {
        StartRespawn1 = false;
        StartCoroutine(Respawn(Player1, SpawnVector, SpawnTransform, Stats1));
    }

    public void RespawnPlayer2(Vector3 SpawnVector, Transform SpawnTransform)
    {
        StartRespawn2 = false;
        StartCoroutine(Respawn(Player2, SpawnVector, SpawnTransform, Stats2));
    }

    public void RespawnPlayer3(Vector3 SpawnVector, Transform SpawnTransform)
    {
        StartRespawn3 = false;
        StartCoroutine(Respawn(Player3, SpawnVector, SpawnTransform, Stats3));
    }

    private IEnumerator Respawn(GameObject Player, Vector3 SpawnVector, Transform SpawnTransform, PlayerStats Stats)
    {
        if (PlayerConfigurationManager.PlayerConfigurationList.Count < 2)
        {
            if (!Player.GetComponent<CompanionState>().enabled)
            {
                Player.GetComponent<CharacterSwitch>().OnNext();
            }
        }

        yield return new WaitForSeconds(RespawnDelay);

        Animator Anim = Player.GetComponentInChildren<Animator>();

        if (Anim != null)
        {
            Anim.SetTrigger("Respawn");
        }
        
        if (SpawnTransform != null)
        {
            Player.transform.position = SpawnTransform.position;
        }

        else
        {
            Player.transform.position = SpawnVector;
        }

        Stats.CurrentHealth = Stats.MaxHealth;
        Stats.UpdateHealthVisuals(Stats.CurrentHealth);
        Stats.CharacterPortrait.sprite = Stats.HappyPortrait;
    }

    public IEnumerator GameOver()
    {
        IsGameOver = true;
        print("Game over");


        StartCoroutine(GameOverImageFade(GameOverBackground));

        PlayerInput Input1 = Player1.GetComponent<PlayerInput>();
        PlayerInput Input2 = Player2.GetComponent<PlayerInput>();
        PlayerInput Input3 = null;

        if (Player3 != null)
        {
            Input3 = Player3.GetComponent<PlayerInput>();
        }

        if (!Player1.GetComponent<CompanionState>().enabled)
        {
            InputDevice = Input1.devices[0];

            if (InputDevice is Keyboard)
            {
                Input1.SwitchCurrentControlScheme("Keyboard&Mouse");
                InputUser.PerformPairingWithDevice(InputDevice, Input1.user);
                InputUser.PerformPairingWithDevice(Mouse.current, Input1.user);
            }

            else if (InputDevice is Gamepad)
            {
                Input1.SwitchCurrentControlScheme(InputDevice);
                InputUser.PerformPairingWithDevice(InputDevice, Input1.user);
            }
        }

        if (!Player2.GetComponent<CompanionState>().enabled)
        {
            InputDevice = Input2.devices[0];

            if (InputDevice is Keyboard)
            {
                Input2.SwitchCurrentControlScheme("Keyboard&Mouse");
                InputUser.PerformPairingWithDevice(InputDevice, Input2.user);
                InputUser.PerformPairingWithDevice(Mouse.current, Input2.user);
            }

            else if (InputDevice is Gamepad)
            {
                Input2.SwitchCurrentControlScheme(InputDevice);
                InputUser.PerformPairingWithDevice(InputDevice, Input2.user);
            }
        }

        if (Player3 != null && !Player3.GetComponent<CompanionState>().enabled)
        {
            InputDevice = Input3.devices[0];

            if (InputDevice is Keyboard)
            {
                Input3.SwitchCurrentControlScheme("Keyboard&Mouse");
                InputUser.PerformPairingWithDevice(InputDevice, Input3.user);
                InputUser.PerformPairingWithDevice(Mouse.current, Input3.user);
            }

            else if (InputDevice is Gamepad)
            {
                Input2.SwitchCurrentControlScheme(InputDevice);
                InputUser.PerformPairingWithDevice(InputDevice, Input3.user);
            }
        }

        yield return new WaitForSeconds(RespawnDelay);

        Animator Anim1 = Player1.GetComponentInChildren<Animator>();
        Animator Anim2 = Player2.GetComponentInChildren<Animator>();
        Animator Anim3 = null;

        if (Player3 != null)
        {
            Anim3 = Player3.GetComponentInChildren<Animator>();
        }

        if (Anim1 != null)
        {
            Anim1.SetTrigger("Respawn");
        }

        if (Anim2 != null)
        {
            Anim2.SetTrigger("Respawn");
        }

        if (Anim3 != null)
        {
            Anim3.SetTrigger("Respawn");
        }

        Stats1.CurrentHealth = Stats1.MaxHealth;
        Stats2.CurrentHealth = Stats2.MaxHealth;
        Stats1.UpdateHealthVisuals(Stats1.CurrentHealth);
        Stats2.UpdateHealthVisuals(Stats2.CurrentHealth);
        Stats1.CharacterPortrait.sprite = Stats1.HappyPortrait;
        Stats2.CharacterPortrait.sprite = Stats2.HappyPortrait;

        if (Player3 != null)
        {
            Stats3.CurrentHealth = Stats3.MaxHealth;
            Stats3.UpdateHealthVisuals(Stats3.CurrentHealth);
            Stats3.CharacterPortrait.sprite = Stats3.HappyPortrait;
        }

        Player1.transform.position = RecentCheckpoint.position;
        Player2.transform.position = RecentCheckpoint.position;

        if (Player3 != null)
        {
            Player3.transform.position = RecentCheckpoint.position;
        }

        InputDevice = null;
        IsGameOver = false;
    }

    private IEnumerator SetCheckpoint()
    {
        CanSetCheckpoint = false;

        if (GroundCheck1.IsGrounded() && Stats1.CurrentHealth > 0)
        {
            IndividualSpawnPoint1 = new Vector3(Player1.transform.position.x, Player1.transform.position.y + 5.0f, Player1.transform.position.z);
        }

        if (GroundCheck2.IsGrounded() && Stats2.CurrentHealth > 0)
        {
            IndividualSpawnPoint2 = new Vector3(Player2.transform.position.x, Player2.transform.position.y + 5.0f, Player2.transform.position.z);
        }

        if (Player3 != null && GroundCheck3.IsGrounded() && Stats3.CurrentHealth > 0)
        {
            IndividualSpawnPoint3 = new Vector3(Player3.transform.position.x, Player3.transform.position.y + 5.0f, Player3.transform.position.z);
        }

        yield return new WaitForSeconds(3.0f);
        CanSetCheckpoint = true;
    }

    private IEnumerator GameOverImageFade(Image GameOverImage)
    {
        GameOverImage.color = new Color(GameOverImage.color.r, GameOverImage.color.g, GameOverImage.color.b, 0);

        while (GameOverImage.color.a < 1.0f)
        {
            GameOverImage.color = new Color(GameOverImage.color.r, GameOverImage.color.g, GameOverImage.color.b, GameOverImage.color.a + (Time.deltaTime / 1.5f));
            yield return null;
        }

        StartCoroutine(SpawnSprite());
    }

    private IEnumerator SpawnSprite()
    {
        int RandomGameOver = Random.Range(0, 2);
        GameOverSprite.sprite = GameOverImageStorage[RandomGameOver];
        GameOverSprite.enabled = true;
        while (GameOverSprite.color.a < 1.0f)
        {
            GameOverSprite.color = new Color(GameOverSprite.color.r, GameOverSprite.color.g, GameOverSprite.color.b, GameOverSprite.color.a + (Time.deltaTime / 0.75f));
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (GameOverSprite.color.a > 0.0f)
        {
            GameOverSprite.color = new Color(GameOverSprite.color.r, GameOverSprite.color.g, GameOverSprite.color.b, GameOverSprite.color.a - (Time.deltaTime / 0.75f));
            yield return null;
        }
        GameOverSprite.enabled = false;

        StartCoroutine(GameOverImageFadeOut(GameOverBackground));
    }

    private IEnumerator GameOverImageFadeOut(Image GameOverImage)
    {
        while (GameOverImage.color.a > 0.0f)
        {
            GameOverImage.color = new Color(GameOverImage.color.r, GameOverImage.color.g, GameOverImage.color.b, GameOverImage.color.a - (Time.deltaTime / 1.5f));
            yield return null;
        }
    }
}
