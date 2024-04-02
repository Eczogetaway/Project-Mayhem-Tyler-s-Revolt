using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public float loadingTime = 5f; // Время в секундах для экрана загрузки
    public string sceneToLoad = "1 Level"; // Имя сцены, которая будет загружена после экрана загрузки

    private float timeElapsed;

    // Start вызывается перед первым обновлением кадра
    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(loadingTime);
        SceneManager.LoadScene(sceneToLoad);
    }

    // Update вызывается один раз за кадр
    void Update()
    {
        // Здесь можно обновить UI экрана загрузки, если необходимо
        timeElapsed += Time.deltaTime;
    }
}
