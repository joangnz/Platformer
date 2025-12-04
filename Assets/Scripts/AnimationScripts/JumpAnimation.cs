using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public Animator an;

    // This will be called by the animation event
    public void OnAnimationEnd()
    {
        // Change the bool parameter in the Animator
        an.SetBool("jump", false);
    }
}