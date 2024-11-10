using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonPlayerController : MonoBehaviour
{
    Rigidbody Rigidbody;

    [SerializeField]
    Transform CameraTransform;

    [SerializeField]
    GameObject freeLookGameObject;

    [Range(0, 10)]
    public float movementSpeed = 2.78f;

    public int state = 0;

    private IEnumerator Start()
    {
        InitializeRigidbody();

        yield return new WaitForSeconds(1f);

        freeLookGameObject.SetActive(true);
    }

    float rotatationY;
    private void Update()
    {
        HandleState();
        HandleRotation();
    }

    void FixedUpdate()
    {
        if(MovementInput().magnitude > 0 && Cursor.lockState == CursorLockMode.Locked)
        {
            Rigidbody.MovePosition(MovementPosition());
        }
    }

    /// <summary>
    /// Key = { Standing: 0, Crouching: 1 }
    /// </summary>
    void HandleState()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (state != 1)
            {
                state = 1;
            }
            else
            {
                state = 0;
            }
        }
    }

    void HandleRotation()
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
        else if (INPUT_X_ONLY())
        {
            float angle = 75f * MovementInput().x;
            rotatationY = Mathf.LerpAngle(rotatationY, CameraTransform.eulerAngles.y + angle, 0.1f);
        }
        Rigidbody.rotation = Quaternion.Euler(0, rotatationY, 0);
    }

    /// <summary>
    /// Returns the movement input of this gameObject on the X and Z axes.
    /// </summary>
    public Vector3 MovementInput() => new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

    public bool INPUT_Z_ONLY() => Mathf.Abs(MovementInput().z) > 0 && MovementInput().x == 0;
    public bool INPUT_Z_AND_X() => Mathf.Abs(MovementInput().z) > 0 && Mathf.Abs(MovementInput().x) > 0;
    public bool INPUT_X_ONLY() => MovementInput().z == 0 && Mathf.Abs(MovementInput().x) > 0;

    Vector3 MovementPosition()
    {
        Vector3 input = MovementInput();
        float movementDivisor = state == 1 ? 2.75f : 1f;
        float speed = (movementSpeed / movementDivisor) * Time.fixedDeltaTime;
        if (INPUT_Z_ONLY() || INPUT_Z_AND_X())
        {
            Vector3 direction = transform.forward * input.z;
            return Rigidbody.position + direction * speed;
        }
        else if (INPUT_Z_AND_X())
        {
            Vector3 direction = transform.forward * input.z + transform.right * input.x;
            return Rigidbody.position + direction * speed;
        }
        else if (INPUT_X_ONLY())
        {
            Vector3 direction = transform.forward * Mathf.Abs(input.x);
            return Rigidbody.position + direction * speed;
        }
        return Rigidbody.position;
    }

    void InitializeRigidbody()
    {
        Rigidbody = GetComponent<Rigidbody>();

        Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
