using Zenject;

namespace Other
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LastGameClientsSession>().AsSingle().NonLazy();
        }
    }
}