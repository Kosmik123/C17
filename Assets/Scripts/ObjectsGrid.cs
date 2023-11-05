using UnityEditor;
using UnityEngine;

public class ObjectsGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrototype;
    [SerializeField]
    private Vector3Int dimensions;
    [SerializeField]
    private Vector3 tileSize;

    [SerializeField]
    private bool recreateOnAwake;

    private void Awake()
    {
        if (recreateOnAwake)
            RecreateObjects();
    }

    [ContextMenu("Recreate")]
    private void RecreateObjects()
    {
        while (transform.childCount > 0)
            BetterDestroy(transform.GetChild(0).gameObject);

        Vector3 firstElementOffset = -Vector3.Scale(dimensions - Vector3.one, tileSize) / 2;
        for (int z = 0; z < dimensions.z; z++) 
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int x = 0; x < dimensions.x; x++)
                {
                    var spawned = BetterSpawn(objectPrototype, transform);
                    spawned.transform.localPosition = firstElementOffset + Vector3.Scale(new Vector3(x, y, z), tileSize);
                }
            } 
        }
    }

    private GameObject BetterSpawn(GameObject obj, Transform parent)
    {
        if (Application.isPlaying)
        {
            return Instantiate(objectPrototype, transform);
        }
        else
        {
#if UNITY_EDITOR
        return PrefabUtility.InstantiatePrefab(objectPrototype, transform) as GameObject;
#else
        return null;
#endif
        }
    }

    private void BetterDestroy(Object obj)
    {
        if (Application.isPlaying)
            Destroy(obj);
        else
            DestroyImmediate(obj);
    }


    private void OnValidate()
    {
        //RecreateObjects();
    }
}
