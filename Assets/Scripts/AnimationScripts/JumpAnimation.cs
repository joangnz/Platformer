using UnityEngine;

public class JumpAnimation : StateMachineBehaviour
{
    // Possibly wrong
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.ResetTrigger("Jump");
    }
}
