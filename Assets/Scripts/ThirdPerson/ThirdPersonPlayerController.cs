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
        if(Input.GetAxisRaw("Vertical") > 0.1f)
        {
            rotatationY = Mathf.LerpAngle(rotatationY, CameraTransform.eulerAngles.y, 0.1f);
            Rigidbody.rotation = Quaternion.Euler(0, rotatationY, 0);
            Rigidbody.MoveRotation(Rigidbody.rotation);
        } 
    }

    void FixedUpdate()
    {
        Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 movement = transform.forward * movementInput.z;

        if (movementInput.magnitude > 0)
        {
            Vector3 newPosition = Rigidbody.position + movement * movementSpeed * Time.fixedDeltaTime;
            Rigidbody.MovePosition(newPosition);
        }
    }


    void InitializeRigidbody()
    {
        Rigidbody = GetComponent<Rigidbody>();

        Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

    }
}
