using UnityEngine;

namespace AnimParams
{
    public abstract class AnimParam
    {
        protected Animator animator;
        protected int paramId;

        protected AnimParam(Animator animator, string name)
        {
            this.animator = animator;
            paramId = Animator.StringToHash(name);
        }
    }

    public class AnimBoolParam : AnimParam
    {
        private bool boolValue;
        public bool BoolValue
        {
            get => boolValue;
            set
            {
                boolValue = value;
                animator.SetBool(paramId, boolValue);
            }
        }

        public AnimBoolParam(Animator animator, string name) : base(animator, name) { }
    }

    public class AnimIntParam : AnimParam
    {
        private int intValue;
        public int IntValue
        {
            get => intValue;
            set
            {
                intValue = value;
                animator.SetInteger(paramId, intValue);
            }
        }

        public AnimIntParam(Animator animator, string name) : base(animator, name) { }
    }

    public class AnimFloatParam : AnimParam
    {
        private float floatValue;
        public float FloatValue
        {
            get => floatValue;
            set
            {
                floatValue = value;
                animator.SetFloat(paramId, floatValue);
            }
        }

        public AnimFloatParam(Animator animator, string name) : base(animator, name) { }
    }

    public class AnimTriggerParam : AnimParam
    {
        public void SetTrigger()
        {
            animator.SetTrigger(paramId);
        }

        public AnimTriggerParam(Animator animator, string name) : base(animator, name) { }
    }

}