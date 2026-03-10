using UnityEngine;

public class CustomerWander : MonoBehaviour
{
    [Header("Wondering")]
    public float moveSpeed = 1.5f;       
    public float wanderRadius = 4.0f;    
    public float waitTime = 2.0f;        

    private Vector2 startPosition;      
    private Vector2 targetPosition;      
    private float waitTimer;             
    private Animator animator;

    private void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();

        PickNewDestination();
    }

    private void Update()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            if (animator != null) animator.SetBool("isMoving", false);
            return;
        }

        Vector2 currentPos = transform.position;
        Vector2 direction = targetPosition - currentPos;

        transform.position = Vector2.MoveTowards(currentPos, targetPosition, moveSpeed * Time.deltaTime);

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.up = direction.normalized;
            if (animator != null) animator.SetBool("isMoving", true);
        }

        if (Vector2.Distance(currentPos, targetPosition) < 0.1f)
        {
            waitTimer = waitTime; 
            PickNewDestination(); 
        }
    }

    private void PickNewDestination()
    {
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        targetPosition = startPosition + randomOffset;
    }
}