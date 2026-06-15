using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private RectTransform _transform;

    private void Update()
    {
        _transform.Translate(Vector2.up * _speed * Time.deltaTime);

        if (_transform.anchoredPosition.y >= 6443f)
        {

            SceneManager.LoadScene("Start Scene");
        }
    }
}
