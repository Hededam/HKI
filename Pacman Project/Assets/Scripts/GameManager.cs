using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;

    public GameOverScreen gameOverScreen;

    public Ghost[] ghosts;

    public Pacman pacman;

    public Transform pellets;

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }

    public bool IsAlive;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }
    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
        IsAlive = true;
        if (this.lives <= 0 && Input.anyKeyDown);
    }
    private void NewRound()
    {
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }
    private void ResetState()
    {
        ResetGhostMultiplier();
        for (int i = 0; i < ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }
        this.pacman.ResetState();

        IsAlive = true;
    }
    private void GameOver()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }
        this.pacman.gameObject.SetActive(false);

        gameOverScreen.Setup(score);

        NewGame();
    }
    private void SetScore(int score)
    {
        this.score = score;

        uiManager.UpdateScoreUI(score);
    }
    private void SetLives(int lives)
    {
        this.lives = lives;
    }
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;

    }
    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        uiManager.UpdateLivesUI(lives);

        IsAlive = false;

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 1.5f);
        }
        else
        {
            GameOver();
        }
    }
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(this.score + pellet.points);

        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 1.5f);
        }
    }
    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}