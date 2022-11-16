using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public bool IsActive;
    [HideInInspector]
    public int ChallengeCollectibles;
    [HideInInspector]
    public int Collectibles;

    [SerializeField]
    private float _delay;
    [SerializeField]
    private GameObject _playerPrefab;
    private GameObject _player;
    private Direction? _exit = null;
    private PlayerAnimationController _playerAnimController;
    private CinematicActor _playerCinematic;
    private Rigidbody2D _playerRb;
    private GameObject _entry;
    private SpawnPoint _spawnPoint;

    private void OnEnable()
    {
        if (IsActive)
            Activate();
    }
    public enum Direction
    {
        Up, Right, Down, Left, 
    }
    public void Activate()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
        IsActive = true;
    }
    public void Deactivate()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
        IsActive = false;
    }
    public void Die()
    {
        ResetPlayer();
        _playerAnimController.DeathAnim();
        StartCoroutine(DeathDelay());
    }
    private IEnumerator DeathDelay()
    {
        _playerCinematic.Freeze(1);
        yield return new WaitForSeconds(1);
        if (_spawnPoint == null)
            _player.transform.position = _entry.transform.position;
        else
            _player.transform.position = _spawnPoint.transform.position;

        _playerCinematic.MoveXSec(0, 2);
        yield return new WaitForSeconds(1);
        _playerAnimController.RespawnAnim();
        yield return new WaitForSeconds(1);
    }

    private void ResetPlayer()
    {
        _playerRb.velocity = Vector2.zero;
    }
    public void LoadNextLevel(string lvl, Direction dir)
    {
        Collectibles += ChallengeCollectibles;
        ChallengeCollectibles = 0;

        _exit = dir;
        StartCoroutine(NextLvlDelay(lvl));
    }
    
    private IEnumerator NextLvlDelay( string lvl)
    {
        if (_exit == Direction.Left)
            _playerCinematic.MoveXSec(-1, _delay);
        else if (_exit == Direction.Right)
            _playerCinematic.MoveXSec(1, _delay);
        else
            _playerCinematic.MoveXSec(0, _delay);

        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(lvl);

    }
    private void OnSceneChanged(Scene arg0, LoadSceneMode arg1)
    {
        var player = ReloadPlayer();
        if (_exit == null) return;

        _entry = GameObject.FindGameObjectsWithTag("Exit").FirstOrDefault(e => (int)e.GetComponent<Exit>().Direction == ((int)_exit + 6) % 4);
        if (_entry == null) return;

        player.transform.position = _entry.transform.position;
        if (_exit == Direction.Left)
            _playerCinematic.MoveXSec(-1, _delay);
        else if (_exit == Direction.Right)
            _playerCinematic.MoveXSec(1, _delay);

        if(_exit == Direction.Up)
        {
            _playerRb.AddForce(transform.up * 700);
        }

        Debug.Log("test");
    }
    private GameObject ReloadPlayer()
    {
        GameObject gb = GameObject.FindGameObjectWithTag("Player");
        if(gb != null)
        {
            Destroy(gb.transform.parent.gameObject);
        }

        _player = Instantiate(_playerPrefab).transform.GetChild(0).gameObject;
        _playerAnimController = _player.GetComponent<PlayerAnimationController>();
        _playerCinematic = _player.GetComponent<CinematicActor>();
        _playerRb = _player.GetComponent<Rigidbody2D>();
        return _player;
    }
    public bool CheckIfLeaving(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return _playerRb.velocity.y > 0.1f;
            case Direction.Down:
                return _playerRb.velocity.y < -0.1f;
            case Direction.Left:
                return _playerRb.velocity.x < -0.1f;
            case Direction.Right:
                return _playerRb.velocity.x > 0.1f;
                default: return false;

        }
    }

    public void NewSpawnPoint(SpawnPoint spawnPoint)
    {
        _spawnPoint?.ChangeState(false);
        _spawnPoint = spawnPoint;
        _spawnPoint.ChangeState(true);
        Debug.Log("New Checkpoint");
    }
}
