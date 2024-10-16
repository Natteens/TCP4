using UnityEngine;

namespace CrimsonReaper
{
    public abstract class State : IState
    {
        protected DynamicEntity entity;
        protected AnimationStateParameter[] animationParameters = null;

        public virtual void Initialize(DynamicEntity entity)
        {
            this.entity = entity;
            ConfigureAnimationParameters();
        }

        public virtual void DoEnterLogic()
        {
            DoChecks();
            ApplyEnterAnimations();
        }

        public virtual void DoExitLogic()
        {
            ResetValues();
            ApplyExitAnimations();
        }

        protected virtual void ConfigureAnimationParameters() { }
        public virtual void DoFrameUpdateLogic() { }
        public virtual void DoPhysicsLogic() { DoChecks(); }
        public virtual void DoChecks() { }
        public virtual void ResetValues() { }

        private void ApplyEnterAnimations()
        {
            if (animationParameters != null)
            {
                foreach (var param in animationParameters)
                {
                    param.ApplyEnter(entity.anim);
                    //Debug.Log($"Aplicando parâmetro de animação: {param.GetType().Name}");
                }
            }
        }

        private void ApplyExitAnimations()
        {
            if (animationParameters != null)
            {
                foreach (var param in animationParameters)
                {
                    param.ApplyExit(entity.anim);
                }
            }
        }
    }
}
