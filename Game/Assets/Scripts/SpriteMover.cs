using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SpriteMover : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private string spriteTag;
    [SerializeField] private float gridCellHeight;
    [SerializeField] private float gridCellWidth;

    private Vector2Int blankPosition = new Vector2Int(2, 2); // Initial position of the blank space
    private bool isMoving = false; // Flag to track if a sprite is currently being moved


    

    //private bool IsAdjacent(Vector2Int position1, Vector2Int position2)
    //{
    //    int distance = Mathf.Abs(position1.x - position2.x) + Mathf.Abs(position1.y - position2.y);
    //    return distance == 1;
    //}

    //private void MoveSpriteToBlankPosition(int row, int col)
    //{
    //    GameObject spriteObject = GetSpriteObjectAtPosition(row, col);
    //    if (spriteObject != null)
    //    {
    //        GameObject blankObject = GetSpriteObjectAtPosition(blankPosition.x, blankPosition.y);

    //        // Swap positions of the sprite and the blank space
    //        Vector3 tempPosition = spriteObject.transform.position;
    //        spriteObject.transform.position = blankObject.transform.position;
    //        blankObject.transform.position = tempPosition;

    //        // Update blank position
    //        blankPosition = new Vector2Int(row, col);
    //    }
    //}

    //private GameObject GetSpriteObjectAtPosition(int row, int col)
    //{
    //    // Calculate the index of the sprite object in the partObjects array
    //    int index = row * 3 + col;

    //    // Ensure the index is within the array bounds
    //    if (index >= 0 && index < GetComponent<SpriteSlicer>().partObjects.Length)
    //    {
    //        return GetComponent<SpriteSlicer>().partObjects[index];
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}
}
