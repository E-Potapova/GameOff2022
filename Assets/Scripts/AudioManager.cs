using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public Sound[] meows;

    public Sound[] purrs;
    
    //singleton
    public static AudioManager instance;

    void Awake()
    {

        if(instance == null){
            instance =this;
        }
        else{
            Destroy(gameObject);
            return;
        }
        //singleton stuff
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        foreach(Sound s in meows){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        foreach(Sound s in purrs){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

    }

    //used in my temp start menu
    void Start(){
        Play("BackgroundMusic");
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s==null){
            Debug.Log("Sound: " + name + " not found:(");
            return;
        }
        s.source.Play();  
    }

    public void PlayRandMeow(){
        int val = UnityEngine.Random.Range(0, meows.Length-1);
        Debug.Log(val);
        Sound meow = meows[val];

        if(meow.source == null){
            Debug.Log("Sound: " + meow.name + " not found:(");
            return;
        }
        meow.source.PlayOneShot(meow.clip, meow.volume);
        //meow.source.Play();
    }

    public void PlayRandPurr(){
        int val = UnityEngine.Random.Range(0, purrs.Length) -1;
        Sound purr = purrs[val];
        if(purr ==null){
            Debug.Log("Sound: " + val + " not found:(");
            return;
        }
        purr.source.PlayOneShot(purr.clip, purr.volume);
    }


}
