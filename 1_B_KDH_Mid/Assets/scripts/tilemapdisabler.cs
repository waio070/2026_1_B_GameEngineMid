using UnityEngine;
using UnityEngine.Tilemaps;

public class tilemapdisabler : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TilemapRenderer>().enabled = false;
    }
}