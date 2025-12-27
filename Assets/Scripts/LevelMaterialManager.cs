using UnityEngine;
using UnityEngine.UI;

public class LevelMaterialController : MonoBehaviour
{
    // Singleton instance so other scripts (like ScoreManager) can find this easily
    public static LevelMaterialController Instance;

    [Header("Target UI Images")]
    [Tooltip("Drag your Earth, Satellite, and any other themed images here.")]
    [SerializeField] private Image[] targetImages;

    [Header("Level Materials")]
    [Tooltip("Assign 10 materials here. Element 0 = Level 1, etc.")]
    [SerializeField] private Material[] levelMaterials = new Material[10];

    private void Awake()
    {
        // Initialize Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Core logic to swap materials across all registered images.
    /// </summary>
    private void ApplyMaterial(int index)
    {
        if (index < 0 || index >= levelMaterials.Length)
        {
            Debug.LogWarning($"LevelMaterialController: Index {index} is out of range!");
            return;
        }

        Material selectedMat = levelMaterials[index];

        if (selectedMat == null)
        {
            Debug.LogWarning($"LevelMaterialController: No material assigned for index {index}");
            return;
        }

        foreach (Image img in targetImages)
        {
            if (img != null)
            {
                img.material = selectedMat;
            }
        }
    }

    // --- Public Methods for External Reference ---

    public void SetToLevel1() => ApplyMaterial(0);
    public void SetToLevel2() => ApplyMaterial(1);
    public void SetToLevel3() => ApplyMaterial(2);
    public void SetToLevel4() => ApplyMaterial(3);
    public void SetToLevel5() => ApplyMaterial(4);
    public void SetToLevel6() => ApplyMaterial(5);
    public void SetToLevel7() => ApplyMaterial(6);
    public void SetToLevel8() => ApplyMaterial(7);
    public void SetToLevel9() => ApplyMaterial(8);
    public void SetToLevel10() => ApplyMaterial(9);

    /// <summary>
    /// Generic method if you prefer to pass the level number directly.
    /// </summary>
    public void SetLevel(int levelNumber)
    {
        ApplyMaterial(levelNumber - 1);
    }
}