using Unity.Audio;
using System;
using System.Collections;
using UnityEngine;


// Shout out to Brackeys!
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

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

    public IEnumerator FadeOut(string name, float time)
    {
        Sound s = FindSound(name);
        while(s.source.volume > 0)
        {
            s.source.volume -= (1 / time) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        s.source.Stop();
    }

    public IEnumerator FadeIn(string name, float time)
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
}
