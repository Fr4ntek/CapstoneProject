using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop = false;
        [HideInInspector] public AudioSource source;
    }

    public List<Sound> sounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.loop = s.loop;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Play("MainMenu");
        }
    }
    public void Play(string name)
    {
        var s = sounds.Find(sound => sound.name == name);
        if (s != null) s.source.Play();
    }

    public void Stop(string name)
    {
        var s = sounds.Find(sound => sound.name == name);
        if (s != null) s.source.Stop();
    }

    public void StopAll()
    {
        AudioSource[] allSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource src in allSources)
        {
            src.Stop();
        }
        //foreach (Sound s in sounds)
        //{
        //    s.source.Stop();
        //}
    }
}
