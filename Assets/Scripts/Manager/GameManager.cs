using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private float _delay;
    [SerializeField]
    private GameObject _playerPrefab;
    private Direction? _exit = null;
    private PlayerController _playerController;
    private CinematicActor _playerCinematic;
    private Rigidbody2D _playerRb;

    public enum Direction
    {
        Up, Right, Down, Left, 
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
        DontDestroyOnLoad(this);
    }
    public void ReloadScene()
    {
        _playerController.Die();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel(string lvl, Direction dir)
    {
        _exit = dir;
        StartCoroutine(NextLvlDelay(lvl));
    }
    
    private IEnumerator NextLvlDelay( string lvl)
    {
        if (_exit == Direction.Left)
            _playerCinematic.MoveXSec(-1, _delay);
        else if (_exit == Direction.Right)
            _playerCinematic.MoveXSec(1, _delay);

        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(lvl);

    }
    private void OnSceneChanged(Scene arg0, LoadSceneMode arg1)
    {
        var player = ReloadPlayer();
        if (_exit == null) return;

        var entry = GameObject.FindGameObjectsWithTag("Exit").FirstOrDefault(e => (int)e.GetComponent<Exit>().Direction == ((int)_exit + 6) % 4);
        if (entry == null) return;

        player.transform.position = entry.transform.position;
        if (_exit == Direction.Left)
            _playerCinematic.MoveXSec(-1, _delay);
        else if (_exit == Direction.Right)
            _playerCinematic.MoveXSec(1, _delay);

        if(_exit == Direction.Up)
        {
            _playerRb.AddForce(transform.up * 700);
        }
    }
    private GameObject ReloadPlayer()
    {
        var player = Instantiate(_playerPrefab).transform.GetChild(0).gameObject;
        _playerController = player.GetComponent<PlayerController>();
        _playerCinematic = player.GetComponent<CinematicActor>();
        _playerRb = player.GetComponent<Rigidbody2D>();
        return player;
    }
    public bool CheckIfLeaving(Direction dir)
    {
        Debug.Log(dir + "   " + _playerRb.velocity.y);
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
}
