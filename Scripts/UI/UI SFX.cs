using UnityEngine;
using UnityEngine.EventSystems;

public class UISFX : MonoBehaviour, ISelectHandler
{
    [SerializeField] private AudioClip SelectSound;
    private SoundHandler SoundHandler;
    private AudioSource AudioSource;

    private void Awake()
    {
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
        AudioSource = GetComponent<AudioSource>();
    }

    public void OnSelect(BaseEventData Data)
    {
        PlaySound(SelectSound);
    }

    public void PlaySound(AudioClip Clip)
    {
        if (Clip != null && Time.timeScale == 0.0f)
        {
            SoundHandler.PlaySoundEffect(AudioSource, Clip, null, transform.position);
        }
    }
}
