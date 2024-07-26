namespace UniModules.UniCore.Runtime.Physics
{
    using UnityEngine;

    public class Rigidbody2DVelocityLimiter : MonoBehaviour
    {

        private float _sqrMaxValocity;
        public float _sqrMinVelocity;

        public float MinVelocity;

        public float MaxVelocity;

        public bool UseMaxValocity;

        public bool UseMinValocity;

        public Rigidbody2D Rigidbody2D;

        public void FixedUpdate()
        {

            if (UseMaxValocity && Rigidbody2D.linearVelocity.sqrMagnitude > _sqrMaxValocity)
            {
                Rigidbody2D.linearVelocity = Rigidbody2D.linearVelocity.normalized * MaxVelocity;
            }

            if (UseMinValocity && Rigidbody2D.linearVelocity.sqrMagnitude < _sqrMinVelocity)
            {
                Rigidbody2D.linearVelocity = Rigidbody2D.linearVelocity.normalized * MinVelocity;
            }

        }

        private void Awake()
        {

            Rigidbody2D = Rigidbody2D == null ? GetComponent<Rigidbody2D>() : Rigidbody2D;
            _sqrMinVelocity = MinVelocity * MinVelocity;
            _sqrMaxValocity = MaxVelocity * MaxVelocity;
        }

    }
}
