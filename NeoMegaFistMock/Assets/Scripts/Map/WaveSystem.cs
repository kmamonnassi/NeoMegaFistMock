using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private WaveData[] waves;
	[SerializeField] private Text waveText;
	[SerializeField] private EnemySpawner spawner;
	[SerializeField] private Vector3 positionMin;
	[SerializeField] private Vector3 positionMax;
	[SerializeField] private Vector3 fixedPosition;

	private int wave = -1;
	private float waitInterval;
	private List<StageObjectID> summonObjects = new List<StageObjectID>();

	private void Update()
	{
		if(wave == -1 && Input.GetKeyDown(KeyCode.Space))
		{
			NextWave();
		}
		if (wave == -1) return;

		if(summonObjects.Count == 0 && spawner.Instances.Count == 0)
		{
			NextWave();
		}

		if(summonObjects.Count > 0)
		{
			if (waitInterval > waves[wave].interval)
			{
				waitInterval = 0;
				StageObjectID id = summonObjects[0];

				float posX;
				float posY;
				if (Random.Range(0, 2) == 0)
				{
					posX = Random.Range(0, 2) == 0 ? fixedPosition.x : -fixedPosition.x;
					posY = Random.Range(positionMin.y, positionMax.y);
				}
				else
				{
					posX = Random.Range(positionMin.x, positionMax.x);
					posY = Random.Range(0, 2) == 0 ? fixedPosition.y : -fixedPosition.y;
				}

				Vector3 position = new Vector3(posX, posY);

				spawner.InstantiateStageObject(id, position, transform);

				summonObjects.RemoveAt(0);
			}
			else
			{
				waitInterval += Time.deltaTime;
			}
		}
		
	}

	private void NextWave()
	{
		wave++;
		waveText.text = "Wave : " + (wave + 1);
		summonObjects = new List<StageObjectID>();
		for(int i = 0; i < waves[wave].objs.Count; i++)
		{
			for(int j = 0; j < waves[wave].counts[i]; j++)
			{
				summonObjects.Add(waves[wave].objs[i]);
			}
		}
		summonObjects = summonObjects.OrderBy(a => Guid.NewGuid()).ToList();
	}
}

[System.Serializable]
public class WaveData
{
    public List<StageObjectID> objs;
    public List<int> counts;
    public float interval;
}