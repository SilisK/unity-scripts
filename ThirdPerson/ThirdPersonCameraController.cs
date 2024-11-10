using System.Collections;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] 
    CinemachineFreeLook FreeLook;

    [SerializeField]
    [Range(0, 2.5f)]
    float HorizontalSensitivity = 0.38f;
    [SerializeField]
    [Range(0, 2.5f)]
    float VerticalSensitivity = 0.38f;

    [SerializeField]
    bool lockCursorOnStart = true;

    private void Start()
    {
        StartCoroutine(InitializeFreeLook());
        if(lockCursorOnStart) Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        ControlFreeLook();
        //HandleRadiusObstruction();
        Debug.Log("IsObstructionWithinRadius: " + IsObstructionWithinRadius());
    }

    /* 
     * To fix the camera clipping through the wall, its important to understand that the camera is orbiting around a large radius.
     * We need to project a sphere cast that can detect obstructions and shrink the radius when colliding.
     * We can return the radius back to default when the obstruction is no longer colliding with our check sphere.
     */

    public Vector3 castDirection = Vector3.zero;

    /// <summary>
    /// Checks if an obstruction is within the CircleCast radius.
    /// </summary>
    /// <returns>True if a collider is within the radius of the CircleCast.</returns>
    bool IsObstructionWithinRadius()
    {
        // Perform a CircleCast in the given direction
        float maxRadius = FreeLook_Radii.Max();
        var circleCast = Physics2D.CircleCast(transform.position, maxRadius, castDirection, 0, ~gameObject.layer);
        Debug.Log(circleCast.collider != null);
        return true;
    }

    /// <summary>
    /// Visualizes the CircleCast in the Scene View with semi-transparency.
    /// </summary>
    void OnDrawGizmos()
    {
        float maxRadius = FreeLook_Radii.Max();

        // Ensure the object has a valid max radius before drawing anything
        if (maxRadius > 0)
        {
            // Set the color of the Gizmo with transparency (alpha value < 1)
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);  // Green with 30% opacity

            // Draw the ray from the camera's position
            Gizmos.DrawLine(transform.position, transform.position + castDirection.normalized * maxRadius);

            // Draw a semi-transparent circle (represented as a wire sphere in 3D) at the end of the ray to represent the CircleCast radius
            Gizmos.DrawWireSphere(transform.position, maxRadius);
        }
    }

    float[] TrackedObjectOffsets = { 0.5f, 0.1f, 0.1f };
    /// <summary>
    /// Moves the camera's aim offset back to the center when IsObstructionWithinRadius is true.
    /// </summary>
    void HandleRadiusObstruction()
    {
        for(int i = 0; i < 3; i++)// We know that the length of rigs is always 3
        {
            var rig = FreeLook.GetRig(i);
            CinemachineComposer composer = rig.GetCinemachineComponent<CinemachineComposer>();
            Vector3 trackedObjectOffset = IsObstructionWithinRadius() ? Vector3.zero : new Vector3(TrackedObjectOffsets[0], TrackedObjectOffsets[1], TrackedObjectOffsets[2]);
            composer.m_TrackedObjectOffset = Vector3.Lerp(composer.m_TrackedObjectOffset, trackedObjectOffset, 50f * Time.deltaTime);
            if (Mathf.Approximately(composer.m_TrackedObjectOffset.x, 0)) composer.m_TrackedObjectOffset = Vector3.zero;
        }
    }

    float[] FreeLook_Heights = { 2.5f, 0.8f, -2.5f };
    float[] FreeLook_Radii = { 0.8f, 2.5f, 1.3f };
    IEnumerator InitializeFreeLook()
    {
        // Checks if FreeLook.m_Orbits height and radius are close enough to default values
        bool IsOrbitsDefault()
        {
            for(int i = 0; i < FreeLook.m_Orbits.Length; i++)
            {
                bool isCloseEnough = Mathf.Approximately(FreeLook.m_Orbits[i].m_Height, FreeLook_Heights[i]) &&
                    Mathf.Approximately(FreeLook.m_Orbits[i].m_Radius, FreeLook_Radii[i]);
                if (isCloseEnough == false)
                {
                    return false;
                }
            }

            // Directly assign values after passing approximation
            FreeLook.m_Orbits[0].m_Height = FreeLook_Heights[0];
            FreeLook.m_Orbits[0].m_Radius = FreeLook_Radii[0];

            FreeLook.m_Orbits[1].m_Height = FreeLook_Heights[1];
            FreeLook.m_Orbits[1].m_Radius = FreeLook_Radii[1];

            FreeLook.m_Orbits[2].m_Height = FreeLook_Heights[2];
            FreeLook.m_Orbits[2].m_Radius = FreeLook_Radii[2];
            return true;
        }

        FreeLook.m_XAxis.m_InputAxisName = null;
        FreeLook.m_YAxis.m_InputAxisName = null;

        FreeLook.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;

        float speed = 2.5f * Time.deltaTime;
        while (!IsOrbitsDefault())
        {
            for (int i = 0; i < FreeLook.m_Orbits.Length; i++)
            {
                FreeLook.m_Orbits[i].m_Height = Mathf.Lerp(FreeLook.m_Orbits[i].m_Height, FreeLook_Heights[i], speed);
                FreeLook.m_Orbits[i].m_Radius = Mathf.Lerp(FreeLook.m_Orbits[i].m_Radius, FreeLook_Radii[i], speed);
            }
            yield return null;  
        }
    }

    /// <summary>
    /// Allows the FreeLook camera to respond to mouse input while cursor is locked.
    /// </summary>
    void ControlFreeLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        FreeLook.m_XAxis.Value += Input.GetAxis("Mouse X") * HorizontalSensitivity;
        FreeLook.m_YAxis.Value -= Input.GetAxis("Mouse Y") * VerticalSensitivity / 250f;
    }
}
