using UnityEngine;

public class AttackAnims : MonoBehaviour
{
    private bool AttackState;

    public void PlayAttackAnim(Animator Animator, string Attack1, string Attack2)
    {
        if (!AttackState || Attack2 == null)
        {
            Animator.SetTrigger(Attack1);
            AttackState = true;
        }

        else if (AttackState && Attack2 != null)
        {
            Animator.SetTrigger(Attack2);
            AttackState = false;
        }
    }
}
