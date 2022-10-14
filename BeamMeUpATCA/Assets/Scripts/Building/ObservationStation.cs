namespace BeamMeUpATCA
{
    public class ObservationStation : Mendable, Enterable, Workable
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

        public void Work(Unit unit)
        {
            throw new System.NotImplementedException();
        }

        public Unit WorkingUnit { get; set; }
        public void Rest(Unit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}