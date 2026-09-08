using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineCamera CameraToActivate;
    [SerializeField] private CinemachineCamera[] AllCams;
    [SerializeField] private GameObject OtherTrigger;
    [SerializeField] private AssignControllers AssignControllers;
    [SerializeField] private Transform TPLocation;
    private UpdateMusic UpdateMusic;
    [HideInInspector] public bool CanSpawnMusic = true;

    private void Start()
    {
        UpdateMusic = GetComponent<UpdateMusic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats PlayerStats = other.GetComponent<PlayerStats>();

        if (PlayerStats != null)
        {
            if (UpdateMusic != null && CanSpawnMusic)
            {
                CanSpawnMusic = false;

                if (OtherTrigger != null)
                {
                    UpdateMusic.SpawnMusicTransition(UpdateMusic.MusicSources, UpdateMusic.NextMusicClip);
                }

                else
                {
                    StartCoroutine(UpdateMusic.MusicTransition());
                }
            }

            if (TPLocation != null)
            {
                foreach (PlayerInput Player in AssignControllers.PlayerInputs)
                {
                    if (other.gameObject != Player.gameObject)
                    {
                        print(gameObject.name + " " + Player.name);
                        Player.transform.position = TPLocation.position;
                    }
                }
            }

            SwitchCam();
        }
    }

    public void SwitchCam()
    {
        foreach (CinemachineCamera Cam in AllCams)
        {
            if (Cam == CameraToActivate)
            {
                Cam.gameObject.SetActive(true);
                Cam.Prioritize();
            }

            else if (Cam != null)
            {
                Cam.gameObject.SetActive(false);
            }
        }

        if (OtherTrigger != null)
        {
            OtherTrigger.GetComponent<CameraSwitch>().CanSpawnMusic = true;
            OtherTrigger.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
