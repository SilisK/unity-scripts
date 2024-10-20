using UnityEngine;
using Cinemachine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] 
    CinemachineFreeLook FreeLook;

    [SerializeField] 
    [Range(0, 10)] 
    float Sensitivity = 2.5f;

    [SerializeField]
    bool lockCursorOnStart = true;

    private void Start()
    {
        InitializeFreeLook();
        if(lockCursorOnStart) Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ?
                CursorLockMode.None : CursorLockMode.Locked;
        }

        ControlFreeLook();
    }

    void InitializeFreeLook()
    {
        FreeLook.m_XAxis.m_InputAxisName = null;
        FreeLook.m_YAxis.m_InputAxisName = null;

        FreeLook.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;

        FreeLook.m_Orbits[0].m_Height = 4.5f;
        FreeLook.m_Orbits[0].m_Radius = 1.3f;

        FreeLook.m_Orbits[1].m_Height = 1;
        FreeLook.m_Orbits[1].m_Radius = 3.5f;

        FreeLook.m_Orbits[2].m_Height = -4.5f;
        FreeLook.m_Orbits[2].m_Radius = 1.3f;
    }

    /// <summary>
    /// Allows the FreeLook camera to respond to mouse input while cursor is locked.
    /// </summary>
    void ControlFreeLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        FreeLook.m_XAxis.Value += Input.GetAxis("Mouse X") * Sensitivity;
        FreeLook.m_YAxis.Value -= Input.GetAxis("Mouse Y") * Sensitivity / 250f;
    }
}
