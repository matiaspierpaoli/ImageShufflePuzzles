using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PNGLoader : MonoBehaviour
{
    [SerializeField] private string folderPath = "Builds/Custom Images"; 
    [SerializeField] private Transform parentTransform; 
    [SerializeField] private string baseMaterialsFolder = "Assets/Materials/Custom/Base Images"; 
    [SerializeField] private string blackAndWhiteMaterialsFolder = "Assets/Materials/Custom/Black And White";
    [SerializeField] private string pixelatedMaterialsFolder = "Assets/Materials/Custom/Pixelated";
    [SerializeField] private string prefabsFolder = "Assets/Prefabs/Custom Game Pieces";
    [SerializeField] private RawImage customImagePrefab; 
    [SerializeField] private Transform customImagesParent;
    [SerializeField] private RawImage exampleImage;

    private List<RawImage> customImages = new List<RawImage>();

    private void Start()
    {
        LoadPNGs();
        exampleImage.texture = customImages.First().texture;
    }

    private void LoadPNGs()
    {
        // Get the full path to the folder
        string relativePath = Path.Combine(Application.dataPath, "..", folderPath);
        string fullPath = Path.GetFullPath(relativePath);

        // Check if the folder exists
        if (Directory.Exists(fullPath))
        {
            // Get all PNG files in the folder
            string[] files = Directory.GetFiles(fullPath, "*.png");

            foreach (string file in files)
            {
                // Load the PNG file as a Texture2D
                Texture2D texture = LoadTexture(file);

                if (texture != null)
                {
                    // Create and save material
                    string textureName = Path.GetFileNameWithoutExtension(file);
                    Material baseMaterial = CreateBaseMaterial(texture, textureName);
                    //Material blackAndWhiteMaterial = CreateBlackAndWhiteMaterial(texture, textureName);

                    // Create a quad and assign the material
                    GameObject quad = CreateQuad(textureName, baseMaterial);

                    // Save the quad as a prefab
                    SaveAsPrefab(quad, textureName);

                    RawImage newImage = Instantiate(customImagePrefab, customImagesParent);
                    newImage.texture = texture;

                    customImages.Add(newImage);
                }
            }
        }
        else
        {
            Debug.LogError($"Directory not found: {fullPath}");
        }
    }

    private Texture2D LoadTexture(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            return texture;
        }
        return null;
    }

    private Material CreateBaseMaterial(Texture2D texture, string textureName)
    {
        Material material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = texture;

        // Check if material or texture is null
        if (material == null)
        {
            Debug.LogError($"Failed to create material for {textureName}");
            return null;
        }
        if (material.mainTexture == null)
        {
            Debug.LogError($"Texture is null for {textureName}");
            return null;
        }

        // Create folder if it doesn't exist
        if (!Directory.Exists(baseMaterialsFolder))
        {
            Directory.CreateDirectory(baseMaterialsFolder);
        }

        // Save the material
        string materialPath = Path.Combine(baseMaterialsFolder, $"{textureName}.mat");
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(material, materialPath);
        UnityEditor.AssetDatabase.SaveAssets();
#endif

        return material;
    }

    private Material CreateBlackAndWhiteMaterial(Texture2D texture, string textureName)
    {
        Material material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = texture;

        // Check if material or texture is null
        if (material == null)
        {
            Debug.LogError($"Failed to create material for {textureName}");
            return null;
        }
        if (material.mainTexture == null)
        {
            Debug.LogError($"Texture is null for {textureName}");
            return null;
        }

        // Create folder if it doesn't exist
        if (!Directory.Exists(blackAndWhiteMaterialsFolder))
        {
            Directory.CreateDirectory(blackAndWhiteMaterialsFolder);
        }

        // Save the material
        string materialPath = Path.Combine(blackAndWhiteMaterialsFolder, $"{textureName}.mat");
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(material, materialPath);
        UnityEditor.AssetDatabase.SaveAssets();
#endif

        return material;
    }

    private GameObject CreateQuad(string name, Material material)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = name;
        quad.transform.SetParent(parentTransform);
        quad.transform.localScale = Vector3.one;

        // Ensure the MeshCollider is destroyed immediately
        MeshCollider meshCollider = quad.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            DestroyImmediate(meshCollider);
        }

        // Add BoxCollider2D
        quad.AddComponent<BoxCollider2D>();

        // Assign the material
        quad.GetComponent<Renderer>().material = material;

        return quad;
    }

    private void SaveAsPrefab(GameObject quad, string name)
    {
        // Create folder if it doesn't exist
        if (!Directory.Exists(prefabsFolder))
        {
            Directory.CreateDirectory(prefabsFolder);
        }

        // Save the quad as a prefab
        string prefabPath = Path.Combine(prefabsFolder, $"{name}.prefab");
#if UNITY_EDITOR
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(quad, prefabPath);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
}
