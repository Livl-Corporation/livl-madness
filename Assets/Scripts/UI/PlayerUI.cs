using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private Player player;

    private PlayerController controller;

    [SerializeField]
    private GameObject pauseMenu;

    // Start is called before the first frame update
    private void Start()
    {
        SetPauseMenuVisibility(false);
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        SetPauseMenuVisibility(!PauseMenu.isOn);
    }

    public void SetPauseMenuVisibility(bool visible)
    {
        pauseMenu.SetActive(visible);
        PauseMenu.isOn = visible;
    }

}
