using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region AudioClips
    [SerializeField]
    List<AudioClip> jumpAudioClips;
    [SerializeField]
    List<AudioClip> dieAudioClips;
    [SerializeField]
    AudioClip runningAudioClips;
    [SerializeField]
    List<AudioClip> BackgroundMusicAudioClips;
    #endregion


    AudioSource soundEffectAudioSource;
    AudioSource backgroundMusicAudioSource;


    // Start is called before the first frame update
    void Start()
    {

        soundEffectAudioSource = gameObject.AddComponent<AudioSource>();
        soundEffectAudioSource.volume = 0.2f;

        backgroundMusicAudioSource = gameObject.AddComponent<AudioSource>();
        
        if (!soundEffectAudioSource)
        {
            throw new System.Exception("audioSource is not found!");
        }

        CharacterHandler.characterDiedEvent += CharacterDiedSound;
        CharacterHandler.characterJumpedEvent += CharacterJumpedSound;
        CharacterHandler.characterRunEvent += CharacterRunSound;
        PlayBackgroundMusic();

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

        CharacterHandler.characterDiedEvent -= CharacterDiedSound;
        CharacterHandler.characterJumpedEvent -= CharacterJumpedSound;
        CharacterHandler.characterRunEvent -= CharacterRunSound;
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
