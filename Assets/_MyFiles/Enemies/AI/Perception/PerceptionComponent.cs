using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    public delegate void OnPerceptionTargetUpdatedDelegate(GameObject target, bool bIsSensed);
    public event OnPerceptionTargetUpdatedDelegate OnPerceptionTargetUpdated;

    private LinkedList<Stimuli> _currentSensedStimuli = new LinkedList<Stimuli>();
    private Stimuli _currentTargetStimuli;
    private void Awake()
    {
        Sense[] senses = GetComponents<Sense>();
        foreach (Sense sense in senses)
        {
            sense.OnSenseUpdated += HandleSenseUpdated;
        }
    }

    private void HandleSenseUpdated(Stimuli stimuli, bool bWasSensed)
    {
        LinkedListNode<Stimuli> foundNode = _currentSensedStimuli.Find(stimuli);
        if (bWasSensed)
        {
            if (foundNode != null)
            {
                _currentSensedStimuli.AddAfter(foundNode, stimuli);
            }
            else
            {
                _currentSensedStimuli.AddLast(stimuli);
            }
        }
        else
        {
            if (foundNode != null)
            {
                _currentSensedStimuli.Remove(foundNode);
            }

        }
        DetermineTarget();
    }

    private void DetermineTarget()
    {
        if (_currentSensedStimuli.Count == 0)
        {
            if (_currentTargetStimuli != null)
            {
                OnPerceptionTargetUpdated?.Invoke(_currentTargetStimuli.gameObject, false);
                _currentTargetStimuli = null;
            }
        }
        else
        {
            Stimuli earliestStimuli = _currentSensedStimuli.First.Value;
            if (earliestStimuli != _currentTargetStimuli && earliestStimuli)
            {
                _currentTargetStimuli = earliestStimuli;
                OnPerceptionTargetUpdated?.Invoke(_currentTargetStimuli.gameObject, true);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(_currentSensedStimuli.Count == 0)
            return;

        Vector3 currentTargetStimuliLoc = _currentTargetStimuli.transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(currentTargetStimuliLoc + Vector3.up, .7f);
        Gizmos.DrawLine(transform.position + Vector3.up, currentTargetStimuliLoc + Vector3.up);

        foreach(Stimuli otherStimuli in _currentSensedStimuli)
        {
            if (otherStimuli == _currentTargetStimuli)
                continue;

            Vector3 stimuliLoc = otherStimuli.transform.position;
            Gizmos.DrawWireSphere(stimuliLoc + Vector3.up, .5f);
            Gizmos.DrawLine(transform.position + Vector3.up, stimuliLoc + Vector3.up);
        }
    }
}
