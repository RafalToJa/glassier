using UnityEngine;
using Lean.Pool;
using System.Collections.Generic;

public class ObjectSpawning : MonoBehaviour
{
	public int nrOfSpawn = 0;

	public int numberOfBasketPrefab = 1;
	public int nrOfBasketSpawn = 0;

	[SerializeField] private GameObject[] _prefabs = null;
	private Transform _playerTransform = null;
	[SerializeField] private float _spawnHeight = 1.0f;
	[SerializeField] private Transform _spawnStack;
	private List<GameObject> _basketPrefabs, _restPrefabs;
	private List<string> _namesOfBAscetPrefabs;
	//pozycje gracza, by obiekt przypadkiem nie zrespi³ siê na graczu
	private float _xMinPlayerSpawnPos, _xMaxPlayerSpawnPos, _zMinPlayerSpawnPos, _zMaxPlayerSpawnPos;
	//pozycje platformy gdzie maj¹ siê respiæ obiekty
	private float _xMinSpawnPos, _xMaxSpawnPos, _zMinSpawnPos, _zMaxSpawnPos;

	private float xSpawnPos, ySpawnPos, zSpawnPos;
	private Vector3 spawnPos;
	private int randomPrefab;

    public List<string> NamesOfBAscetPrefabs { get => _namesOfBAscetPrefabs;}

    public void SpawnObjects()
	{
		_restPrefabs = new List<GameObject>(_prefabs);
		_basketPrefabs=new List<GameObject>();
		_namesOfBAscetPrefabs = new List<string>();
		for (int i = 0; i < numberOfBasketPrefab; i++)
        {
			if (_restPrefabs.Count > 0)
			{
				_basketPrefabs.Add(_restPrefabs[Random.Range(0, _restPrefabs.Count)]);
				_restPrefabs.Remove(_basketPrefabs[i]);
				_namesOfBAscetPrefabs.Add(_basketPrefabs[i].name);
			}
			else
            {
				break;
            }
		}

		for (int i = 1; i <= nrOfSpawn; i++)
		{
				randomPrefab = Random.Range(0, _restPrefabs.Count);

			spawnPos = spawnPos = GenSpawnPoint();
			LeanPool.Spawn(_prefabs[randomPrefab], spawnPos, _prefabs[randomPrefab].transform.rotation *
				Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f),0.0f), _spawnStack);
		}
		for (int j = 0; j < _basketPrefabs.Count; j++)
		{
			for (int i = 1; i <= Random.Range(1,nrOfBasketSpawn); i++)
			{
				spawnPos = spawnPos = GenSpawnPoint();
				GameObject go=LeanPool.Spawn(_basketPrefabs[j], spawnPos, _prefabs[j].transform.rotation *
					Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), _spawnStack);
				//go.GetComponent<ObjectController>().toBascet = true;
			}
		}

	}
	private Vector3 GenSpawnPoint()
    {
		//wylosuj pozycje dopóki pozycja jest na platforaomie i nie nad graczem
		do
		{
			xSpawnPos = Random.Range(_xMinSpawnPos, _xMaxSpawnPos);
		}
		while (xSpawnPos > _xMinPlayerSpawnPos && xSpawnPos < _xMaxPlayerSpawnPos);
		do
		{
			zSpawnPos = Random.Range(_zMinSpawnPos, _zMaxSpawnPos);
		}
		while (zSpawnPos > _zMinPlayerSpawnPos && zSpawnPos < _zMaxPlayerSpawnPos);
		ySpawnPos = transform.position.y + transform.localScale.z / 2.0f + _spawnHeight;

		spawnPos = new Vector3(xSpawnPos, ySpawnPos, zSpawnPos);
		return spawnPos;
	}
	

	private void Awake()
	{
		_xMinSpawnPos = -transform.localScale.x / 2.0f;
		_xMaxSpawnPos = transform.localScale.x / 2.0f;
		_zMinSpawnPos = -transform.localScale.z / 2.0f;
		_zMaxSpawnPos = transform.localScale.z / 2.0f;
	}

	private void Start()
	{
		_playerTransform = PlayerSpawner.Instance.Player.transform;
		_xMinPlayerSpawnPos = _playerTransform.position.x - _playerTransform.localScale.x / 2.0f;
		_xMaxPlayerSpawnPos = _playerTransform.position.x + _playerTransform.localScale.x / 2.0f;
		_zMinPlayerSpawnPos = _playerTransform.position.z - _playerTransform.localScale.z / 2.0f;
		_zMaxPlayerSpawnPos = _playerTransform.position.z + _playerTransform.localScale.z / 2.0f;
	}
}
