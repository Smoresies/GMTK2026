using UnityEngine;

public class MoveToPlayerAi : MovementAi
{
    [SerializeField]
    private float stopDistance;
    public override Vector3 GetMoveDirection()
    {
        // Move towards the player
        if ((player.position - self.position).magnitude < stopDistance)
        {
            return Vector3.zero;
        }
        else
        {
            return (player.position - self.position).normalized;
        }
    }
}
