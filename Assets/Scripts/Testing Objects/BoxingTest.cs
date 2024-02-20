using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxingTest : MonoBehaviour
{
    [SerializeField] int count;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var stopWatch = new Stopwatch();
            var tstClass = new TSTClass();

            stopWatch.Start();
            for(int i = 0; i < count; i++)
            {
                var t = (TSTInterface)tstClass;
            }
            stopWatch.Stop();
            UnityEngine.Debug.Log($"Casting speed: {stopWatch.ElapsedTicks/10000f}");

            var interf = (TSTInterface)tstClass;

            stopWatch.Restart();
            for (int i = 0; i < count; i++)
            {
                var t = (TSTClass)interf;
            }
            stopWatch.Stop();
            UnityEngine.Debug.Log($"Upcasting speed: {stopWatch.ElapsedTicks / 10000f}");

            stopWatch.Restart();
            for (int i = 0; i < count; i++)
            {
                var t = (object)tstClass;
            }
            stopWatch.Stop();
            UnityEngine.Debug.Log($"Boxing speed: {stopWatch.ElapsedTicks / 10000f}");

            var boxed = (object)tstClass;

            stopWatch.Restart();
            for (int i = 0; i < count; i++)
            {
                var t = (TSTClass)boxed;
            }
            stopWatch.Stop();
            UnityEngine.Debug.Log($"UnBoxing speed: {stopWatch.ElapsedTicks / 10000f}");

            stopWatch.Restart();
            for (int i = 0; i < count; i++)
            {
                var t = tstClass;
            }
            stopWatch.Stop();
            UnityEngine.Debug.Log($"Simple writing speed: {stopWatch.ElapsedTicks / 10000f}");
        }
    }
}


public class TSTClass : TSTInterface
{
    public string penis = "penispenispenis penis";
    public string hui = "huihuihui hui";
    public float fgfgfg = 5400f;
    public float gfgfgfg = 0.54f;

    public int count => 54;
}

public interface TSTInterface
{
    public int count { get; }
}
