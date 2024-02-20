namespace Playmode.ServerEnteties
{
    public class TurnCycleData
    {
        public ThrowCubesResult LastThrowCubesResult 
        { 
            get => _lastThrowCubesResult; 
            set
            {
                if(value == null)
                {
                    UnityEngine.Debug.LogError("Try to set null in LastThrowCubesResult");
                }
                else
                {
                    if (value.HasSameResults()) SameCubesResultsCount++;
                    _lastThrowCubesResult = value;
                }
            }
        }
        public int SameCubesResultsCount { get; set; }

        private ThrowCubesResult _lastThrowCubesResult;
    }
}
