using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "Event Channels/Void Event Channel")]
public class VoidEventChannelSO : ScriptableObject
{
    public event EventHandler OnEventRaised;

    public void RaiseEvent(object sender)
    {
        OnEventRaised?.Invoke(sender, EventArgs.Empty);
    }
}
