using Services;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Services.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour
    {
        public event Action OnValueCahngedEvent;

        [SerializeField] Image _backGround;
        [SerializeField] Text _loadingText;

        public bool IsScreenActive { get; private set; } = true;

        public async Task ActivateScreen()
        {
            if(IsScreenActive == false)
            {
                float alpha = 0f;
                for (int i = 0; i < 10; i++)
                {
                    alpha += 0.1f;
                    _backGround.color = new Color(_backGround.color.r, _backGround.color.b, _backGround.color.g, alpha);
                    await Task.Delay(25);
                }

                IsScreenActive = true;
                OnValueCahngedEvent?.Invoke();

                await Task.Delay(150);

                ComponentExtention.Activate(_loadingText);
            }
        }

        public async Task DisactivateScreen()
        {
            if (IsScreenActive)
            {
                ComponentExtention.Disactivate(_loadingText);

                await Task.Delay(150);

                float alpha = 1f;
                for (int i = 0; i < 10; i++)
                {
                    alpha -= 0.1f;
                    _backGround.color = new Color(_backGround.color.r, _backGround.color.b, _backGround.color.g, alpha);
                    await Task.Delay(25);
                }

                IsScreenActive = false;
                OnValueCahngedEvent?.Invoke();
            }
        }
    }
}