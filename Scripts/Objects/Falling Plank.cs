using UnityEngine;

public class FallingPlank : MonoBehaviour
{
    private bool Fall;
    [SerializeField] private float FallSpeed, FallAngle;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip FallClip, LandSound;
    private Quaternion TargetRotation;
    private ButtonIconTrigger ButtonIconTrigger;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        ButtonIconTrigger = transform.parent.GetComponentInChildren<ButtonIconTrigger>();
    }

    private void Update()
    {
        if (Fall)
        {
            TargetRotation = Quaternion.Euler(FallAngle, transform.root.eulerAngles.y, transform.root.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, FallSpeed * Time.deltaTime);

            if (transform.rotation == TargetRotation)
            {
                print("Fell");
                AudioSource.clip = LandSound;
                AudioSource.Play();
                Fall = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Charge" && TargetRotation.x == 0)
        {
            AudioSource.clip = FallClip;
            AudioSource.Play();

            if (ButtonIconTrigger != null)
            {
                ButtonIconTrigger.ButtonIconParent.SetActive(false);
                Destroy(ButtonIconTrigger.gameObject);
            }

            Fall = true;
        }
    }
}
