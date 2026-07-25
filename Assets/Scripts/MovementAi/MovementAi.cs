using UnityEngine;

public abstract class MovementAi : MonoBehaviour
{
    protected Transform player;
    protected Transform self;

    public virtual void initialize(Transform playerTransform, Transform selfTransform)
    {
        player = playerTransform;
        self = selfTransform;
        Debug.Log("MovementAi initialized with player: " + player.name + " and enemy: " + self.name);
    }

    public virtual Vector3 GetMoveDirection()
    {
        return Vector3.zero;
    }

    public virtual void OnCollision(Collision2D collision)
    {
        
    }

}
