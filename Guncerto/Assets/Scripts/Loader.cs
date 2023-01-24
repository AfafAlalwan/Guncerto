using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class Loader : MonoBehaviour
{
    public static Loader Instance;

    [SerializeField] GameObject loadingPanel;
    [SerializeField] float minLoadTime;

    [SerializeField] GameObject loadingIcon;
    [SerializeField] float rotatingSpeed;

    [SerializeField] Image fadeImage;
    [SerializeField] float fadeTime;

    private string targetScene;
    private bool isLoading;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        loadingPanel.SetActive(false);
        fadeImage.gameObject.SetActive(false);
    }
   
    public async void LoadScene(string sceneName)
    {
        targetScene = sceneName;
        isLoading = true;
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1f, 0.5f).OnComplete(() => 
        {
            loadingPanel.SetActive(true);
            fadeImage.DOFade(0f, 0.1f);

        });

        await Task.Delay(500);
        StartCoroutine(LoadSceneRoutine());


    }

    IEnumerator LoadSceneRoutine()
    {
        StartCoroutine(SpinIconRoutine());

        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
        float elapsedLoadTime = 0f;

        while (!op.isDone)
        {
            elapsedLoadTime += Time.deltaTime;
            yield return null;

        }

        while (elapsedLoadTime < minLoadTime)
        {
            elapsedLoadTime += Time.deltaTime;
            yield return null;
        }


        fadeImage.DOFade(1f, 0.1f);
        loadingPanel.SetActive(false);
        fadeImage.DOFade(0f, 0.5f);

        isLoading = false;

    }
    IEnumerator SpinIconRoutine()
    {
        while (isLoading)
        {
            loadingIcon.transform.Rotate(0, 0, -rotatingSpeed);
            yield return null;
        }
    }
}
