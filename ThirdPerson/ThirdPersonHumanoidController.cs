using System.Collections.Generic;
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
            mainBlend = Mathf.Lerp(mainBlend, input.z, playerController.movementSpeed / 10);
        }
        else if(playerController.INPUT_Z_AND_X())
        {
            float xBlend = input.z > 0 ? Mathf.Abs(input.x) : -Mathf.Abs(input.x);
            mainBlend = Mathf.Lerp(mainBlend, input.z + xBlend, playerController.movementSpeed / 10);
        }
        else if (playerController.INPUT_X_ONLY())
        {
            mainBlend = Mathf.Lerp(mainBlend, Mathf.Abs(input.x), playerController.movementSpeed / 10);
        }
        else
        {
            mainBlend = Mathf.Lerp(mainBlend, 0, playerController.movementSpeed / 10);
        }
        animator.SetFloat("Main Blend", mainBlend);
        animator.SetInteger("State", playerController.state);
    }
}
