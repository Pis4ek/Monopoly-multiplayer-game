using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using MainMenu.StateMachine;

namespace MainMenu.Installers
{
    public class StateMachineInstaller : MonoInstaller
    {
        [SerializeField] UI_HostConfigMenu _HostConfigMenu;
        [SerializeField] UI_LobbyMenu _LobbyMenu;
        [SerializeField] UI_LocalServerList _LocalServerList;
        [SerializeField] UI_NetworkModes _NetworkModes;
        [SerializeField] UI_EnterPassword _EnterPassword;

        public override void InstallBindings()
        {
            Container.Bind<MainMenuStateMachine>().FromMethod(InitializeMainMenuStateMachine).AsSingle();
        }

        private MainMenuStateMachine InitializeMainMenuStateMachine() 
        {
            MainMenuStateMachine stateMachine = new();

            stateMachine.RegisterState(_NetworkModes, true);
            stateMachine.RegisterState(_LobbyMenu);
            stateMachine.RegisterState(_HostConfigMenu);
            stateMachine.RegisterState(_LocalServerList);
            stateMachine.RegisterState(_EnterPassword);

            stateMachine.Start();

            return stateMachine;
        }
    }
}
