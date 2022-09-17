using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<Manager>();
            }

            return instance;
        }
    }

    const int spinPrice = 199;

    const int minPrizeValue = 9;
    const int maxPrizeValue = 299;

    const string saveKey = "coins";
    const int initCoinsCount = 1000;
    int coinsCount;

    [Space(10)]
    [SerializeField] GameObject menu;
    [SerializeField] GameObject game;

    [Space(10)]
    [SerializeField] Text coinsCountText;

    private void Start()
    {
        coinsCount = Load();
        UpdateCoinsCount();
    }

    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void Save()
    {
        PlayerPrefs.SetInt(saveKey, coinsCount);
        PlayerPrefs.Save();
    }

    int Load()
    {
        int _coins = PlayerPrefs.HasKey(saveKey) ? PlayerPrefs.GetInt(saveKey) : initCoinsCount;
        if(_coins <= 0)
        {
            _coins = minPrizeValue;
        }

        return _coins;
    }

    bool CanSpin()
    {
        return coinsCount >= spinPrice;
    }

    public void TrySpin()
    {
        if(!CanSpin())
        {
            return;
        }

        coinsCount -= spinPrice;
        if(coinsCount < 0)
        {
            coinsCount = 0;
        }

        UpdateCoinsCount();
        Save();
    }

    public void CalculatePrize()
    {
        int prize = Random.Range(minPrizeValue, maxPrizeValue);
        UpdateCoinsCount(prize);
        Save();
    }

    public void UpdateCoinsCount(int amount = 0)
    {
        coinsCount += amount;
        coinsCountText.text = string.Format("{0:0000}", coinsCount);
        Save();
    }

    public void StartGame(GameObject openGameGO)
    {
        menu.SetActive(false);
        openGameGO.SetActive(true);

        UpdateCoinsCount();
    }
}
