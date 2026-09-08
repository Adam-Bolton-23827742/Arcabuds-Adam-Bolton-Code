using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private SoundHandler SoundHandler;
    [SerializeField] private AudioClip CollectableSound;
    private AudioSource AudioSource;

    private void Start()
    {
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SoundHandler.PlaySoundEffect(AudioSource, CollectableSound, null, transform.position);
            PlayerStats Stats = other.GetComponent<PlayerStats>();
            Stats.CollectableCount++;
            Stats.CollectableCountText.text = Stats.CollectableCount.ToString();
            Stats.FlashStar();
            Destroy(gameObject);
        }
    }
}
