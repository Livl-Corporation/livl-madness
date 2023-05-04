using Network;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerFrame : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        PlayerNetwork player;

        // TODO : lors de l'implémentation du système de username, penser a modif ici
        public void SetPlayer (PlayerNetwork player)
        {
            this.player = player;
            text.text = "Player " + this.player.playerIndex;
        }
    }
}