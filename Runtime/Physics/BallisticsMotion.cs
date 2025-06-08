﻿namespace UniGame.Runtime.Physics
{
    using System.Collections.Generic;
    using PlayerPhysicsMotion;
    using UnityEngine;

    public class BallisticsMotion : MonoBehaviour
    {

        private Rigidbody _rigidbody;
        private bool _isPrepared;
        private RigidbodyBallisticMotionCalculator _calculator;
        
        [SerializeField]
        public BallisticsMotionData Data;
        [SerializeField]
        private int _pointsCount = 20;
        [SerializeField]
        private float _timeShift = 0.1f;
        [SerializeField]
        private Vector3 _force = new Vector3(10, 10, 0);
        [SerializeField]
        private ForceMode _forceMode = ForceMode.Impulse;

        
        public List<Vector3> Trajectory = new List<Vector3>();

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            Data = new BallisticsMotionData();
            
            _calculator = new RigidbodyBallisticMotionCalculator(UnityEngine.Physics.gravity);
        }

        private void Update()
        {

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                if (_isPrepared)
                {
                    ApplyForce();
                }
                _isPrepared = false;
            }

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _isPrepared = true;
            }

            if (_isPrepared)
            {

                var position = UnityEngine.Input.mousePosition;
                position.z = 10;
                var worldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(position);
                
                UpdateTrajectory(_pointsCount, worldPosition);
            }
        }

        private void ApplyForce()
        {
            _rigidbody.AddForce(Data.Force, _forceMode);
        }

        private void UpdateTrajectory(int pointCount, Vector3 worldPosition)
        {

            Trajectory.Clear();

            var initialiPoint = transform.position;

            var forceDirection = worldPosition - initialiPoint;
            var normalizedPosition = forceDirection.normalized;

            Data.Force = Vector3.Scale(normalizedPosition, _force);
            Data.InitialPosition = initialiPoint;
            
            for (int i = 0; i < pointCount; i++) {
                
                Data.Time = (_timeShift * i) * UnityEngine.Time.fixedDeltaTime;
                
                var position = _calculator.GetPosition(Data);

                Trajectory.Add(position);
            }

        }


    }


}
