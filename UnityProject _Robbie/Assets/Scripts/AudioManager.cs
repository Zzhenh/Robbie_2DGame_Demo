using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager audioCurrent;

    [Header("环境声音")]
    public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("Robbie音效")]
    public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip deathClip;

    [Header("Robbie人声")]
    public AudioClip jumpVoiceClip;
    public AudioClip deathVoiceClip;
    public AudioClip orbVoiceClip;

    [Header("FX声音")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    public AudioClip doorFXClip;
    public AudioClip StartLevelClip;
    public AudioClip WinClip;

    public AudioMixerGroup ambientGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup fxGroup;
    public AudioMixerGroup playerGroup;
    public AudioMixerGroup voiceGroup;

    AudioSource ambientSource;
    AudioSource musicSource;
    AudioSource fxSource;
    AudioSource playerSource;
    AudioSource voiceSource;

    private void Awake()
    {
        if ( audioCurrent != null )
        {
            Destroy(gameObject);
            return;
        }

        audioCurrent = this;

        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        ambientSource.outputAudioMixerGroup = ambientGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        fxSource.outputAudioMixerGroup = fxGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;

        StartLevelAudio();
    }

    void StartLevelAudio()
    {
        audioCurrent.ambientSource.clip = audioCurrent.ambientClip;
        audioCurrent.ambientSource.loop = true;
        audioCurrent.ambientSource.Play();

        audioCurrent.musicSource.clip = audioCurrent.musicClip;
        audioCurrent.musicSource.loop = true;
        audioCurrent.musicSource.Play();

        audioCurrent.fxSource.clip = audioCurrent.StartLevelClip;
        audioCurrent.fxSource.Play();
    }

    public static void PlayerFootstepAudio()
    {
        int index = Random.Range(0, audioCurrent.walkStepClips.Length);

        audioCurrent.playerSource.clip = audioCurrent.walkStepClips[index];
        audioCurrent.playerSource.Play();
    }

    public static void PlayerCrouchFootstepAudio()
    {
        int index = Random.Range(0, audioCurrent.crouchStepClips.Length);

        audioCurrent.playerSource.clip = audioCurrent.crouchStepClips[index];
        audioCurrent.playerSource.Play();
    }

    public static void PlayJumpAudio()
    {
        audioCurrent.playerSource.clip = audioCurrent.jumpClip;
        audioCurrent.playerSource.Play();

        audioCurrent.voiceSource.clip = audioCurrent.jumpVoiceClip;
        audioCurrent.voiceSource.Play();
    }

    public static void PlayDeathAudio()
    {
        audioCurrent.playerSource.clip = audioCurrent.deathClip;
        audioCurrent.playerSource.Play();

        audioCurrent.voiceSource.clip = audioCurrent.deathVoiceClip;
        audioCurrent.voiceSource.Play();

        audioCurrent.fxSource.clip = audioCurrent.deathFXClip;
        audioCurrent.fxSource.Play();
    }

    public static void PlayOrbAudio()
    {
        audioCurrent.voiceSource.clip = audioCurrent.orbVoiceClip;
        audioCurrent.voiceSource.Play();

        audioCurrent.fxSource.clip = audioCurrent.orbFXClip;
        audioCurrent.fxSource.Play();
    }

    public static void PlayDoorOpedAudio()
    {
        audioCurrent.fxSource.clip = audioCurrent.doorFXClip;
        audioCurrent.fxSource.PlayDelayed(1f);
    }

    public static void PlayWinAudio()
    {
        audioCurrent.fxSource.clip = audioCurrent.WinClip;
        audioCurrent.fxSource.Play();
        audioCurrent.playerSource.Stop();
    }
}
