using UnityEngine;

public class ComboAttackController : StateMachineBehaviour
{
    //private PlayerController plc;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("combo", false);
        animator.SetBool("attack", false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool isInComboWindow = stateInfo.normalizedTime > 0.5f && stateInfo.normalizedTime < 0.9f;
        if (animator.GetBool("attack") && isInComboWindow)
        {
            animator.SetBool("combo", true);
            animator.SetBool("attack", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.SetBool("combo", false);
       animator.SetBool("attack", false);
    }
}

