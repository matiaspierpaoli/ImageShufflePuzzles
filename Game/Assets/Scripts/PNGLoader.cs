using System.IO;
using UnityEngine;

public class PNGLoader : MonoBehaviour
{
    [SerializeField] private string folderPath = "Builds/Custom Images"; // Relative path within the build
    [SerializeField] private Transform parentTransform; // Parent transform to attach loaded images
    [SerializeField] private string materialsFolder = "Assets/Materials"; // Folder to save materials
    [SerializeField] private string prefabsFolder = "Assets/Prefabs"; // Folder to save prefabs

    private void Start()
    {
        LoadPNGs();
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
                    Material material = CreateMaterial(texture, textureName);

                    // Create a quad and assign the material
                    GameObject quad = CreateQuad(textureName, material);

                    // Save the quad as a prefab
                    SaveAsPrefab(quad, textureName);
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

    private Material CreateMaterial(Texture2D texture, string textureName)
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
        if (!Directory.Exists(materialsFolder))
        {
            Directory.CreateDirectory(materialsFolder);
        }

        // Save the material
        string materialPath = Path.Combine(materialsFolder, $"{textureName}.mat");
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
