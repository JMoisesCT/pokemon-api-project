using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Boolean Event Channel")]
public class BoolEventChannelSO : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool boolParam)
    {
        OnEventRaised?.Invoke(boolParam);
    }
}
