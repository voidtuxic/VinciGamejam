using Void.Player;
using UnityEngine;

namespace Void.UI.Components
{
    public class StartGameHandler : UIEventComponent
    {
        public void StartGame()
        {
            gameObject.SetActive(false);
            EventBus.PublishEvent(new GameEvent.StartGame());
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
