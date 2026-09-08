using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshLinkJump : MonoBehaviour
{
    private NavMeshAgent Agent;
    private float Timer;
    private bool IsRunning;
    public SoundHandler SoundHandler;
    [SerializeField] private AudioClip JumpSound;
    private AudioSource AudioSource;
    private Animator Animator;
    private CompanionState CompanionState;

    private void Start()
    {
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
        Agent = GetComponent<NavMeshAgent>();
        AudioSource = GetComponent<AudioSource>();
        Animator = GetComponentInChildren<Animator>();
        CompanionState = GetComponent<CompanionState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Agent.isOnOffMeshLink && !IsRunning)
        {
            StartCoroutine(JumpCurve(transform.position, Agent.currentOffMeshLinkData.endPos + Vector3.up * Agent.baseOffset, Agent.currentOffMeshLinkData.owner.GetComponent<NavMeshLinkCurve>()));
        }
    }

    public IEnumerator JumpCurve(Vector3 StartPosition, Vector3 EndPosition, NavMeshLinkCurve Curve)
    {
        print("Jump Curve started");
        IsRunning = true;
        Agent.enabled = false;
        Timer = 0.0f;
        SoundHandler.PlaySoundEffect(AudioSource, JumpSound, null, transform.position);

        if (Animator != null)
        {
            Animator.SetTrigger("Jumping");
            Debug.Log("Jumping");
        }

        while (Timer < 1.0f)
        {
            float YOffset = Curve.JumpCurve.Evaluate(Timer);
            transform.position = Vector3.Lerp(StartPosition, EndPosition, Timer) + YOffset * Vector3.up;
            Timer += Time.deltaTime * Curve.JumpSpeed;
            yield return null;
        }

        if (CompanionState != null && CompanionState.enabled || CompanionState == null)
        {
            Agent.enabled = true;
            Agent.CompleteOffMeshLink();
        }

        IsRunning = false;
    }
}
