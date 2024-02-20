using Assets.Scripts.Other;
using Playmode.PlayData;
using Playmode.View;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Zenject;

namespace Playmode.Installers
{
    [CreateAssetMenu(fileName = "PlaymodeSettings", menuName = "Config/Game settings")]
    public class PlaymodeSettings : ScriptableObjectInstaller<PlaymodeSettings>
    {
        public UIFactory.Settings UIFactory;
        public GameMapConfig gameMapConfig;
        public SpriteAtlas _cellAtlas;

        public override void InstallBindings()
        {
            Container.BindInstances(UIFactory);
            Container.Bind<Converter>().AsSingle();
            gameMapConfig.Init();
            Container.Bind<GameMapConfig>().FromInstance(gameMapConfig).AsSingle();

            Container.Bind<IconProvaider>().AsSingle().WithArguments(_cellAtlas);
        }
    }

}
