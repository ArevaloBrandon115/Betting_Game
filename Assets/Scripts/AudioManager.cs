using UnityEngine.Audio;
using System;
using UnityEngine;
namespace PickBonus {
    public class AudioManager : MonoBehaviour {

        public Sound[] sounds;

        void Awake()
        {
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        //play sound
        public void Play(string name)
        {
            //find sound to play
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("Sound not found: " + name);
                return;
            }
            s.source.Play();
        }

        //stop sound
        public void Stop(string name)
        {
            //find sound to play
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("Sound not found: " + name);
                return;
            }
            s.source.Stop();
        }
    }
}