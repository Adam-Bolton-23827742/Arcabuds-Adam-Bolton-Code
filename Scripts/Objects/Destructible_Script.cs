using UnityEngine;

public class Destructible_Script : MonoBehaviour
{
    public float BoxHealth;
    public ParticleSystem PS;

    [Header("AdamChanges")]
    [SerializeField] private SoundHandler SoundHandler;
    [SerializeField] AudioClip DestroyClip;
    private AudioSource AudioSource;
    [SerializeField] private GameObject[] SpawnableObjects;
    [SerializeField] private float PercentageChance = 0.5f;
    private ButtonIconTrigger ButtonIconTrigger;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        ButtonIconTrigger = GetComponentInChildren<ButtonIconTrigger>();
    }

    public void TakeDamage(float damage)
    {
        BoxHealth -= damage;
        if (BoxHealth <= 0)
        {
            if (SoundHandler != null)
            {
                SoundHandler.PlaySoundEffect(AudioSource, DestroyClip, null, transform.position);
            }

            if (PS != null)
            {
                Instantiate(PS, transform.position, Quaternion.identity);
            }

            if (SpawnableObjects != null && SpawnableObjects.Length > 0 && Random.value <= PercentageChance)
            {
                int RandomPrefab = Random.Range(0, SpawnableObjects.Length);

                if (SpawnableObjects[RandomPrefab] != null)
                {
                    Instantiate(SpawnableObjects[RandomPrefab], transform.position, Quaternion.identity);
                }
            }

            if (ButtonIconTrigger != null)
            {
                ButtonIconTrigger.ButtonIconParent.SetActive(false);
            }

            Destroy(gameObject);
        }
    }
}