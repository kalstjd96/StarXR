using UnityEngine;

namespace Starxr.SDK.AI.Components
{
    internal class AnimatorHandler : MonoBehaviour
    {
        private readonly Animator animator;
        private readonly string parameterName;

        public AnimatorHandler(Animator animator, string parameterName)
        {
            this.animator = animator;
            this.parameterName = parameterName;
        }

        public void PlayAnimation()
        {
            if (animator == null)
            {
                Debug.LogError("Animator가 할당되지 않았습니다.");
                return;
            }
            animator.SetBool(parameterName, true);
        }

        public void StopAnimation()
        {
            if (animator == null)
            {
                Debug.LogError("Animator가 할당되지 않았습니다.");
                return;
            }
            animator.SetBool(parameterName, false);
        }

    }

}
