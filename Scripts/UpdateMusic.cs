using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class UpdateMusic : MonoBehaviour
{
    public AudioClip NextMusicClip;
    public AudioSource[] MusicSources;
    [HideInInspector] public float FadeInOutSpeed = 0.75f;
    private AudioSource ActiveSource, NextSource;
    [HideInInspector] public bool RunFromThis;
    private float MaxVolume;

    private void Start()
    {
        if (RunFromThis)
        {
            StartCoroutine(MusicTransition());
        }
    }

    public static void SpawnMusicTransition(AudioSource[] Sources, AudioClip Clip)
    {
        GameObject MusicTransitionObject = new GameObject();
        MusicTransitionObject.AddComponent<UpdateMusic>();
        UpdateMusic Music = MusicTransitionObject.GetComponent<UpdateMusic>();
        Music.MusicSources = Sources;
        Music.NextMusicClip = Clip;
        Music.RunFromThis = true;
    }

    public IEnumerator MusicTransition()
    {
        foreach (AudioSource Source in MusicSources)
        {
            if (Source.gameObject.activeSelf)
            {
                ActiveSource = Source;

                if (NextSource != null)
                {
                    break;
                }
            }

            else
            {
                NextSource = Source;

                if (ActiveSource != null)
                {
                    break;
                }
            }
        }

        if (NextSource != null)
        {
            NextSource.volume = 0.0f;
            NextSource.gameObject.SetActive(true);
            NextSource.clip = NextMusicClip;
            NextSource.Play();
        }

        MaxVolume = ActiveSource.volume;
        
        while (ActiveSource != null && ActiveSource.volume > 0.0f || NextSource != null && NextSource.volume < MaxVolume)
        {
            ActiveSource.volume = Mathf.MoveTowards(ActiveSource.volume, 0.0f, FadeInOutSpeed * Time.deltaTime);
            NextSource.volume = Mathf.MoveTowards(NextSource.volume, MaxVolume, FadeInOutSpeed * Time.deltaTime);
            yield return null;
        }

        if (ActiveSource != null)
        {
            ActiveSource.Stop();
            ActiveSource.gameObject.SetActive(false);
        }
        
        ActiveSource = null;
        NextSource = null;

        if (RunFromThis)
        {
            Destroy(gameObject);
        }
    }
}
