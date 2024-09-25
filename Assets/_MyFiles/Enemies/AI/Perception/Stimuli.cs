using UnityEngine;

public class Stimuli : MonoBehaviour
{
    void Start()
    {
        Sense.RegisterStimuli(this);
    }

    private void OnDestroy()
    {
        Sense.UnregisterStimuli(this);
    }

    public void SendSoundEvent(float volume)
    {
        HearingSense.SendSoundEvent(volume, this);
    }
}
