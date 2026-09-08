using System.Collections.Generic;
using UnityEngine;

public class Lillypad : MonoBehaviour
{
    public float LilyPadBounceForce;
    public List<GameObject> CharactersOnLilypad;
    public Vector3 BounceDirection;
    private Vector3 OGPosition, DownPosition;
    [HideInInspector] public AudioSource AudioSource;
    [HideInInspector] public Transform Trajectory;
    [SerializeField] private float FallSpeed, TopYOffset, BottomYOffset;
    private bool CanMove, MoveDirection;

    private void Start()
    {
        CharactersOnLilypad = new List<GameObject>();
        AudioSource = GetComponent<AudioSource>();
        Trajectory = transform.Find("Trajectory");
        OGPosition = transform.position;
        DownPosition = new Vector3(OGPosition.x, OGPosition.y - BottomYOffset, OGPosition.z);
    }

    private void Update()
    {
        if (CanMove)
        {
            if (!MoveDirection)
            {
                print("Move down");
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, DownPosition, FallSpeed * Time.deltaTime);

                if (transform.parent.position == DownPosition)
                {
                    MoveDirection = true;
                }
            }

            else
            {
                print("Move up");
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, OGPosition, FallSpeed * Time.deltaTime);

                if (transform.parent.position == OGPosition)
                {
                    MoveDirection = false;
                    CanMove = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (tag == "Bounce" && collision.gameObject.tag == "Player")
        {
            print("Added: " + collision.gameObject.name + " to lilypad");
            CharactersOnLilypad.Add(collision.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (tag == "Fall" && collision.gameObject.tag == "Player" && collision.gameObject.name == "Pebbles" && PlayerConfigurationManager.PlayerConfigurationList.Count == 2
            || tag == "Fall" && collision.gameObject.tag == "Player" && collision.gameObject.name == "Pebbles" && PlayerConfigurationManager.PlayerConfigurationList.Count == 1 && !collision.gameObject.GetComponent<CompanionState>().enabled)
        {
            if (collision.transform.GetComponent<PlayerStats>().CurrentHealth > 0)
            {
                CanMove = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (tag == "Bounce" && collision.gameObject.tag == "Player")
        {
            print("Removed: " + collision.gameObject.name + " from lilypad");
            CharactersOnLilypad.Remove(collision.gameObject);
        }
    }
}
