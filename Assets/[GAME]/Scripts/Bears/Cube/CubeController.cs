using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Manager;
using _GAME_.Scripts.Structs;
using _ORANGEBEAR_.EventSystem;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace _GAME_.Scripts.Bears.Cube
{
    public class CubeController : Bear, ICube
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private Renderer meshRenderer;

        #endregion

        #region Private Variables

        private BoxCollider _boxCollider;
        private Rigidbody _rigidbody;

        private bool _canStack;

        private Vector3 _target;
        private NativeArray<Vector3> _destinationArray;
        private NativeArray<bool> _isMoving;

        private Vector3 _previousPosition;
        private Vector3 _currentPosition;

        private PoolManager _poolManager;

        private RigidbodyConstraints _originalConstraints;

        private Color _orangeColor;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _rigidbody = GetComponent<Rigidbody>();

            _poolManager = PoolManager.Instance;

            _originalConstraints = _rigidbody.constraints;
            
            _orangeColor = new Color(1f, 0.5f, 0f);
        }

        private void FixedUpdate()
        {
            if (!_canStack)
            {
                return;
            }

            Vector3 cubePosition = transform.position;
            _destinationArray = new NativeArray<Vector3>(1, Allocator.TempJob);
            _isMoving = new NativeArray<bool>(1, Allocator.TempJob);
            MoveToCollectPointJob job = new MoveToCollectPointJob
            {
                target = _target,
                position = cubePosition,
                destination = _destinationArray
            };

            JobHandle jobHandle = job.Schedule();
            jobHandle.Complete();

            _rigidbody.velocity = _destinationArray[0].normalized * 15f;

            CalculateIsObjectMovingJob calculateIsObjectMovingJob = new CalculateIsObjectMovingJob
            {
                previousPosition = _previousPosition,
                currentPosition = cubePosition,
                isMoving = _isMoving
            };

            JobHandle calculateIsObjectMovingJobHandle = calculateIsObjectMovingJob.Schedule();
            calculateIsObjectMovingJobHandle.Complete();

            if (!_isMoving[0])
            {
                _canStack = false;
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }

            _previousPosition = cubePosition;
            
            _destinationArray.Dispose();
            _isMoving.Dispose();
        }

        #endregion

        #region Public Methods

        public void InitCube(Color color)
        {
            meshRenderer.material.color = color;
            _rigidbody.isKinematic = false;
            Transform cubeTransform = transform;
            cubeTransform.localEulerAngles = Vector3.zero;
            _boxCollider.enabled = true;
            _canStack = false;

            _previousPosition = cubeTransform.position;
        }

        public void ResetCube()
        {
            _rigidbody.constraints = _originalConstraints;
            _rigidbody.velocity = Vector3.zero;
            gameObject.layer = LayerMask.NameToLayer(GlobalStrings.Cube);
            _poolManager.cubePool.Release(this);
        }

        #endregion

        public void CubeInteractedWithTheCollectPoint(params object[] args)
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
            _target = (Vector3)args[0];
            _canStack = true;
            meshRenderer.material.color = _orangeColor;
            gameObject.layer = LayerMask.NameToLayer(GlobalStrings.StackedCube);

            Roar(CustomEvents.DecreaseCubeCount);
        }
    }
}