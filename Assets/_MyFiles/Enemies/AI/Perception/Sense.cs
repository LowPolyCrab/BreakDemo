using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sense : MonoBehaviour
{
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

    protected abstract bool IsStimuliSensible(Stimuli stimuli);

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

    private void HandleNonSensibleStimuli(Stimuli stimuli)
    {
        //Not sensed and never sensed prior
        if(!_currentSensibleStimuliSet.Contains(stimuli))
            return;
        //No longer sensed
        _currentSensibleStimuliSet.Remove(stimuli);

        Coroutine forgetingCoroutine = StartCoroutine(ForgetStimuli(stimuli));
        _forgettingCoroutine.Add(stimuli, forgetingCoroutine);
    }
    private void HandleSensibleStimuli(Stimuli stimuli)
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

        Debug.Log($"I just sensed: {stimuli.gameObject.name}");
    }

    private IEnumerator ForgetStimuli(Stimuli stimuli)
    {
        yield return new WaitForSeconds(forgetTime);
        Debug.Log($"I just lost track of: {stimuli.gameObject.name}");
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
