using UnityEngine;

public class HearingSense : Sense
{
    [SerializeField] float hearingMinVolume = 10f;

    public delegate void OnSoundEventSendDelegate(float volume, Stimuli stimuli);

    private static float _attenuation = 0.05f;

    public static event OnSoundEventSendDelegate OnSoundEventSend;

    public static void SendSoundEvent(float volume, Stimuli stimuli)
    {
        OnSoundEventSend?.Invoke(volume, stimuli);
    }

    private void Awake()
    {
        OnSoundEventSend += HandleSoundEvent;
    }

    private void HandleSoundEvent(float volume, Stimuli stimuli)
    {
        float soundTravelDistance = Vector3.Distance(transform.position, stimuli.transform.position);
        float volumeAtOwner = volume - 20 * Mathf.Log(soundTravelDistance, 10) - _attenuation * soundTravelDistance;
        Debug.Log($"Volume at Owner is: {volumeAtOwner}");
        if (volume < hearingMinVolume)
            return;

        HandleSensibleStimuli(stimuli);
    }
}
