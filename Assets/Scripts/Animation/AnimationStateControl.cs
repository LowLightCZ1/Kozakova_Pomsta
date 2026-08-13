using UnityEditor.Animations;
using UnityEngine;

public class AnimationStateControl : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool forwardPress = Input.GetKey("w");
        bool backwardPress = Input.GetKey("s");
        bool sprintPress = Input.GetKey(KeyCode.LeftShift);
        //bool chrouchPress = Input.GetKey(KeyCode.LeftControl);

        if (forwardPress)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (sprintPress)
        {
            animator.SetBool("IsSprinting", true);
        }
        else
        {
            animator.SetBool("IsSprinting", false);
        }

        //if (chrouchPress)
        //{
        //    animator.SetBool("IsChrouching", true);
        //}
        //else
        //{
        //    animator.SetBool("IsChrouching", false);
        //}
    }
}                                           



// Animation Settings//
// Allways set transform right
// Disable exit time and Enable loop on animations