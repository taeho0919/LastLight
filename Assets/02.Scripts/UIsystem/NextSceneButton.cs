using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
    [SerializeField]private string _sceneName;
    [SerializeField] private GameObject image;
    public void OnClick()
    {
        image.SetActive(true);
        FadeInOutSystem.Instance.FadeIn(() => SceneManager.LoadScene(_sceneName));
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
