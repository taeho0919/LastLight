using System;
using TMPro;
using System.Collections;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string[] dialogues;

    [SerializeField] private GameObject penal;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float autoNextDelay = 3f; // ✅ 자동 넘김 대기 시간

    private bool _hasPlayed = false;
    private bool _isActive = false;
    public Action OnDialogueEnd;
    private int currentIndex = 0;
    private bool isTyping = false;
    public PlayerMovement _pm;


    private void Start()
    {
        _pm = FindAnyObjectByType<PlayerMovement>();
        penal.SetActive(false); // 처음엔 숨김
    }

    private void Update()
    {
        if (!_isActive) return; // 활성화된 것만 입력 받음

        if (Input.GetKeyDown(KeyCode.Return))
            NextDialogue();
    }
    public void StartDialogue()
    {
        if (_hasPlayed) return; // 이미 실행됐으면 무시
        _hasPlayed = true;

        _isActive = true;
        currentIndex = 0;
        if (dialogues.Length > 0)
        {
            _pm._isTimeLine = true;
            ShowDialogue();
        }
    }


    public void NextDialogue()
    {
        if (isTyping)
        {
            // 타이핑 중 엔터 → 즉시 전체 출력 + 자동 넘김 코루틴 시작
            StopAllCoroutines();
            dialogueText.text = dialogues[currentIndex];
            isTyping = false;
            StartCoroutine(AutoNext()); // ✅ 즉시 출력 후에도 자동 넘김 대기
            return;
        }

        currentIndex++;

        if (currentIndex >= dialogues.Length)
        {
            EndDialogue();
            return;
        }

        ShowDialogue();
    }

    private void ShowDialogue()
    {
        penal.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypeText(dialogues[currentIndex]));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        StartCoroutine(AutoNext()); // ✅ 타이핑 끝나면 자동 넘김 대기 시작
    }

    // ✅ 일정 시간 후 자동으로 NextDialogue 호출
    private IEnumerator AutoNext()
    {
        yield return new WaitForSeconds(autoNextDelay);
        NextDialogue();
    }

    private void EndDialogue()
    {
        _isActive = false;
        _pm._isTimeLine = false;
        penal.SetActive(false);
        dialogueText.text = "";
        OnDialogueEnd?.Invoke();
    }
}
