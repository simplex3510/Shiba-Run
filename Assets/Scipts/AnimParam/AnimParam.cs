using UnityEngine;

namespace AnimParams
{
    public abstract class AnimParam
    {
        protected Animator animator;
        protected int id;

        protected AnimParam(Animator animator, string name)
        {
            this.animator = animator;
            id = Animator.StringToHash(name);
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
                animator.SetBool(id, value);
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
                animator.SetInteger(id, value);
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
                animator.SetFloat(id, value);
            }
        }

        public AnimFloatParam(Animator animator, string name) : base(animator, name) { }
    }
    public class AnimTriggerParam : AnimParam
    {
        public void SetTrigger()
        {
            animator.SetTrigger(id);
        }

        public AnimTriggerParam(Animator animator, string name) : base(animator, name) { }
    }

}