using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class PlayerSpawner : MonoSingleton<PlayerSpawner>
{
    private GameObject _player;
    private Rigidbody _playerRb;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private bool _spawnOnAwake;

    public GameObject Player { get => _player;}
    public Rigidbody PlayerRb { get => _playerRb;}

    protected override void Awake()
    {
        base.Awake();
        if(_spawnOnAwake)
        {
            SpawnPlayer();
        }
    }
    public void SpawnPlayer()
    {
        _player = LeanPool.Spawn(_playerPrefab, this.transform);
        _playerRb = _player.GetComponent<Rigidbody>();
        _playerRb.velocity = Vector3.zero;
        _playerRb.angularVelocity = Vector3.zero;
        CameraMenager.Instance.FollowThistarget(_player.transform);
    }
    public void DespawnPlayer()
    {
        if (_player != null)
        {
            LeanPool.Despawn(_player);
        }
        CameraMenager.Instance.DeactivatePlayerFollow();
    }
    public void RespawnPlayer()
    {
        DespawnPlayer();
        SpawnPlayer();
    }
}
