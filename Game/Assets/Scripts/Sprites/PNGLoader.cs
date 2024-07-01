using System.IO;
using UnityEngine;

public class PNGLoader : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Transform parentTransform; 

    [Header("PNG Resources")]
    [SerializeField] private string folderPath = "Builds/Custom Images"; 

    [Header("Materials")]
    [SerializeField] private string normalMaterialsFolder = "Assets/Materials/Custom/BaseImages"; 
    [SerializeField] private string grayScaleMaterialsFolder = "Assets/Materials/Custom/BlackAndWhite"; 
    [SerializeField] private string pixelatedMaterialsFolder = "Assets/Materials/Custom/Pixelated";

    [Header("Prefabs")]
    [SerializeField] private string basePrefabsFolder = "Assets/Prefabs/Custom Game Pieces/BasePieces"; 
    [SerializeField] private string grayScalePrefabsFolder = "Assets/Prefabs/Custom Game Pieces/GrayScalePieces"; 
    [SerializeField] private string pixelatedPrefabsFolder = "Assets/Prefabs/Custom Game Pieces/PixelatedPieces"; 

    private void Start()
    {
        ClearDirectory(normalMaterialsFolder);
        ClearDirectory(grayScaleMaterialsFolder);
        ClearDirectory(pixelatedMaterialsFolder);

        ClearDirectory(basePrefabsFolder);
        ClearDirectory(grayScalePrefabsFolder);
        ClearDirectory(pixelatedPrefabsFolder);

        LoadPNGs();
    }

    private void OnApplicationQuit()
    {
        ClearDirectory(normalMaterialsFolder);
        ClearDirectory(grayScaleMaterialsFolder);
        ClearDirectory(pixelatedMaterialsFolder);

        ClearDirectory(basePrefabsFolder);
        ClearDirectory(grayScalePrefabsFolder);
        ClearDirectory(pixelatedPrefabsFolder);
    }

    private void ClearDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                subDirectory.Delete(true);
            }
        }
        else
        {
            Directory.CreateDirectory(path);
        }
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
                    Debug.Log($"Successfully loaded texture: {file}");

                    // Create and save materials
                    string textureName = Path.GetFileNameWithoutExtension(file);
                    Material normalMaterial = CreateMaterial(texture, textureName, "Unlit/Texture", normalMaterialsFolder);
                    Material grayscaleMaterial = CreateMaterial(texture, textureName, "Custom/GrayscaleShader", grayScaleMaterialsFolder);
                    Material pixelatedMaterial = CreateMaterial(texture, textureName, "Custom/PixelatedShader", pixelatedMaterialsFolder);

                    if (normalMaterial != null && grayscaleMaterial != null && pixelatedMaterial != null)
                    {
                        // Create quads and assign materials
                        GameObject baseQuad = CreateQuad(textureName + "_Base", normalMaterial);
                        GameObject grayscaleQuad = CreateQuad(textureName + "_Grayscale", grayscaleMaterial);
                        GameObject pixelatedQuad = CreateQuad(textureName + "_Pixelated", pixelatedMaterial);

                        // Save the quads as prefabs
                        SaveAsPrefab(baseQuad, textureName + "_Base", basePrefabsFolder);
                        SaveAsPrefab(grayscaleQuad, textureName + "_Grayscale", grayScalePrefabsFolder);
                        SaveAsPrefab(pixelatedQuad, textureName + "_Pixelated", pixelatedPrefabsFolder);
                    }
                }
                else
                {
                    Debug.LogError($"Failed to load texture: {file}");
                }
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
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
            Debug.Log($"Loaded texture from {filePath} with size {texture.width}x{texture.height}");
            return texture;
        }
        else
        {
            Debug.LogError($"Failed to load texture from {filePath}");
        }
        return null;
    }

    private Material CreateMaterial(Texture2D texture, string textureName, string shaderName, string materialsPath)
    {
        Shader shader = Shader.Find(shaderName);
        if (shader == null)
        {
            Debug.LogError($"Shader not found: {shaderName}");
            return null;
        }

        Material material = new Material(shader);
        material.mainTexture = texture;

        // Check if material or texture is null
        if (material == null)
        {
            Debug.LogError($"Failed to create material for {textureName} using {shaderName}");
            return null;
        }
        if (material.mainTexture == null)
        {
            Debug.LogError($"Texture is null for {textureName} using {shaderName}");
            return null;
        }

        // Create folder if it doesn't exist
        if (!Directory.Exists(materialsPath))
        {
            Directory.CreateDirectory(materialsPath);
        }

        // Save the material
        string materialPath = Path.Combine(materialsPath, $"{textureName}_{shaderName.Replace('/', '_')}.mat");
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

    private void SaveAsPrefab(GameObject quad, string name, string prefabsPath)
    {
        // Create folder if it doesn't exist
        if (!Directory.Exists(prefabsPath))
        {
            Directory.CreateDirectory(prefabsPath);
        }

        // Save the quad as a prefab
        string prefabPath = Path.Combine(prefabsPath, $"{name}.prefab");
#if UNITY_EDITOR
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(quad, prefabPath);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
