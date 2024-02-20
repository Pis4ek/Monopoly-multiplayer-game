using Mirror;
using Playmode.NetCommunication;
using System;

namespace Playmode.View
{
    public interface IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        public void SetPermission(IInputRequireNetMessage inputMessage);
        public void HideInput();
    }
}
