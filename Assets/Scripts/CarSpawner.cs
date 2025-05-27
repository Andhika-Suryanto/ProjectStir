using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Car Prefabs")]
    public GameObject[] carPrefabs; // Assign all possible car prefabs in the Inspector

    [Header("Spawn Settings")]
    public Transform spawnPoint; // Set the spawn location in the Inspector

    private void Start()
    {
        SpawnSelectedCar();
    }

    private void SpawnSelectedCar()
    {
        int selectedCarIndex = PlayerPrefs.GetInt("SelectedCar", 0); // Default to the first car if none is set

        if (selectedCarIndex < 0 || selectedCarIndex >= carPrefabs.Length)
        {
            Debug.LogError("🚨 Selected car index is out of range!");
            return;
        }

        // Spawn the selected car
        GameObject spawnedCar = Instantiate(carPrefabs[selectedCarIndex], spawnPoint.position, spawnPoint.rotation);
        Debug.Log("✅ Spawned car: " + spawnedCar.name);

        // Assign car to GameManager and Damage script
        AssignCarToScripts(spawnedCar);
    }

    private void AssignCarToScripts(GameObject car)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.car = car.GetComponent<RCC_CarControllerV3>();
            Debug.Log("✅ Assigned car to GameManager.");
        }
        else
        {
            Debug.LogWarning("⚠️ GameManager not found in the scene!");
        }

        Damage damageScript = FindObjectOfType<Damage>(); // Ensure Damage script is on GameManager
        if (damageScript != null)
        {
            Debug.Log("✅ Found Damage script, assigning car parts...");

            // Automatically assign car parts
            damageScript.bumperFPart = FindPart(car, "Bumper_F");
            damageScript.bumperRPart = FindPart(car, "Bumper_R");
            damageScript.hood = FindPart(car, "Hood");
            damageScript.trunk = FindPart(car, "Trunk");
            damageScript.doorR = FindPart(car, "Door_R");
            damageScript.doorL = FindPart(car, "Door_L");

            Debug.Log("✅ Assigned car parts to Damage script.");
        }
        else
        {
            Debug.LogWarning("⚠️ Damage script not found in the scene!");
        }
    }

    /// <summary>
    /// Finds and returns an RCC_DetachablePart by name.
    /// </summary>
    private RCC_DetachablePart FindPart(GameObject car, string partName)
    {
        Transform partTransform = car.transform.Find(partName);
        if (partTransform != null)
        {
            return partTransform.GetComponent<RCC_DetachablePart>();
        }
        return null;
    }
}
