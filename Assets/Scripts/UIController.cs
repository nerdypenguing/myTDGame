using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private TowerData[] towers;
    [SerializeField] private GameObject gameOverPanel;
    private static int highScore;
    private Platform currentPlatform;
    private List<GameObject> activeCards = new List<GameObject>();
    [SerializeField] private List<GameObject> gameOverInactives = new List<GameObject>();
    private void OnEnable()
    {
        Spawner.onWaveChange += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLivesText;
        GameManager.OnRecourceChange += UpdateCurrencyText;
        GameManager.OnScoreChange += UpdateScoreText;
        Platform.OnPlatformClicked += HandlePlatformClicked;
        TowerCard.OnTowerSelected += HandleTowerSelected;
    }
    private void OnDisable()
    {
        Spawner.onWaveChange -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
        GameManager.OnRecourceChange -= UpdateCurrencyText;
        GameManager.OnScoreChange -= UpdateScoreText;
        Platform.OnPlatformClicked -= HandlePlatformClicked;
        TowerCard.OnTowerSelected -= HandleTowerSelected;
    }

    private void UpdateWaveText(int currentWave)
    {
        waveText.text = $"Wave:  {currentWave + 1}";

    }
    private void UpdateLivesText(int currentLives)
    {
        livesText.text = $"{currentLives}";

        if (currentLives <= 0)
        {
           ShowGameOver();
        }
    }
    private void UpdateCurrencyText(int currentCurrency)
    {
        currencyText.text = $"{currentCurrency}";
    }
    private void UpdateScoreText(int currentScore)
    {
        scoreText.text = $"Score:  {currentScore}";
        highScoreText.text = $"High Score:  {highScore}";
        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreText.text = $"High Score:  {highScore}";
        }
    }
    private void HandlePlatformClicked(Platform platform)
    {
        currentPlatform = platform;
        showPanel();
    }
    private void showPanel()
    {
        towerPanel.SetActive(true);
        GameManager.instance.SetTimeScale(0f);
        PopulateTowerCard();
    }
    public void hidePanel()
    {
        towerPanel.SetActive(false);
        GameManager.instance.SetTimeScale(1f);
    }
    private void PopulateTowerCard()
    {
        foreach (var card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();
        foreach (var data in towers)
        {
            GameObject cardGameObject = Instantiate(towerCardPrefab, cardsContainer);
            TowerCard card = cardGameObject.GetComponent<TowerCard>();
            card.Initialize(data);
            activeCards.Add(cardGameObject);
        }
    }
    private void HandleTowerSelected(TowerData towerData)
    {
        if (GameManager.instance.Recources >= towerData.cost)
        {
        GameManager.instance.SpendRecources(towerData.cost);
        currentPlatform.PlaceTower(towerData);
        }
        hidePanel();
    }
    public void RestartGame()
    {
        GameManager.instance.SetTimeScale(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
        
    }
    public void ReturnToMainMenu()
    {
        GameManager.instance.SetTimeScale(1f);
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
    private void ShowGameOver()
    {
        GameManager.instance.SetTimeScale(0f);
        gameOverPanel.SetActive(true);
        gameOverInactives.ForEach(obj => obj.SetActive(false));
    }
}
