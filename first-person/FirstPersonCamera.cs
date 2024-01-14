using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public bool cursorLocked;

    [Tooltip("When this is on, the camera will control its own horizontal rotation.")]
    public bool controlPitch = true;
    [SerializeField] Transform playerTransform;

    [Header("Attributes")]
    public float sensitivity = 1.0f;
    [Range(0, 90)] public float yMax = 65f;
    [Range(-90, 0)] public float yMin = -65f;

    private void Start()
    {
        cursorLocked = true;
    }

    private void Update()
    {
        HandleCursor();
        HandleRotation();
    }

    /// <summary>
    /// Toggle the cursor with the Escape key.
    /// </summary>
    void HandleCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) cursorLocked = !cursorLocked;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    float yRot, xRot;
    /// <summary>
    /// When the cursor is locked, your camera can look up and down, 
    /// but make sure to toggle "controlPitch" for independent horizontal rotation.
    /// </summary>
    void HandleRotation()
    {
        if (!cursorLocked) return;

        yRot -= sensitivity * Input.GetAxis("Mouse Y");
        yRot = Mathf.Clamp(yRot, yMin, yMax);

        if (controlPitch)
        {
            xRot += sensitivity * Input.GetAxis("Mouse X");
        }

        transform.rotation = Quaternion.Euler(yRot, playerTransform != null ? playerTransform.eulerAngles.y : xRot, transform.eulerAngles.z);
    }
}
