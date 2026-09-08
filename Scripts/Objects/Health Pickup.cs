using UnityEngine;
using UnityEngine.VFX;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float HealthIncrease;
    private SoundHandler SoundHandler;
    [SerializeField] private AudioClip HealthAudioClip;
    [SerializeField] private VisualEffectAsset HealVFX;
    [SerializeField] private GameObject VFXTemplate;

    private void Start()
    {
        SoundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerStats Stats = other.GetComponent<PlayerStats>();

            if (Stats.CurrentHealth < Stats.MaxHealth)
            {
                Stats.CurrentHealth += HealthIncrease;
                SoundHandler.PlaySoundEffect(GetComponent<AudioSource>(), HealthAudioClip, null, transform.position);

                if (HealVFX != null && VFXTemplate != null)
                {
                    GameObject VFX = Instantiate(VFXTemplate, transform.position, Quaternion.identity);
                    VFX.GetComponent<VisualEffect>().visualEffectAsset = HealVFX;
                    VFX.GetComponent<VisualEffect>().Play();
                }

                if (Stats.CurrentHealth > Stats.MaxHealth)
                {
                    Stats.CurrentHealth = Stats.MaxHealth;
                }

                Stats.UpdateHealthVisuals(Stats.CurrentHealth);

                Destroy(gameObject);
            }
        }
    }
}
