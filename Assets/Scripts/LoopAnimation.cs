using UnityEngine;

public class LoopAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("loadinganimation", 0, 0); // Play animation
        animator.SetBool("Looping", true); // Ensure looping if needed
    }
}
