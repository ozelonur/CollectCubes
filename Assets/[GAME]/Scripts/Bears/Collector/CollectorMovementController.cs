using _GAME_.Scripts.Bears.CustomInput;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Manager;
using _GAME_.Scripts.ScriptableObjects;
using _GAME_.Scripts.Structs;
using _ORANGEBEAR_.EventSystem;
using _ORANGEBEAR_.Scripts.Managers;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace _GAME_.Scripts.Bears.Collector
{
    public class CollectorMovementController : Bear
    {
        #region Private Methods

        private CollectorSettingsScriptableObject _collectorSettings;
        private Joystick _joystick;

        private bool _canMove;
        private float _speed;

        private float _inputX;
        private float _inputZ;

        private float _joystickMagnitude;

        private Vector3 _destination;
        private Vector3 _lookDirection;
        private Quaternion _lookRotation;

        private GameManager _gameManager;

        private NativeArray<Vector3> _destinationArray;
        private readonly Transform[] _transformArray = new Transform[1];
        private TransformAccessArray _transformAccessArray;
        
        private Rigidbody _rigidbody;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collectorSettings = Resources.Load<CollectorSettingsScriptableObject>(FolderPaths.CollectorSettings);
            _joystick = JoystickManager.Instance.Joystick;
            _gameManager = GameManager.Instance;
            _transformArray[0] = transform;
            _transformAccessArray = new TransformAccessArray(_transformArray);
            _destinationArray = new NativeArray<Vector3>(1, Allocator.TempJob);
        }

        private void Start()
        {
            _speed = _collectorSettings.speed;
        }

        private JobHandle _jobHandle;
        
        
        private void FixedUpdate()
        {
            _rigidbody.velocity = Vector3.zero;
            if (_gameManager.isGameEnded && !_gameManager.isGameStarted)
            {
                return;
            }

            if (!_canMove)
            {
                return;
            }

            if (_joystick.Direction.magnitude <= _collectorSettings.responseThreshold)
            {
                return;
            }

            _inputX = _joystick.Direction.x;
            _inputZ = _joystick.Direction.y;

            _joystickMagnitude = _joystick.Direction.magnitude;

            if (_joystickMagnitude <= .2f)
            {
                _joystickMagnitude = .2f;
            }
            
            
            CalculateDestinationJob movementJob = new CalculateDestinationJob
            {
                inputX = _inputX,
                inputZ = _inputZ,
                joystickMagnitude = _joystickMagnitude,
                speed = _speed,
                destination = _destinationArray,
                deltaTime = Time.fixedDeltaTime,
                rotationSpeed = _collectorSettings.rotationSpeed
            };
            
            JobHandle handle = movementJob.Schedule(_transformAccessArray);
            
            handle.Complete();
            
            _rigidbody.velocity = movementJob.destination[0];

        }

        private void OnDestroy()
        {
            _transformAccessArray.Dispose();
            
            _destinationArray.Dispose();
        }

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(CustomEvents.CollectorCanMove, CollectorCanMove);
            }

            else
            {
                UnRegister(CustomEvents.CollectorCanMove, CollectorCanMove);
            }
        }

        private void CollectorCanMove(object[] args)
        {
            _canMove = (bool)args[0];
        }

        #endregion
    }
}