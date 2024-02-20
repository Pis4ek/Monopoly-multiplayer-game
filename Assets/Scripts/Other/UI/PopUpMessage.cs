using TMPro;

namespace UnityEngine.UI
{
    public class PopUpMessage : MonoBehaviour
    {
        [SerializeField] Button _ok;
        [SerializeField] TMP_Text _title;
        [SerializeField] TMP_Text _message;

        private void Start()
        {
            _ok.onClick.AddListener(OnOkClicked);
        }

        public void Initionalize(string title, string message) 
        {
            _title.text = title;
            _message.text = message;
        }

        private void OnOkClicked()
        {
            Destroy(gameObject);
        }
    }
}
