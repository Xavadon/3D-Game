using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBoolOnEnter : StateMachineBehaviour
{
    [SerializeField] private string[] _name;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < _name.Length; i++)
        {
            animator.SetBool(_name[i], false);
        }
    }
}
