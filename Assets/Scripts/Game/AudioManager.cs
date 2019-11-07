using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public AudioClip[] bigFishDieAC;
    public AudioClip gold;
    public AudioClip silver;
    public AudioClip changeGun;
    public AudioClip web;
    public AudioClip fire;
    public AudioClip wave;
    public AudioClip levelUp;
    private bool audioIsOn;
    private AudioSource ads;


    public static AudioManager Instance;
    private void Awake()
    {
        Instance = this;       
    }

    private void Start()
    {
        ads = gameObject.GetComponent<AudioSource>();
        audioIsOn = GameManager.Instance.GameData.AudioIsOn;
        AudioSet(audioIsOn);
    }

    private Vector3 pos = new Vector3(0, 0, -45);
    public void PlayAudio(ACName ac)
    {
        if (audioIsOn == false)
            return;

        switch (ac)
        {
            case ACName.Gold:
                AudioSource.PlayClipAtPoint(gold, pos);
                break;
            case ACName.Silver:
                AudioSource.PlayClipAtPoint(silver, pos);
                break;
            case ACName.ChangeGun:
                AudioSource.PlayClipAtPoint(changeGun, pos);
                break;
            case ACName.Web:
                AudioSource.PlayClipAtPoint(web, pos);
                break;
            case ACName.Fire:
                AudioSource.PlayClipAtPoint(fire, pos);
                break;
            case ACName.Wave:
                AudioSource.PlayClipAtPoint(wave, pos);
                break;
            case ACName.LevelUp:
                AudioSource.PlayClipAtPoint(levelUp, pos);
                break;
            case ACName.BigFish:
                if (isCD == false)
                {
                    int index = Random.Range(0, bigFishDieAC.Length);
                    AudioSource.PlayClipAtPoint(bigFishDieAC[index], pos);
                    isCD = true;
                }                
                break;
            default:
                break;
        }
    }

    private float timer = 2f;
    bool isCD = false;
    private void Update()
    {
        if (isCD == true)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 2f;
                isCD = false;
            }
        }        
    }

    public void AudioSet(bool isOn)
    {
        audioIsOn = isOn;

        if (audioIsOn)
            ads.Play();
        else
            ads.Pause();
    }
}

public enum ACName
{
    Gold,
    Silver,
    ChangeGun,
    Web,
    Fire,
    Wave,
    LevelUp,
    BigFish
}