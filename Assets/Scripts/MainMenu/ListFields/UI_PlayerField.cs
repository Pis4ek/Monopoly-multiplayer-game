using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace MainMenu
{
    class UI_PlayerField : MonoBehaviour
    {
        [SerializeField] Sprite _readyImage;
        [SerializeField] Sprite _notReadyImage;

        [Header("Elements")]
        [SerializeField] TMP_Text _nickname;
        [SerializeField] Image _image;
        [SerializeField] Image _setReadyImage;

        public string Nickname { get; private set; }

        public void UpdateData(string nickname, Texture2D image, bool ready = false) 
        {
            _nickname.text = nickname;
            Nickname = nickname;
            _image.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));

            if (ready)
                _setReadyImage.sprite = _readyImage;
            else
                _setReadyImage.sprite = _notReadyImage;
        }

        public void UpdateReady(bool ready) 
        {
            if (ready)
                _setReadyImage.sprite = _readyImage;
            else
                _setReadyImage.sprite = _notReadyImage;
        }
    }
}
