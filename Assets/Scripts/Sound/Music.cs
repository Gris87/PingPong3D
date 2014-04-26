using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{
    public AudioClip[] clips;

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(gameObject);

        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        int previousClip=-1;

        do
        {
            int clip;

            do
            {
                clip=Random.Range(0, clips.Length);
            } while (clip==previousClip);

            audio.clip=clips[clip];
            audio.volume=Options.musicVolume;
            audio.Play();

            do
            {
                yield return new WaitForSeconds(0.2f);
            } while(audio.isPlaying);

            yield return new WaitForSeconds(0.2f);

            previousClip=clip;
        } while (true);
    }

    // Update is called once per frame
    void Update()
    {
        if (audio.volume!=Options.musicVolume)
        {
            audio.volume=Options.musicVolume;
        }
    }
}
