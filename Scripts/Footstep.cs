using UnityEngine;

public class Footstep : MonoBehaviour
{
    public SoundHandler SoundHandler;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip FootStepSound;

    private void Start()
    {
        AudioSource = GetComponentInParent<AudioSource>();
    }

    public void PlayFootstep()
    {
        SoundHandler.PlaySoundEffect(AudioSource, FootStepSound, null, transform.position);
    }
}
