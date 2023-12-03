using System;
using UnityEngine;

public abstract class GenericEventChannelSO : ScriptableObject
{
    public event EventHandler OnEventRaised;

    public void RaiseEvent(object sender)
    {
        OnEventRaised?.Invoke(sender, EventArgs.Empty);
    }
}

public abstract class GenericEventChannelSO<T> : ScriptableObject where T : EventArgs
{
    public event EventHandler<T> OnEventRaised;

    public void RaiseEvent(object sender, T args)
    {
        OnEventRaised?.Invoke(sender, args);
    }
}
