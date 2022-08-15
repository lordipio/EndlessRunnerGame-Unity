using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region AudioClips
    [Header("AudioClips:")]
    [Space]
    [Space]

    [SerializeField]
    List<AudioClip> jumpAudioClips;
    [SerializeField]
    List<AudioClip> dieAudioClips;
    [SerializeField]
    AudioClip runningAudioClips;
    [SerializeField]
    List<AudioClip> BackgroundMusicAudioClips;
    [SerializeField]
    AudioClip sparkleAudioClip;
    #endregion

    #region AudioSources
    [Header("AudioSources:")]
    [Space]
    [Space]

    AudioSource soundEffectAudioSource;
    AudioSource backgroundMusicAudioSource;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        AssignInitialValues();
        PlayBackgroundMusic();
    }

    void AssignInitialValues()
    {
        soundEffectAudioSource = gameObject.AddComponent<AudioSource>();
        soundEffectAudioSource.volume = 0.2f;
        backgroundMusicAudioSource = gameObject.AddComponent<AudioSource>();

        if (!soundEffectAudioSource)
        {
            throw new System.Exception("audioSource is not found!");
        }

        GameManager.characterDiedEvent += CharacterDiedSound;
        CharacterHandler.characterJumpedEvent += CharacterJumpedSound;
        CharacterHandler.characterRunEvent += CharacterRunSound;
        CoinAndGemHandler.grabedCoinsEvent += PlaySparkleSound;

    }

    void CharacterJumpedSound()
    {
        soundEffectAudioSource.clip = SelectRandomAudioClip(jumpAudioClips);
        soundEffectAudioSource = RandomizePitch(soundEffectAudioSource);
        soundEffectAudioSource.Play();
        soundEffectAudioSource.loop = false;
    }

    void CharacterDiedSound()
    {
        soundEffectAudioSource.clip = SelectRandomAudioClip(dieAudioClips);
        soundEffectAudioSource = RandomizePitch(soundEffectAudioSource);
        soundEffectAudioSource.Play();
        soundEffectAudioSource.loop = false;

        GameManager.characterDiedEvent -= CharacterDiedSound;
        CharacterHandler.characterJumpedEvent -= CharacterJumpedSound;
        CharacterHandler.characterRunEvent -= CharacterRunSound;
        CoinAndGemHandler.grabedCoinsEvent -= PlaySparkleSound;
    }

    void CharacterRunSound()
    {
        soundEffectAudioSource.clip = runningAudioClips;
        soundEffectAudioSource.Play();
        soundEffectAudioSource.loop = true;
    }

    void PlayBackgroundMusic()
    {
        AudioClip currentAudioClip = SelectRandomAudioClip(BackgroundMusicAudioClips);
        backgroundMusicAudioSource.volume = 0.05f; 
        backgroundMusicAudioSource.clip = currentAudioClip;
        backgroundMusicAudioSource.Play();
        Invoke("PlayBackgroundMusic", currentAudioClip.length);
    }

    void PlaySparkleSound()
    {
        soundEffectAudioSource.clip = sparkleAudioClip;
        RandomizePitch(soundEffectAudioSource);
        soundEffectAudioSource.Play();
        soundEffectAudioSource.loop = false;
    }

    AudioClip SelectRandomAudioClip(List<AudioClip> audioClipList)
    {
        return audioClipList[Random.Range(0, audioClipList.Count)];
    }

    AudioSource RandomizePitch(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        return audioSource;
    }

    
}
