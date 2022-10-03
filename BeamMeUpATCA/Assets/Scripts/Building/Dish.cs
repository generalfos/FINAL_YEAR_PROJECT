using UnityEngine;

namespace BeamMeUpATCA
{
    public class Dish : Mendable, Moveable, Enterable, Stowable
    {
        public void Enter(Unit unit)
        {
            throw new System.NotImplementedException();
        }

        public bool IsInside(Unit unit)
        {
            throw new System.NotImplementedException();
        }

        public void Leave(Unit unit)
        {
            throw new System.NotImplementedException();
        }

        public void Move(Unit unit)
        {
            throw new System.NotImplementedException();
        }

        public void Stow(Unit unit)
        {
            throw new System.NotImplementedException();
        }

        public bool ToggleLock(Unit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}