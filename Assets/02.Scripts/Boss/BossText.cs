using TMPro;
using System.Collections;
using UnityEngine;

public class BossText : MonoBehaviour
{

    [SerializeField] private TextMeshPro textMesh;
    [TextArea]
    [SerializeField] private string[] texts;
    [SerializeField] private float interval = 1f;
    [SerializeField] private float typeSpeed = 0.05f;

    private Coroutine routine;


    public void StartText()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(PlayText());
    }

    public void StopText()
    {
        if (routine != null) StopCoroutine(routine);
        textMesh.text = "";
    }

    private IEnumerator PlayText()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            yield return StartCoroutine(TypeText(texts[i]));
            yield return new WaitForSeconds(interval);
            textMesh.text = "";
        }
    }

    private IEnumerator TypeText(string text)
    {
        textMesh.text = "";
        foreach (char c in text)
        {
            textMesh.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
