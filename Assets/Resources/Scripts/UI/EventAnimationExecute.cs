using UnityEditor.Animations;
using UnityEngine;

namespace Tcp4
{
    public class AnimationExecute: MonoBehaviour
    {
        public Animator anim;

        public void ExecuteAnimation(string parameter)
        {
            anim.Play(parameter);
        }


    }
}
