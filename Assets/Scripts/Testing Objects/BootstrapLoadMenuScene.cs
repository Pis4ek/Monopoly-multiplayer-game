using UnityEngine;
using UnityEngine.SceneManagement;

namespace Testing_Objects
{
    public class BootstrapLoadMenuScene : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
