using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGame : MonoBehaviour
{
    public static ManagerGame Instance;
    public GameObject maskPrefab;
    private void Awake()
    {
        Instance = this;
    }
    public void InstantiateMask(Piece piece)
    {
        foreach (Sprite sprite in piece.GetData().layers_sprite)
        {
            GameObject mask = Instantiate(maskPrefab, piece.transform);
            Image image = mask.GetComponent<Image>();
            image.sprite = sprite;
            image.preserveAspect = true;
        }
    }
}
