using UnityEngine;

namespace BeamMeUpATCA
{
    public class Dish : Mendable, Moveable, Enterable, Stowable
    {
        // Dish only allows single unit inside
        private Unit _unitInsideSlot;

        [field: Header("Dish Position")]
        [SerializeField] private JunctionBox currentJunction = null;
        
        [field: Header("Dish Rotation")]
        [SerializeField] private float rotationSpeed = 2.5f;
        [SerializeField] private GameObject turret = null;
        [SerializeField] private GameObject dish = null;
        
        private Quaternion _dishRotationStart = Quaternion.identity;
        private Quaternion _dishRotationEnd = Quaternion.identity;
        private float _dishRotationPercent = 0f;
        
        private Quaternion _turretRotationStart = Quaternion.identity;
        private Quaternion _turretRotationEnd = Quaternion.identity;
        private float _turretRotationPercent = 0f;
        
        private float _unstowedAltitude = 0f;
        private float _unstowedAzimuth = 0f;

        protected override void Awake()
        {
            base.Awake();

            // TODO: Change to call move function.
            transform.localPosition = currentJunction is null
                ? transform.localPosition
                : currentJunction.ArrangementSlot.transform.position;

            _unitInsideSlot = null;

            _dishRotationStart = _dishRotationEnd = dish.transform.localRotation;
            _dishRotationPercent = 1f;
            
            _turretRotationStart =_turretRotationEnd = turret.transform.localRotation;
            _turretRotationPercent = 1f;
            
            // Convert altitude to negative value as for world coordinate system.
            _unstowedAltitude = -dish.transform.localEulerAngles.z;
            _unstowedAzimuth = turret.transform.localEulerAngles.y;

            if (IsStowed) AltazCoordinates(90f, _unstowedAzimuth);
        }

        protected override void Update()
        {
            base.Update();
            StowHanding();

            if (_dishRotationPercent < 1f)
            {
                _dishRotationPercent += Time.deltaTime * rotationSpeed;
                _dishRotationPercent = Mathf.Clamp(_dishRotationPercent, 0, 1f);
                dish.transform.localRotation =
                    Quaternion.Slerp(_dishRotationStart, _dishRotationEnd, _dishRotationPercent);
            }

            if (_turretRotationPercent < 1f)
            {
                _turretRotationPercent += Time.deltaTime * rotationSpeed;
                _turretRotationPercent = Mathf.Clamp(_turretRotationPercent, 0, 1f);
                turret.transform.localRotation = 
                    Quaternion.Slerp(_turretRotationStart, _turretRotationEnd, _turretRotationPercent);
            }
        }

        public void AltazCoordinates(float altitude, float azimuth)
        {
            // Store current value to compare and Quaternion.Slerp
            _dishRotationStart = dish.transform.localRotation;
            _turretRotationStart = turret.transform.localRotation;
            
            // Convert altitude to negative value as for world coordinate system.
            _dishRotationEnd = Quaternion.Euler(0, 0, -altitude);
            _turretRotationEnd = Quaternion.Euler(0, azimuth, 0);

            // Reset percentage of rotation. If values are the same set job to 100% done.
            _dishRotationPercent = _dishRotationStart == _dishRotationEnd ? 1f : 0f;
            _turretRotationPercent = _turretRotationStart == _turretRotationEnd ? 1f : 0f;
        }

        // If the dish enter slot is free add the unit to it
        public void Enter(Unit unit) { _unitInsideSlot ??= unit; }

        // Check if unit list contains the unit
        public bool IsInside(Unit unit) => unit == _unitInsideSlot;

        // If unit is inside the building remove them from the list.
        public void Leave(Unit unit) { if (IsInside(unit)) _unitInsideSlot = null; }

        public void Move(Unit unit)
        {
            // If the unit inside the array is not the one calling move, or the dish is locked return
            if (unit != _unitInsideSlot || IsLocked) return;
            
            throw new System.NotImplementedException();
        }

        [field: SerializeField] public bool IsStowed { get; private set; } = false;
        public void ToggleStow() { IsStowed = !IsStowed; }

        private void StowHanding()
        {
            // If it's stowed set altitude to 90f (directly up), else set it to previous state.
            AltazCoordinates(IsStowed ? 90f : _unstowedAltitude, _unstowedAzimuth);
            
            // Set Locked to true if IsStowed, else keep Lock state
            IsLocked = IsStowed ? IsStowed : IsLocked;
            
            if (IsStowed) return;
            // If rotation is in a final state store its changes.
            if (_dishRotationPercent >= 1f) _unstowedAltitude = -dish.transform.localEulerAngles.z;
            if (_dishRotationPercent >= 1f) _unstowedAzimuth = turret.transform.localEulerAngles.y;
        }

        public bool IsLocked { get; private set; }

        public void ToggleLock(Unit unit) { IsLocked = !IsLocked; }

        public override void Mend(Unit unit)
        {
            // Only allow locking if the building isLocked
            if (IsLocked) base.Mend(unit);
        }
    }
}