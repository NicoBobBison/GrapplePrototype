using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


// Shout out to Brackeys!!!
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    public AudioMixerGroup musicMixer;
    string currentFadeIn;
    string currentFadeOut;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = musicMixer;
        }
    }

    public void Play(string name)
    {
        Sound s = FindSound(name);
        s.source.Play();
    }

    public bool SoundPlaying(string name)
    {
        Sound s = FindSound(name);
        return s.source.isPlaying;
    }

    IEnumerator _FadeOut(string name, float time)
    {
        
        Sound s = FindSound(name);
        while(s.source.volume > 0)
        {
            s.source.volume -= (1 / time) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        s.source.Stop();
    }

    IEnumerator _FadeIn(string name, float time)
    {
        Sound s = FindSound(name);
        Play(name);
        while (s.source.volume < 1)
        {
            s.source.volume += (1 / time) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    Sound FindSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s != null)
        {
            return s;
        }
        Debug.Log("Couldn't find sound named " + name);
        return null;
    }
    public void FadeOut(string name)
    {
        StartCoroutine(_FadeOut(name, 0.2f));
    }
    public void FadeIn(string name)
    {
        StartCoroutine(_FadeIn(name, 0.2f));
    }
}
