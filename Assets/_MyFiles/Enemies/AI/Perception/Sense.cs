using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sense : MonoBehaviour
{
    public delegate void OnSenseUpdatedDelegate(Stimuli stimuli, bool bWasSensed);
    public event OnSenseUpdatedDelegate OnSenseUpdated;
    
    [SerializeField] private bool bDrawDebug = false;
    [SerializeField] private float forgetTime = 3f;
    private static HashSet<Stimuli> _registeredStimuliSet = new HashSet<Stimuli>();

    private HashSet<Stimuli> _currentSensibleStimuliSet = new HashSet<Stimuli>();

    private Dictionary<Stimuli, Coroutine> _forgettingCoroutine = new Dictionary<Stimuli, Coroutine>();


    public static void RegisterStimuli(Stimuli stimuli)
    {
        _registeredStimuliSet.Add(stimuli);
    }

    public static void UnregisterStimuli(Stimuli stimuli)
    {
        _registeredStimuliSet.Remove(stimuli);
    }

    protected virtual bool IsStimuliSensible(Stimuli stimuli)
    {
        return false;
    }

    private void Update()
    {
        foreach(Stimuli stimuli in _registeredStimuliSet)
        {
            if (IsStimuliSensible(stimuli))
            {
                HandleSensibleStimuli(stimuli);
            }
            else
            {
                HandleNonSensibleStimuli(stimuli);
            }
        }
    }

    protected void HandleNonSensibleStimuli(Stimuli stimuli)
    {
        //Not sensed and never sensed prior
        if(!_currentSensibleStimuliSet.Contains(stimuli))
            return;
        //No longer sensed
        _currentSensibleStimuliSet.Remove(stimuli);

        Coroutine forgetingCoroutine = StartCoroutine(ForgetStimuli(stimuli));
        _forgettingCoroutine.Add(stimuli, forgetingCoroutine);
    }
    protected void HandleSensibleStimuli(Stimuli stimuli)
    {
        //Currently sensed and sensed prior
        if(_currentSensibleStimuliSet.Contains(stimuli))
            return;
        //Newly sensed
        _currentSensibleStimuliSet.Add(stimuli);

        if (_forgettingCoroutine.ContainsKey(stimuli))
        {
            StopCoroutine(_forgettingCoroutine[stimuli]);
            _forgettingCoroutine.Remove(stimuli);
            return;
        }

        OnSenseUpdated?.Invoke(stimuli, true);
    }

    private IEnumerator ForgetStimuli(Stimuli stimuli)
    {
        yield return new WaitForSeconds(forgetTime);
        _forgettingCoroutine.Remove(stimuli);
        OnSenseUpdated.Invoke(stimuli, false);
    }

    private void OnDrawGizmos()
    {
        if (bDrawDebug)
        {
            OnDrawDebug();
        }
    }

    protected virtual void OnDrawDebug()
    {
        
    }
}
