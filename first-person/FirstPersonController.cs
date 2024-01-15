using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] FirstPersonCamera firstPersonCamera;

    public bool usePhysics = true;

    [SerializeField] Rigidbody rigidBody;

    [Header("Attributes")]
    public float movementSpeed = 2.5f;
    [Range(0, 10)] public float maxRigidbodyVelocity = 5f;

    [Header("Input Options")]
    [Tooltip("Raw input instantly tracks input instead of smoothly lerping it.")] 
    public bool useRawInput;

    private void FixedUpdate()
    {
        if(usePhysics && rigidBody != null)
        {
            TranslatePositionPhysics();
        }
    }

    private void Update()
    {
        if(!usePhysics)
        {
            TranslatePosition();
        }

        if(firstPersonCamera != null)
        {
            HandleRotation();
        }
    }

    /// <summary>
    /// Useful for free roam cameras.
    /// </summary>
    void TranslatePosition()
    {
        float inputX = (useRawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal")) * movementSpeed;
        float inputY = (useRawInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical")) * movementSpeed;

        transform.Translate(Vector3.right * inputX * Time.deltaTime);
        transform.Translate(Vector3.forward * inputY * Time.deltaTime);
    }

    /// <summary>
    /// Useful for collisions/player characters.
    /// </summary>
    void TranslatePositionPhysics()
    {
        float inputX = (useRawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal")) * movementSpeed;
        float inputY = (useRawInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical")) * movementSpeed;

        rigidBody.AddForce(transform.forward * inputY, ForceMode.VelocityChange);
        rigidBody.AddForce(transform.right * inputX, ForceMode.VelocityChange);

        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxRigidbodyVelocity);
    }

    /// <summary>
    /// When a camera script is attached, horizontal rotation is dictated by that camera.
    /// </summary>
    void HandleRotation()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
            firstPersonCamera.transform.eulerAngles.y,
            transform.eulerAngles.z);
    }
}
