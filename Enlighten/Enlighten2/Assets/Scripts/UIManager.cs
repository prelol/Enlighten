using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent onSelect = new UnityEvent();

    ButtonAnimHandler btnHandler;

    #region References
    [SerializeField]
    private GameObject menuCanvas, levelCanvas;
    [SerializeField]
    private Button playButton, playLevelButton, exitButton;
    [SerializeField]
    private int sceneCount;
    #endregion

    #region Singleton

    public static UIManager instance = null;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    #endregion

    private void Start()
    {
        if(PlayerPrefs.HasKey("UnlockedLevel"))
            SetupCanvases();

        //playButton.onClick.AddListener(Play);
        //exitButton.onClick.AddListener(Application.Quit);
        onSelect.AddListener(Play); //for the image play button
    }

    private void SetupCanvases()
    {
        SetupLevelButtons();
        levelCanvas.SetActive(false);
    }

    private void SetupLevelButtons()
    {
        int startingX = -500;
        int startingY = 400;

        int x = 0;
        int y = 0;
        for (int i = 0; i < sceneCount; i++)
        {
            int a = i; //to store level index helper
            if (i % 5 == 0)
            {
                y++;
                x = 0;
            }

            else
                x++;

            Button current = Instantiate(playLevelButton.gameObject, new Vector3(startingX + (x * 250), startingY + (y * -200), 0), Quaternion.identity).GetComponent<Button>();
            current.transform.SetParent(levelCanvas.transform, false);
            current.GetComponentInChildren<Text>().text = (i + 1).ToString();
            current.onClick.AddListener(delegate { PlayLevel(a + 1); });

            if (PlayerPrefs.GetInt("UnlockedLevel") < (a + 1))
                current.interactable = false;
        }
    }

    #region UI Functions
    private void Play()
    {
        if (!PlayerPrefs.HasKey("UnlockedLevel"))
            SceneManager.LoadScene("Level01");

        else
        {
            menuCanvas.SetActive(false);
            levelCanvas.SetActive(true);
        }

        PlayerPrefs.SetInt("UnlockedLevel", 1);
    }

    private void PlayLevel(int index) => SceneManager.LoadScene("Level" + index);
    #endregion
}
