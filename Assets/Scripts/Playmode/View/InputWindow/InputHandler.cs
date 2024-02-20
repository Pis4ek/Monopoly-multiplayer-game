using Mirror;
using Playmode.NetCommunication;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Playmode.View
{
    public class InputHandler
    {
        private NetMessageSender _messageSender;
        private List<IInputUIElement> _inputElements;
        private ViewInputStateMachine _viewInputSM;
        private IInputRequireNetMessage _input;

        [Inject]
        public void Init(List<IInputUIElement> inputElements, NetMessageSender messageSender,
            ViewInputStateMachine viewInputSM)
        {
            _inputElements = inputElements;
            _messageSender = messageSender;
            _viewInputSM = viewInputSM;

            foreach (var element in _inputElements)
            {
                element.OnInputEntered += SendInputResult;
            }

            //_viewInputSM.OnStateChanged += () => { if(_viewInputSM.State = ViewInputState.Default)};
        }

        public void ShowInput(IInputRequireNetMessage inputMessage)
        {
            _input = inputMessage;
            foreach (var element in _inputElements)
            {
                element.SetPermission(inputMessage);
            }
        }

        public void HideInput()
        {
            foreach (var element in _inputElements)
            {
                element.HideInput();
            }
            _viewInputSM.State = ViewInputState.Default;
        }

        private void SendInputResult(NetworkMessage message)
        {
            HideInput();
            _messageSender.SendMessage(message);
        }
    }
}