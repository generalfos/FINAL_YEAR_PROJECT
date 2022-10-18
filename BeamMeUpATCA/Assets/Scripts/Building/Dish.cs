using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using BeamMeUpATCA.Extensions;

namespace BeamMeUpATCA
{
    public class Dish : Mendable, Moveable, Enterable, Stowable
    {
        #region Dish Initialization

        // Dish only allows single unit inside
        private Unit _unitInsideSlot;
        
        [field: Header("Dish Position")]
        [SerializeField] private float movementSpeed = 1.5f;
        [SerializeField] private JunctionBox currentJunction;

        private Queue<JunctionBox> _pathwayToGoalBox;
        private double _dishMovementPercent;
        
        [field: Header("Dish Rotation")]
        [SerializeField] private float rotationSpeed = 2.5f;
        [SerializeField] private GameObject turret;
        [SerializeField] private GameObject dish;
        
        private Quaternion _dishRotationStart = Quaternion.identity;
        private Quaternion _dishRotationEnd = Quaternion.identity;
        private float _dishRotationPercent;
        
        private Quaternion _turretRotationStart = Quaternion.identity;
        private Quaternion _turretRotationEnd = Quaternion.identity;
        private float _turretRotationPercent;
        
        private float _unstowedAltitude;
        private float _unstowedAzimuth;

        protected override void Awake()
        {
            base.Awake();
    
            _pathwayToGoalBox = new Queue<JunctionBox>();
            
            // If a junction is defined dock to it
            if (currentJunction)
            {
                currentJunction.Dock(this);
            
                // Update dish position to arrangement slot's position
                transform.position = currentJunction.ArrangementSlot.position;
            }
            else
            {
                Debug.LogWarning("JunctionBox is not set on dish", this);
            }

            // Default no units inside
            _unitInsideSlot = null;

            // Default Dish rotation values
            _dishRotationStart = _dishRotationEnd = dish.transform.localRotation;
            _turretRotationStart =_turretRotationEnd = turret.transform.localRotation;

            _dishMovementPercent = 1f;
            _dishRotationPercent = 1f;
            _turretRotationPercent = 1f;
            
            // Convert altitude to negative value as for world coordinate system.
            _unstowedAltitude = -dish.transform.localEulerAngles.z;
            _unstowedAzimuth = turret.transform.localEulerAngles.y;

            if (IsStowed) AltazCoordinates(90.0f, 0.0f);
        }

        protected override void Update()
        {
            base.Update();
            
            // Update calls for movement and rotation
            HandleRotation(Time.deltaTime);
            if (_dishRotationPercent >= 1f && _turretRotationPercent >= 1f)
            {
                HandleMovement(Time.deltaTime);
            }
        }

        #endregion // End of 'Dish Initialization'
        #region Rotating

        public void AltazCoordinates(float altitude, float azimuth)
        {
            // Store current value to compare and Quaternion.Slerp
            _dishRotationStart = dish.transform.localRotation;
            _turretRotationStart = turret.transform.localRotation;
            
            // Translate given altitude to the localRotation of dish and clamp rotation to 90 degrees.
            altitude = (360.0f - (altitude % 360f)) % 360f;
            altitude = altitude == 0 ? 0f : Mathf.Clamp(altitude, 270f, 360f);

            azimuth %= 360.0f;

            _dishRotationEnd = Quaternion.Euler(0, 0, (float) altitude);
            _turretRotationEnd = Quaternion.Euler(0, (float) azimuth, 0);
            
            // Comparision of Quaternion values
            const float tolerance = 0.00001f;
            bool dishComparison = (1 - Mathf.Abs(Quaternion.Dot(_dishRotationStart, _dishRotationEnd)) < tolerance);
            bool turretComparison = (1 - Mathf.Abs(Quaternion.Dot(_turretRotationStart, _turretRotationEnd)) < tolerance);

            // Reset percentage of rotation. If values are the same set job to 100% done.
            _dishRotationPercent = dishComparison ? 1f : 0f;
            _turretRotationPercent = turretComparison ? 1f : 0f;
        }

        private void HandleRotation(float time)
        {
            bool isDishRotating = _dishRotationPercent < 1f;
            bool isTurretRotating = _turretRotationPercent < 1f;
            
            if (isDishRotating)
            {
                _dishRotationPercent += time * rotationSpeed;
                _dishRotationPercent = Mathf.Clamp( _dishRotationPercent, 0f, 1f);
                dish.transform.localRotation =
                    Quaternion.Slerp(_dishRotationStart, _dishRotationEnd, _dishRotationPercent);
            }
            else
            {
                dish.transform.localRotation = _dishRotationEnd;
                _unstowedAltitude = IsStowed ? _unstowedAltitude : -dish.transform.localEulerAngles.z;
            }

            if (isTurretRotating)
            {
                _turretRotationPercent += time * rotationSpeed;
                _turretRotationPercent = Mathf.Clamp(_turretRotationPercent, 0, 1f);
                turret.transform.localRotation = 
                    Quaternion.Slerp(_turretRotationStart, _turretRotationEnd, _turretRotationPercent);
            }
            else
            {
                turret.transform.localRotation = _turretRotationEnd;
                _unstowedAzimuth = IsStowed ? _unstowedAzimuth : turret.transform.localEulerAngles.y;
            }

            if (!isDishRotating && !isTurretRotating && !IsStowed && _previousLockState.Item1)
            {
                IsLocked = _previousLockState.Item2;
                _previousLockState.Item1 = false;
            }
        }

        #endregion // End of 'Rotating'
        #region Moving

        private bool IsMoving { get; set; }
        private bool _isStartedMoving;
        private JunctionBox _currentGoalBox;
        
        public void Move(Unit unit, Transform positionAnchor)
        {
            // If the unit inside the array is not the one calling move, or the dish is locked return
            if (unit != _unitInsideSlot || IsLocked || IsMoving) return;
            
            JunctionBox goalBox = positionAnchor.gameObject.GetComponentInParent<JunctionBox>();

            // If goalBox is not a valid JunctionBox, return
            if (!goalBox) return;

            // Get pathway to goalBox
            _pathwayToGoalBox = goalBox.DockPath(currentJunction);

            _isStartedMoving = IsMoving = (_pathwayToGoalBox.Count > 0);
            
            // If is moving stow, else leave stowed state.
            IsStowed = IsMoving ? IsMoving : IsStowed;
        }

        private void HandleMovement(float time)
        {
            if (!IsMoving) return;
            
            // Dock all paths to goalBox
            if (_isStartedMoving)
            {
                _isStartedMoving = false;

                // First position of queue is array's currentJunction, so we skip it first.
                _currentGoalBox = _pathwayToGoalBox.Count > 0 ? _pathwayToGoalBox.Dequeue() : null;
                _dishMovementPercent = 1f;
            }
            
            // If there is any nodes in pathway queue move
            if (_currentGoalBox)
            {
                // Dish is if finished previous movement, look for next
                if (_dishMovementPercent >= 1f)
                {
                    currentJunction.Undock();
                    currentJunction = _currentGoalBox;
                    currentJunction.Dock(this);
                    
                    _currentGoalBox = _pathwayToGoalBox.Count > 0 ? _pathwayToGoalBox.Dequeue() : null;
                    _dishMovementPercent = 0f;
                }
                else
                {
                    Vector3 startPosition = currentJunction.ArrangementSlot.position;
                    Vector3 endPosition = _currentGoalBox.ArrangementSlot.position;
                    
                    float distance = Vector3.Distance(startPosition, endPosition);
                    _dishMovementPercent += time * (movementSpeed / distance);
                    _dishMovementPercent = Mathf.Clamp((float) _dishMovementPercent, 0, 1f);

                    transform.position =
                        Vector3.Lerp(startPosition, endPosition, (float) _dishMovementPercent);
                }
            }
            else
            {
                IsStowed = IsMoving = false;
                _dishMovementPercent = 1f;
            }
        }

        #endregion // End of 'Moving'
        #region Stowing

        // NOTE: EDITOR ONLY
        [field: SerializeField] private bool stowedState;
        private void OnValidate()
        {
            IsStowed = stowedState;
        }

        private bool _isStowed = false;
        public bool IsStowed
        {
            get => _isStowed;
            private set
            {
                // Toggle stow except if moving, in which case force true
                _isStowed = value || (IsMoving && IsMoving);
                
                // Update editor value
                stowedState = IsStowed;
                if (IsStowed)
                {
                    // Set Locked to true if IsStowed, else keep Lock state
                    if (!IsLocked)
                    {
                        _previousLockState = (true, IsLocked);
                        IsLocked = true;
                    }
                    // If it's stowed set altitude to 90f (directly up).
                    AltazCoordinates(90f, 0f);
                }
                else
                {
                    AltazCoordinates(_unstowedAltitude, _unstowedAzimuth);
                }
            }
        }

        public void ToggleStow() { IsStowed = !IsStowed; }

        #endregion // End of 'Stowing'
        #region Locking
        
        // First bool = a previous lock state is stored, second is the lock state
        private (bool, bool) _previousLockState;
        [field: SerializeField] public bool IsLocked { get; private set; }

        public void ToggleLock() { IsLocked = !IsLocked; }
        
        #endregion // End of 'Locking'

        #region Entering
        
        // If the dish enter slot is free add the unit to it
        public void Enter(Unit unit) { _unitInsideSlot ??= unit; }

        // Check if unit list contains the unit
        public bool IsInside(Unit unit) => unit == _unitInsideSlot;

        // If unit is inside the building remove them from the list.
        public void Leave(Unit unit) { if (IsInside(unit)) _unitInsideSlot = null; }

        #endregion // End of 'Entering'
        #region Mending
        
        public override void Mend(Unit unit)
        {
            // Only allow locking if the building isLocked
            if (IsLocked) base.Mend(unit);
        }
        
        #endregion // End of 'Mending'
    }
}