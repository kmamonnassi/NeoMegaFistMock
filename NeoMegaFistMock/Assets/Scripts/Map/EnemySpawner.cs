using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private StageObject[] prefabs;

    private List<StageObject> instances = new List<StageObject>();

    public IReadOnlyList<StageObject> Instances => instances;

    public void InstantiateStageObject(StageObjectID id, Vector3 position, Transform parent = null)
    {
        StageObject prefab = null;
        for(int i = 0; i < prefabs.Length; i++)
        {
            if(prefabs[i].ID == id)
            {
                prefab = prefabs[i];
                break;
			}
		}
        StageObject instance = Instantiate(prefab, position, Quaternion.identity, parent);
        instances.Add(instance);
        instance.OnKill += () => instances.Remove(instance);
    }
}
