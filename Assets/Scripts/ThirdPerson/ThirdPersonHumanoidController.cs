using UnityEngine;

public class ThirdPersonHumanoidController : MonoBehaviour
{
    [SerializeField] Animator animator;
    private void Update()
    {
        animator.SetFloat("Main Blend", Input.GetAxis("Vertical"));
    }
}
