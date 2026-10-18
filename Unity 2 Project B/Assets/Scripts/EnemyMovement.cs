using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float distance = 5f;

    private Vector3 startPosition;
    private bool movingForward = true;

    // this puts the enemy in its starting position
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (movingForward)
        {
            // move the enemy
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // once it hits the set distance stop
            if (Vector3.Distance(startPosition, transform.position) >= distance)
            {
                movingForward = false;
            }
        }
        else
        {
            // move back
            transform.Translate(Vector3.back * speed * Time.deltaTime);

            // once back stop
            if (Vector3.Distance(startPosition, transform.position) <= 0.1f)
            {
                movingForward = true;
            }
        }
    }
}
