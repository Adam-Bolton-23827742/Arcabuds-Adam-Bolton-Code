using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [HideInInspector] public bool RunSoundFromScript;

    public void PlaySoundEffect(AudioSource AudioSource, AudioClip AudioClip, AudioClip AudioClip2, Vector3 Position)
    {
        RunSoundFromScript = false;

        if (AudioClip2 == null)
        {
            AudioSource.clip = AudioClip;

            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                PlayClipAtPointNew(AudioClip, Position, PlayerPrefs.GetFloat("SFXVolume"), 0.5f, AudioSource);
            }

            else
            {
                PlayClipAtPointNew(AudioClip, Position, 1.0f, 0.5f, AudioSource);
            }
        }

        else
        {
            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                PlayClipAtPointNew(AudioClip, Position, PlayerPrefs.GetFloat("SFXVolume"), 0.5f, AudioSource);
                PlayClipAtPointNew(AudioClip2, Position, PlayerPrefs.GetFloat("SFXVolume"), 0.5f, AudioSource);
            }

            else
            {
                PlayClipAtPointNew(AudioClip, Position, 1.0f, 0.5f, AudioSource);
                PlayClipAtPointNew(AudioClip2, Position, 1.0f, 0.5f, AudioSource);
            }
        }
    }

    private void PlayClipAtPointNew(AudioClip clip, Vector3 position, [UnityEngine.Internal.DefaultValue("1.0F")] float volume, float SpatialBlend, AudioSource Source)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = SpatialBlend;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
}
