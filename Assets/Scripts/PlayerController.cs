
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runMultiplier = 1.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.2f;
    public Transform cameraTransform;
    public int maxHealth = 100;
    public int health;

    public GameObject meleePrefab;
    public GameObject rangedPrefab;

    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        string sel = PlayerPrefs.GetString("SelectedCharacter", "Kael");
        if (sel == "Kael")
        {
            moveSpeed = 6.5f;
            maxHealth = 110;
            if (meleePrefab != null) Instantiate(meleePrefab, transform);
        }
        else
        {
            moveSpeed = 4.5f;
            maxHealth = 90;
            if (rangedPrefab != null) Instantiate(rangedPrefab, transform);
        }
        health = maxHealth;
        UI.HUDController.Instance?.UpdateHealth(health, maxHealth);
        UI.HUDController.Instance?.UpdateCharacter(sel);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);
        float speed = moveSpeed * (running ? runMultiplier : 1f);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + (cameraTransform? cameraTransform.eulerAngles.y : 0f);
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            // Attack input; actual damage via animation event
            var animator = GetComponent<Animator>();
            if (animator != null) animator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        UI.HUDController.Instance?.UpdateHealth(health, maxHealth);
    }

    void Die()
    {
        var animator = GetComponent<Animator>();
        if (animator != null) animator.SetTrigger("Die");
        Debug.Log("Player died");
    }

    void Interact()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            var item = hit.collider.GetComponent<ItemPickup>();
            if (item != null)
            {
                item.Collect();
            }
        }
    }
}
