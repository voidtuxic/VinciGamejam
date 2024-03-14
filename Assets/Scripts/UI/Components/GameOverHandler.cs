using Void.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Void.UI.Components
{
    public class GameOverHandler : UIEventComponent
    {
        [SerializeField] private TextMeshProUGUI label;

        protected override void Bind()
        {
            EventBus.SubscribeEvent<GameEvent.GameOver>(OnGameOverEvent);
        }

        private void OnGameOverEvent(GameEvent.GameOver evt)
        {
            label.text = evt.Message;
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            EventBus.UnsubscribeEvent<GameEvent.GameOver>(OnGameOverEvent);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(0);
        }
    }
}
