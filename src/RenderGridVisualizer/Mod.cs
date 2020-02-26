using ICities;
using UnityEngine;

namespace RenderGridVisualizer
{
    public class Mod : LoadingExtensionBase, IUserMod
    {
        private GameObject _gameObject;

        public string Name => "Render Grid Visualizer";
        public string Description => "Visualizer for the draw render grid.";

        public void OnEnabled()
        {
            if (LoadingManager.exists && LoadingManager.instance.m_loadingComplete)
            {
                InstallMod();
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (!(mode == LoadMode.LoadGame || mode == LoadMode.NewGame || mode == LoadMode.NewGameFromScenario))
            {
                return;
            }

            InstallMod();
        }

        public void OnDisabled()
        {
            if (LoadingManager.exists && LoadingManager.instance.m_loadingComplete)
            {
                UninstallMod();
            }
        }

        public override void OnLevelUnloading()
        {
            UninstallMod();
        }

        private void InstallMod()
        {
            _gameObject = new GameObject("RenderGridVisualizer");
            _gameObject.AddComponent<RenderGridVisualizer>();
        }

        private void UninstallMod()
        {
            Object.Destroy(_gameObject);
        }
    }
}
