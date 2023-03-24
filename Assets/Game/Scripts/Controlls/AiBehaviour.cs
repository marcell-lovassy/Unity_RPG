using UnityEngine;

public abstract class AiBehaviour : MonoBehaviour
{
    public int BehaviourPriority => behaviourPriority;

    [SerializeField]
    private int behaviourPriority;

    public abstract void StartBehaviour();

    public abstract void StopBehaviour();

    public virtual bool CanDoBehaviour()
    {
        return true;
    }
}
