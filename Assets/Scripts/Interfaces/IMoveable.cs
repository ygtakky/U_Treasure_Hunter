using UnityEngine;

public interface IMoveable
{
    public abstract void MoveTowards(Vector2 targetPosition);
    public abstract void StopMoving();
}
