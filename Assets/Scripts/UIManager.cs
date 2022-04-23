using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject[] ShopItems;
    public GameObject[] Characters;
    public GameObject defaultCharacter;
    public GameObject PauseMenuUI;
    public GameObject GameOverMenuUI;

    public Slider LifeBar;
    public Slider FuelBar;

    public Text CoinsText;
    public Text ScoreText;
    public Text FinalCoinsText;
    public Text FinalScoreText;
    public Text HighScoreText;
    public Text TotalCoinsText;

    public float timeToScore;

    public bool GameIsPaused;

    public static UIManager instance;

    private int Score = 0;
    private int Coins = 0;

    private string[] CharactersPurchased = new string[6];

    private bool CanUpdateScore = true;

    private void Awake()
    {
        instance = this;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (!PlayerPrefs.HasKey("2"))
            {
                PlayerPrefs.SetString("2", null);
                PlayerPrefs.SetString("3", null);
                PlayerPrefs.SetString("4", null);
                PlayerPrefs.SetString("5", null);
                PlayerPrefs.SetString("6", null);
                PlayerPrefs.SetString("7", null);
            }

            if (!PlayerPrefs.HasKey("PlayerCoins"))
            {
                PlayerPrefs.SetInt("PlayerCoins", 0);
            }

            if (!PlayerPrefs.HasKey("CharSelected"))
            {
                PlayerPrefs.SetInt("CharSelected", 6);
            }

            CharactersPurchased[0] = PlayerPrefs.GetString("2");
            CharactersPurchased[1] = PlayerPrefs.GetString("3");
            CharactersPurchased[2] = PlayerPrefs.GetString("4");
            CharactersPurchased[3] = PlayerPrefs.GetString("5");
            CharactersPurchased[4] = PlayerPrefs.GetString("6");
            CharactersPurchased[5] = PlayerPrefs.GetString("7");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (CanUpdateScore && !GameIsPaused)
            {
                CanUpdateScore = false;
                StartCoroutine("UpdateScore");
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !GameOverMenuUI.activeSelf)
            {
                GameIsPaused = !GameIsPaused;
                PauseMenuUI.SetActive(GameIsPaused);

                if (GameIsPaused)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }

    public void UpdateCoins()
    {
        Coins++;
        CoinsText.text = Coins.ToString();
    }

    public void SetMaxFuel(int amount)
    {
        FuelBar.maxValue = amount;
        FuelBar.value = amount;
    }

    public void SetMaxLife(int amount)
    {
        LifeBar.maxValue = amount;
        LifeBar.value = amount;
    }

    public void SetFuel(int amount)
    {
        FuelBar.value = amount;
    }

    public void SetLife(int amount)
    {
        LifeBar.value = amount;
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void ContinueGame(GameObject ui)
    {
        ui.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void LoadMenu(GameObject ui)
    {
        ui.SetActive(true);
    }

    public void UnLoadMenu(GameObject ui)
    {
        ui.SetActive(false);
    }

    public void LoadShop(GameObject Shop)
    {
        for (int i = 0; i < ShopItems.Length; i++)
        {
            if (CharactersPurchased[i] != "")
            {
                ShopItems[i].SetActive(false);
            }
        }


        TotalCoinsText.text = PlayerPrefs.GetInt("PlayerCoins").ToString();
        Shop.SetActive(true);
    }

    public void LoadCharacters(GameObject ui)
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            if (CharactersPurchased[i] == "")
            {
                Characters[i].SetActive(false);
            }
            
            if (i == PlayerPrefs.GetInt("CharSelected"))
            {
                Characters[i].GetComponent<Button>().interactable = false;
            }
        }

        if (PlayerPrefs.GetInt("CharSelected") == 6)
        {
            defaultCharacter.GetComponent<Button>().interactable = false;
        }

        ui.SetActive(true);
    }

    public void SelectCharacter(int index)
    {
        if (index != 6)
        {
            if (PlayerPrefs.GetInt("CharSelected") == 6)
            {
                defaultCharacter.GetComponent<Button>().interactable = true;
            }
            else
            {
                Characters[PlayerPrefs.GetInt("CharSelected")].GetComponent<Button>().interactable = true;
            }
            
            if (CharactersPurchased[index] != "")
            {
                PlayerPrefs.SetInt("CharSelected", index);
                Characters[index].GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            defaultCharacter.GetComponent<Button>().interactable = false;

            Characters[PlayerPrefs.GetInt("CharSelected")].GetComponent<Button>().interactable = true;
            PlayerPrefs.SetInt("CharSelected", index);
        }
    }

    public void PurchaseCharacter(int character)
    {
        if (PlayerPrefs.GetInt("PlayerCoins") >= 100)
        {
            PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("PlayerCoins") - 100);

            TotalCoinsText.text = PlayerPrefs.GetInt("PlayerCoins").ToString();

            PlayerPrefs.SetString((character + 2).ToString(), "purchased");

            CharactersPurchased[character] = "purchased";

            ShopItems[character].SetActive(false);

        }
    }

    public void GameOver()
    {
        PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("PlayerCoins") + Coins);

        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", Score);
        }
        else if (PlayerPrefs.GetInt("HighScore") < Score)
        {
            PlayerPrefs.SetInt("HighScore", Score);
        }

        Time.timeScale = 0f;
        GameIsPaused = true;

        FinalScoreText.text = Score.ToString();
        HighScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        FinalCoinsText.text = Coins.ToString();
        TotalCoinsText.text = PlayerPrefs.GetInt("PlayerCoins").ToString();

        GameOverMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void IncrementScore(int amount)
    {
        Score += amount;
        ScoreText.text = Score.ToString();
    }

    private IEnumerator UpdateScore()
    {
        IncrementScore(1);       

        yield return new WaitForSeconds(timeToScore);

        CanUpdateScore = true;
    }
}
