using System;
using UnityEngine;
using UnityEngine.UI;

namespace Playmode.View
{
    public class PlayerInfoTimer : MonoBehaviour
    {
        [SerializeField] Text _counter;
        private bool _isCounting = false;
        private DateTime _time;

        private void FixedUpdate()
        {
            if (_isCounting)
            {
                var text = $"{(_time - DateTime.Now).Seconds}";
                _counter.text = text;
                if (_time < DateTime.Now)
                {
                    _isCounting = false;
                }
            }
        }

        public void SetTimerData(DateTime time)
        {
            _time = time;
            if(time > DateTime.Now)
            {
                _isCounting = true;
            }
        }


    }
}
