using System.Collections.Generic;

namespace BeamMeUpATCA
{
    public interface Enterable
    {
        // Takes a Unit and handles it entering the implementing class.
        // MUST set Unit.IsInsideBuilding() to True if successful enter.
        void Enter(Unit unit);
        
        bool IsInside(Unit unit);
        
        // MUST set Unit.IsInsideBuilding() to False if successful leave.
        void Leave(Unit unit);
    }
}