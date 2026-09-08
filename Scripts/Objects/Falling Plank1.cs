using UnityEngine;

public class FallingPlank1 : MonoBehaviour
{
    private bool Fall;
    [SerializeField] private float FallSpeed, FallAngle;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip FallClip, LandSound;
    private Quaternion TargetRotation;
    public GameObject navmeshobject;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
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
                Destroy(navmeshobject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Charge" && TargetRotation.x == 0)
        {
            print("fall");
            AudioSource.clip = FallClip;
            AudioSource.Play();
            Fall = true;
        }
    }
}
