using UnityEngine;
using UnityEngine.UI;

public class SpriteSlicer : MonoBehaviour
{
    public Texture2D originalTexture;
    public GameObject[] partObjects; 

    private void Start()
    {
        SliceAndAssingSprites();
    }

    void SliceAndAssingSprites()
    {
        Sprite[] slicedSprites = SliceTexture(originalTexture);

        Sprite[] shuffledSprites = ShuffleArray(slicedSprites);

        for (int i = 0; i < partObjects.Length; i++)
        {
            Image spriteRenderer = partObjects[i].GetComponent<Image>();
            spriteRenderer.sprite = shuffledSprites[i];
        }

        partObjects[partObjects.Length - 1].SetActive(false);
    }

    Sprite[] SliceTexture(Texture2D texture)
    {
        // Calculate width and height of each slice
        int sliceWidth = texture.width / 3;
        int sliceHeight = texture.height / 3;

        // Initialize an array to store sliced sprites
        Sprite[] slicedSprites = new Sprite[9];

        // Generate sprite slices
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                // Calculate texture coordinates for the current slice
                Rect textureRect = new Rect(col * sliceWidth, row * sliceHeight, sliceWidth, sliceHeight);

                // Create a new sprite using the current slice
                Sprite slicedSprite = Sprite.Create(texture, textureRect, new Vector2(0.5f, 0.5f));

                // Store the sliced sprite in the array
                slicedSprites[row * 3 + col] = slicedSprite;
            }
        }

        return slicedSprites;
    }

    Sprite[] ShuffleArray(Sprite[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Sprite temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
    }
}
