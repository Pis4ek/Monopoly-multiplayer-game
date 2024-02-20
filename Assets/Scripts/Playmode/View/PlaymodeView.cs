using Playmode.NetCommunication;
using Playmode.PlayData;
using System;
using UnityEngine;

namespace Playmode.View
{
    public class PlaymodeView
    {
        private PlayersWindow _playersWindow;
        private GameMapWindow _gameMapWindow;
        private InputHandler _inputHandler;
        private CubeResultShower _cubesResultShower;
        private AnimationQueue _animationQueue;

        public PlaymodeView(PlayersWindow playersWindow, GameMapWindow gameMapWindow,
            InputHandler inputHandler, CubeResultShower cubesResultShower, AnimationQueue animationQueue)
        {
            _playersWindow = playersWindow;
            _gameMapWindow = gameMapWindow;
            _inputHandler = inputHandler;
            _cubesResultShower = cubesResultShower;
            _animationQueue = animationQueue;
        }

        public void ShowInput(IInputRequireNetMessage inputMessage) 
        {
            _inputHandler.ShowInput(inputMessage);
        }

        public void ShowThrowCubesResult(ThrowCubesResult result)
        {
            _cubesResultShower.ShowResult(result);
        }

        public void SetWaitedPlayer(PlayerID player, DateTime waitedDate)
        {
            _playersWindow.SetWaitedPlayer(player, waitedDate);
        }
    }
}
