using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ManagerGame : MonoBehaviour
{
    public static ManagerGame Instance;
    public GameObject maskPrefab;
    private bool options;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            options = !options;
            if (options)
            {
                SceneManager.LoadSceneAsync("Options", LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.UnloadSceneAsync("Options");
            }
        }
    }
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

    public Sprite LoadSpriteFromSpritesheet(string resourcePath, string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>(resourcePath);
        foreach (var s in all)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }
        return null;
    }
}
