using UnityEngine;

public class BorderScript : MonoBehaviour
{
    [SerializeField]
    private Vector2 _AmountToMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.attachedRigidbody.MovePosition(collision.attachedRigidbody.position + _AmountToMove);
    }
}
