using UnityEngine;

public class MoveToPlayerAi : MovementAi
{
    public override Vector3 GetMoveDirection()
    {
        // Move towards the player
        if ((player.position - self.position).magnitude < 0.1f)
        {
            return Vector3.zero;
        }
        else
        {
            return (player.position - self.position).normalized;
        }
    }
}
