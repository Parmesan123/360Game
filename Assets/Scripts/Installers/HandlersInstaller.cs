using Zenject;

namespace Installers
{
	public class HandlersInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindPauseHandler();
			BindRestartHandler();
			BindGameOverHandler();
		}

		private void BindRestartHandler()
		{
			Container
				.BindInterfacesAndSelfTo<RestartHandler>()
				.FromNew()
				.AsSingle();
		}

		private void BindPauseHandler()
		{
			Container
				.BindInterfacesAndSelfTo<PauseHandler>()
				.FromNew()
				.AsSingle();
		}

		private void BindGameOverHandler()
		{
			Container
				.BindInterfacesAndSelfTo<GameOverHandler>()
				.FromNew()
				.AsSingle();
		}
	}
}