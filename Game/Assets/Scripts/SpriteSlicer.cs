using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSlicer : MonoBehaviour
{
    [SerializeField] private Transform piecePrefab;
    [SerializeField] private Transform gameTransform;
    [SerializeField] private float gapThickness;
    
    public int gridSize;
    [HideInInspector] public int emptyLocation;
    [HideInInspector] public List<Transform> pieces;

    private void Start()
    {
        emptyLocation = -1;
        pieces = new List<Transform>();
        CreateGamePieces();
    }

    public void CreateGamePieces()
    {
        float width = 1 / (float)gridSize;
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                    +1 - (2 * width * row) - width,
                    0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * gridSize) + col}";
                // We want an empty space in the bottom right.
                if ((row == gridSize - 1) && (col == gridSize - 1))
                {
                    emptyLocation = (gridSize * gridSize) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    // UV coord order: (0, 1), (1, 1), (0, 0), (1, 0)
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
                    mesh.uv = uv;
                }
            }
        }
    }
}
