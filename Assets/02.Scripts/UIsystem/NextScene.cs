using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] private string _scene;
    [SerializeField] private DialogueSystem _ds;

    private bool _playerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (_playerEntered) return; // 이미 실행됐으면 무시

        _playerEntered = true;

        if (_ds == null) return;
        _ds.OnDialogueEnd += HandleDialogueEnd;
        _ds.StartDialogue();
    }

    private void HandleDialogueEnd()
    {
        _ds.OnDialogueEnd -= HandleDialogueEnd;

        // 씬 이름이 비어있으면 그냥 끝
        if (string.IsNullOrEmpty(_scene))
            return;

        FadeInOutSystem.Instance.FadeIn(() =>
        {
            SceneManager.LoadScene(_scene);
        });
    }
}
