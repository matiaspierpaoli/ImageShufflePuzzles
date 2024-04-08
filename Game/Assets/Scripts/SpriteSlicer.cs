using UnityEngine;

public class SpriteSlicer : MonoBehaviour
{
    public Sprite originalSprite; // Reference to the original sprite
    public GameObject[] partObjects; // Array to store empty GameObjects representing sliced parts

    void Start()
    {
        SliceSprite();
    }

    void SliceSprite()
    {
        // Calculate width and height of each slice
        float sliceWidth = originalSprite.texture.width / 3f;
        float sliceHeight = originalSprite.texture.height / 3f;

        // Calculate scale factors for the sliced parts
        float scaleX = 1f / 3f;
        float scaleY = 1f / 3f;

        // Iterate through each part GameObject and assign the sliced sprite
        for (int i = 0; i < partObjects.Length; i++)
        {
            // Calculate texture coordinates for the current slice
            int row = i / 3;
            int col = i % 3;
            Rect textureRect = new Rect(sliceWidth * col, sliceHeight * row, sliceWidth, sliceHeight);

            // Create a new sprite using the current slice
            Sprite slicedSprite = Sprite.Create(originalSprite.texture, textureRect, new Vector2(0.5f, 0.5f));

            // Get the SpriteRenderer component of the current part GameObject and assign the sliced sprite
            SpriteRenderer spriteRenderer = partObjects[i].GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                // If there is no SpriteRenderer component, add one
                spriteRenderer = partObjects[i].AddComponent<SpriteRenderer>();
            }
            spriteRenderer.sprite = slicedSprite;

            // Scale the part GameObject to match the size of the sliced part
            partObjects[i].transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }
}
