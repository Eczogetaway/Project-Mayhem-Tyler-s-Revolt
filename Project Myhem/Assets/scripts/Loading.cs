using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public float loadingTime = 5f; // ����� � �������� ��� ������ ��������
    public string sceneToLoad = "1 Level"; // ��� �����, ������� ����� ��������� ����� ������ ��������

    private float timeElapsed;

    // Start ���������� ����� ������ ����������� �����
    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(loadingTime);
        SceneManager.LoadScene(sceneToLoad);
    }

    // Update ���������� ���� ��� �� ����
    void Update()
    {
        // ����� ����� �������� UI ������ ��������, ���� ����������
        timeElapsed += Time.deltaTime;
    }
}
