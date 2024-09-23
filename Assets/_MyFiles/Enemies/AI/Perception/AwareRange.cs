using UnityEngine;

public class AwareRange : Sense
{
    [SerializeField] private float awareRange = 1f;
    protected override bool IsStimuliSensible(Stimuli stimuli)
    {
        return transform.InRangeOf(stimuli.transform, awareRange);
    }

    protected override void OnDrawDebug()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, awareRange);
    }
}
