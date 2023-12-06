using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntEventChannel", menuName = "Event Channels/Int Event Channel")]
public class IntEventChannelSO : GenericEventChannelSO<IntEventArgs>
{
    
}

public class IntEventArgs : EventArgs
{
    public int Value { get; private set; }

    public IntEventArgs(int value)
    {
        Value = value;
    }
}
