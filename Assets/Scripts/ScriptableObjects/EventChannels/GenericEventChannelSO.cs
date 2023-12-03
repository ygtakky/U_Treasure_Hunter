using System;
using UnityEngine;

public abstract class GenericEventChannelSO<T> : ScriptableObject where T : EventArgs
{
    public event EventHandler<T> OnEventRaised;

    public void RaiseEvent(object sender, T args)
    {
        OnEventRaised?.Invoke(sender, args);
    }
}
