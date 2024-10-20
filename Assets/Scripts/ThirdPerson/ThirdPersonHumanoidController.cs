using UnityEngine;

[RequireComponent (typeof(Animator))]
public class ThirdPersonHumanoidController : MonoBehaviour
{
    Animator animator;

    [SerializeField] ThirdPersonPlayerController playerController;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    float mainBlend;
    private void Update()
    {
        if(playerController == null)
        {
            Debug.LogWarning("This gameObject is trying to access a ThirdPersonPlayerController. Read https://github.com/SilisK/unity-scripts for more info.");
            return;
        }

        Vector3 input = playerController.MovementInput();
        if (playerController.INPUT_Z_ONLY())
        {
            mainBlend = Mathf.Lerp(mainBlend, input.z, 0.1f);
        }
        else if(playerController.INPUT_Z_AND_X())
        {
            float xBlend = input.z > 0 ? Mathf.Abs(input.x) : -Mathf.Abs(input.x);
            mainBlend = Mathf.Lerp(mainBlend, input.z + xBlend, 0.1f);
        }
        else if (playerController.INPUT_X_ONLY())
        {
            mainBlend = Mathf.Lerp(mainBlend, Mathf.Abs(input.x), 0.1f);
        }
        else
        {
            mainBlend = Mathf.Lerp(mainBlend, 0, 0.1f);
        }
        animator.SetFloat("Main Blend", mainBlend);
    }
}
