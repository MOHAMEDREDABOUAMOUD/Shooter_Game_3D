using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI toLevelUpText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private UIHealthBar levelBar;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject gameOverMenu;
    

    private PlayerHealth playerHealth;
    

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        hud.SetActive(true);
        gameOverMenu.SetActive(false);
    }

    private void Update()
    {
        healthText.text = playerHealth.currentHealth + "/" + playerHealth.maxHealth;
        scoreText.text = "Score : " + Game.Instance.player.score;
        finalScoreText.text = "Score : " + Game.Instance.player.score;
        levelText.text = "Level : " + Game.Instance.player.level;
        toLevelUpText.text = "To Level up : " + Game.Instance.player.toLevelUp;
        levelBar.SetHealthBarPercentage(((float) Game.Instance.player.xpGoal - Game.Instance.player.toLevelUp) / Game.Instance.player.xpGoal);
        ammoText.text = Game.Instance.player.rifleAmmo + "";
        
        if(playerHealth.currentHealth <= 0)
            GameOverMenu();
    }

    private void GameOverMenu()
    {
        hud.SetActive(false);
        gameOverMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Debug.Log("herllo there");
        Application.Quit();
    }
}
