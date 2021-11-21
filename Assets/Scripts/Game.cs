using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game game;

    public List<GameObject> referenceUnits;
    [SerializeField] private List<Tile> tiles;
    [SerializeField] private List<State> states;
    [SerializeField] private InputField orderInputField;
    [SerializeField] private ConsoleUI console;

    public static List<string> provinceCodes = new List<string>();

    public List<Tile> markedTiles;
    public bool isWinter;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.C))
        {
            return;
        }
        console.Toggle();
    }

    private void Awake()
    {
        game = this;

        foreach (var tile in tiles)
            provinceCodes.Add(tile.provinceCode);
    }

    //Når spiller klikker "enter" etter å ha skrevet en ordre
    public void OnOrderInput()
    {
        return;
    }

    //Når spiller endrer på ordren skrevet i vinduet
    public void OnInputChange()
    {
        orderInputField.image.color = Order.IsValidOrderString(orderInputField.text, out _) ? 
            Color.white : Color.red;
    }

    public Tile GetTileFromCode(string code)
    {
        foreach (var tile in tiles)
        {
            if (tile.provinceCode == code)
            {
                return tile;
            }
        }
        Debug.Log("Found no tile with code: " + code);

        return null;
    }
}

public enum Nation
{
    Local,
    Black,
    White,
    Yellow,
    Red,
    Green
}

public static class Extensions
{
    public static Color GetNationColor(this Nation nation)
    {
        return nation switch
        {
            Nation.Local => Color.clear,
            Nation.Black => new Color(0.2f, 0.2f, 0.2f, 1f),
            Nation.White => Color.white,
            Nation.Yellow => Color.yellow,
            Nation.Red => Color.red,
            Nation.Green => Color.green,
            _ => Color.clear,
        };
    }
}
