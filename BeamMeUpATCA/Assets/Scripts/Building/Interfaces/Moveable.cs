namespace BeamMeUpATCA
{
    public interface Moveable
    {
        // Takes a Unit and handles it moving the implementing class.
        void Move(Unit unit); 

        // Takes a Unit and toggles a lock on Move() functionality on an implementing class.
        void ToggleLock(Unit unit);
        bool IsLocked { get; }
    }
}