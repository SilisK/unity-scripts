using System.Collections;
using UnityEngine;
using Cinemachine;

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
