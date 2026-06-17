using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCubeManager : MonoBehaviour
{
    [SerializeField] private GameObject _dieCube;

    public Stack<GameObject> dieCubePool = new Stack<GameObject>();
    public static DieCubeManager instance;

    private void Start()
    {
        instance = this;
    }

    public void SpawnDieCube(Vector2 position, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject cube;

            if (dieCubePool.Count > 0)
            {
                cube = dieCubePool.Pop();
                cube.SetActive(true);
            }
            else
            {
                cube = Instantiate(_dieCube, transform);
            }

            cube.transform.position = position;

            Rigidbody2D rb = cube.GetComponent<Rigidbody2D>();

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            float angle = Random.Range(0f, 360f);

            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            rb.AddForce(dir * Random.Range(8f, 15f),
                ForceMode2D.Impulse);

            rb.AddTorque(Random.Range(-800f, 800f));

            StartCoroutine(ReturnCube(cube));
        }
    }

    private IEnumerator ReturnCube(GameObject cube)
    {
        yield return new WaitForSeconds(0.6f);

        cube.SetActive(false);

        dieCubePool.Push(cube);
    }
}
