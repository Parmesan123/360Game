using UnityEngine;
using Zenject;

namespace Installers
{
	public class PlayerInstaller : MonoInstaller
	{
		[SerializeField] private Player _player;
		[SerializeField] private Transform _spawnPoint;

		public override void InstallBindings()
		{
			BindPlayer();
		}

		private void BindPlayer()
		{
			Player player = Container.InstantiatePrefabForComponent<Player>(_player, _spawnPoint.position, Quaternion.identity, null);

			Container
				.BindInterfacesAndSelfTo<Player>()
				.FromInstance(player)
				.AsSingle()
				.NonLazy();
		}
	}
}