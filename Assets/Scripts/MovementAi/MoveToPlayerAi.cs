using UnityEngine;

public class MoveToPlayerAi : MovementAi
{
    [SerializeField]
    private float stopDistance;
    public override Vector3 GetMoveDirection()
    {
        // Move towards the player
        if (IsWithinStopDistance())
        {
            return Vector3.zero;
        }
        else
        {
            return (player.position - self.position).normalized;
        }
    }

    public bool IsWithinStopDistance()
    {
        return (player.position - self.position).magnitude < stopDistance;
    }
}
