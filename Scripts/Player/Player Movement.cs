using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody Rigidbody;
    public float MoveSpeed, TurnSpeed, JumpForce, MaxCharacterDistance;
    private Vector3 CamForward, CamRight, MoveDirection, Position;
    [SerializeField] private Vector3 GroundCheckOffset;
    [SerializeField] private bool CanDoubleJump;
    private int JumpCount;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip JumpSound, LandSound;
    public SoundHandler SoundHandler;
    private CharacterSwitch CharacterSwitch;
    private GroundCheck GroundCheck;
    private Animator PlayerAnimator;
    private PlayerStats PlayerStats;
    private Vector2 MoveInputVector;
    [HideInInspector] public bool CanPlayFall = true;

    [Header("TomChanges")]
    [SerializeField] private RockChargeAttack RCA;
    //[SerializeField] private float GlobalMoveVectorX;
    //[SerializeField] private Camera RockCam;
    private bool isMoving;
    public bool inDialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        CharacterSwitch = GetComponent<CharacterSwitch>();
        GroundCheck = GetComponent<GroundCheck>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerStats = GetComponent<PlayerStats>();

        if (GetComponentInChildren<RockChargeAttack>() != null)
        {
            RCA = GetComponentInChildren<RockChargeAttack>();
        }
        else
        {
            RCA = null;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerStats.CurrentHealth > 0)
        {
            transform.forward = Vector3.RotateTowards(transform.forward, MoveDirection, TurnSpeed * Time.fixedDeltaTime, 0.0f);

            if (RCA != null && RCA.isCharging)
            {
                MoveDirection = transform.forward;
            }

            Rigidbody.linearVelocity = new Vector3(MoveDirection.x * MoveSpeed, Rigidbody.linearVelocity.y, MoveDirection.z * MoveSpeed);

            if (PlayerAnimator != null && (RCA == null || !RCA.isCharging))
            {
                if (GroundCheck.IsGrounded() && MoveDirection != Vector3.zero && (RCA == null || !RCA.isCharging))
                {
                    PlayerAnimator.SetBool("Walking", true);
                }

                else if (GroundCheck.IsGrounded() && MoveDirection == Vector3.zero)
                {
                    isMoving = false;
                    PlayerAnimator.SetBool("Walking", false);
                }

                if (Rigidbody.linearVelocity.y <= 0.0f && !GroundCheck.IsGrounded() && CanPlayFall)
                {
                    PlayerAnimator.SetBool("Walking", false);
                    PlayerAnimator.SetBool("Falling", true);
                }
            }
        }
    }

    private void Update()
    {
        if (PlayerStats.CurrentHealth > 0)
        {
            if (Vector3.Distance(CharacterSwitch.SwitchableCharacters[0].transform.position, CharacterSwitch.SwitchableCharacters[1].transform.position) < MaxCharacterDistance)
            {
                Position = transform.position;
            }

            else if (Position != Vector3.zero)
            {
                transform.position = Position;
            }

            GroundCheck.PlayLandSound(AudioSource, LandSound);

            if (Rigidbody != null)
            {
                CamForward = Camera.main.transform.forward;
                CamRight = Camera.main.transform.right;

                CamForward.y = 0.0f;
                CamRight.y = 0.0f;

                CamForward.Normalize();
                CamRight.Normalize();

                MoveDirection = CamForward * MoveInputVector.y + CamRight * MoveInputVector.x;
                MoveDirection.Normalize();
            }
        }
    }

    //public void Charge()
    //{
    //    if (PlayerStats.CurrentHealth > 0)
    //    {
    //        Vector2 MoveInputVector = new Vector2(GlobalMoveVectorX, 1f);

    //        CamForward = RockCam.transform.forward;
    //        CamRight = RockCam.transform.right;

    //        CamForward.y = 0.0f;
    //        CamRight.y = 0.0f;

    //        CamForward.Normalize();
    //        CamRight.Normalize();

    //        MoveDirection = CamForward * MoveInputVector.y + CamRight * MoveInputVector.x;
    //        MoveDirection.Normalize();

    //        Rigidbody.linearVelocity = new Vector3(MoveDirection.x * MoveSpeed, Rigidbody.linearVelocity.y, MoveDirection.z * MoveSpeed);
    //    }
    //}

    public void EndCharge()
    {
        if (PlayerStats.CurrentHealth > 0)
        {
            if (!isMoving)
            {
                MoveDirection = Vector3.zero;
            }
            Rigidbody.linearVelocity = Vector3.zero;
            PlayerAnimator.SetBool("Charging", false);
            if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("ChargeLoop") || PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("ChargeEnd"))
            {
                
            }
        }
    }

    public void OnMove(InputValue Value)
    {
        isMoving = true;
        MoveInputVector = Value.Get<Vector2>();

        if (Keyboard.current.wKey.wasPressedThisFrame && Keyboard.current.sKey.isPressed)
        {
            MoveInputVector.y = 1.0f;
        }

        else if (Keyboard.current.sKey.wasPressedThisFrame && Keyboard.current.wKey.isPressed)
        {
            MoveInputVector.y = -1.0f;
        }

        if (Keyboard.current.dKey.wasPressedThisFrame && Keyboard.current.aKey.isPressed)
        {
            MoveInputVector.x = 1.0f;
        }

        else if (Keyboard.current.aKey.wasPressedThisFrame && Keyboard.current.dKey.isPressed)
        {
            MoveInputVector.x = -1.0f;
        }
    }

    private void OnJump()
    {
        if (Time.timeScale != 0.0f)
        {
            if (RCA != null && !RCA.isCharging || RCA == null && name == "Splashy"|| RCA == null && name == "Zappy")
            {
                if (CanDoubleJump)
                {
                    JumpCount++;

                    if (GroundCheck.IsGrounded())
                    {
                        JumpCount = 0;
                    }
                }

                if (CanDoubleJump && JumpCount < 2 || GroundCheck.IsGrounded())
                {
                    Jump();
                }
            }
        }
    }

    private void Jump()
    {
        if (inDialogue == false)
        {
            if (PlayerAnimator != null)
            {
                StartCoroutine(JumpAnim());
            }

            Rigidbody.linearVelocity = new Vector3(Rigidbody.linearVelocity.x, 0.0f, Rigidbody.linearVelocity.z);
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            SoundHandler.PlaySoundEffect(AudioSource, JumpSound, null, transform.position);
        }
    }

    private IEnumerator JumpAnim()
    {
        PlayerAnimator.SetBool("Walking", false);
        PlayerAnimator.SetBool("JumpingBool", true);
        yield return new WaitForEndOfFrame();
        PlayerAnimator.SetBool("JumpingBool", false);
    }

    public void StartDialogueWait()
    {
        StartCoroutine(WaitForDialogue());
    }

    public IEnumerator WaitForDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        inDialogue = false;
    }
}
