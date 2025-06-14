using UnityEngine;

public class HitToBodyController : StateMachineBehaviour
{
    //private PlayerController plc;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("combo", false);
        animator.SetBool("attack", false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("combo", false);
        animator.SetBool("attack", false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.SetBool("combo", false);
       animator.SetBool("attack", false);
    }
}

