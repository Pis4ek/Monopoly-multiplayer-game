namespace Playmode
{
    [System.Serializable]
    public class ThrowCubesResult
    {
        public int ResultSum => Cube1Result + Cube2Result;
        public bool IsDouble => Cube1Result == Cube2Result;

        public PlayerID Thrower;
        public int Cube1Result;
        public int Cube2Result;

        public ThrowCubesResult(PlayerID thrower)
        {
            Thrower = thrower;
            Cube1Result = UnityEngine.Random.Range(1, 7);
            Cube2Result = UnityEngine.Random.Range(1, 7);
        }

        public ThrowCubesResult(PlayerID thrower, int cube1Result, int cube2Result)
        {
            Thrower = thrower;
            Cube1Result = cube1Result;
            Cube2Result = cube2Result;
        }

        public bool HasSameResults() => Cube1Result == Cube2Result;

        public override string ToString()
        {
            return $"ThrowCubesResult: {Thrower} has thrown {Cube1Result} and {Cube2Result}";
        }
    }
}
