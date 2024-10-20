using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonPlayerController : MonoBehaviour
{
    Rigidbody Rigidbody;

    [SerializeField]
    Transform CameraTransform;

    [SerializeField] 
    float movementSpeed = 5f;

    private void Start()
    {
        InitializeRigidbody();
    }

    float rotatationY;
    private void Update()
    {
        if (INPUT_Z_ONLY())
        {
            rotatationY = Mathf.LerpAngle(rotatationY, CameraTransform.eulerAngles.y, 0.1f);
        }
        else if (INPUT_Z_AND_X())
        {
            float angle = 45f * MovementInput().x * MovementInput().z;
            rotatationY = Mathf.LerpAngle(rotatationY, CameraTransform.eulerAngles.y + angle, 0.1f);
        }
        else if(INPUT_X_ONLY())
        {
            float angle = 75f * MovementInput().x;
            rotatationY = Mathf.LerpAngle(rotatationY, CameraTransform.eulerAngles.y + angle, 0.1f);
        }
        Rigidbody.rotation = Quaternion.Euler(0, rotatationY, 0);
    }

    void FixedUpdate()
    {
        if(MovementInput().magnitude > 0)
        {
            Rigidbody.MovePosition(MovementPosition());
        }
    }

    /// <summary>
    /// Returns the movement input of this gameObject on the X and Z axes.
    /// </summary>
    public Vector3 MovementInput() => new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

    public bool INPUT_Z_ONLY() => Mathf.Abs(MovementInput().z) > 0 && MovementInput().x == 0;
    public bool INPUT_Z_AND_X() => Mathf.Abs(MovementInput().z) > 0 && Mathf.Abs(MovementInput().x) > 0;
    public bool INPUT_X_ONLY() => MovementInput().z == 0 && Mathf.Abs(MovementInput().x) > 0;

    Vector3 MovementPosition()
    {
        if (INPUT_Z_ONLY() || INPUT_Z_AND_X())
        {
            Vector3 direction = (transform.forward * MovementInput().z);
            return Rigidbody.position + direction * movementSpeed * Time.fixedDeltaTime;
        }
        else if (INPUT_X_ONLY())
        {
            Vector3 direction = (transform.forward * Mathf.Abs(MovementInput().x));
            return Rigidbody.position + direction * movementSpeed * Time.fixedDeltaTime;
        }
        return Rigidbody.position;
    }

    void InitializeRigidbody()
    {
        Rigidbody = GetComponent<Rigidbody>();

        Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
