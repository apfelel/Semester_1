using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public bool IsActive;

    [HideInInspector]
    public int Collectibles 
    {
        get
        {
            return _collectibles;
        }
        set
        {
            _collectibles = value;
            UIManager.Instance.UpdateGemCount(value);
            _playerVar.ResizeHair(value);
        } 
    }
    private int _collectibles;

    [HideInInspector]
    public GameObject Player;
    private PlayerVar _playerVar;
    [SerializeField]
    private GameObject _playerPrefab;
    private PlayerAnimationController _playerAnimController;
    private CinematicActor _playerCinematic;
    private PlayerController _playerController;
    private Rigidbody2D _playerRb;

    public GameObject SceneMainCam;
    [SerializeField]
    private RenderTexture _pixelatedTexture;
    private float _curScreenSize;

    [SerializeField]
    private float _nextLvlDelay;
    
    private Direction? _exit = null;
    private GameObject _entry;
    private SpawnPoint _spawnPoint;

    [HideInInspector]
    public bool Weakend;


    private void OnEnable()
    {
        if (IsActive)
            Activate();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        SceneMainCam = Camera.main.gameObject;
    }
    private void Update()
    {
        if (Player == null) return;

        UIManager.Instance.AllignPixelImage(SceneMainCam.transform.position, _curScreenSize, _pixelatedTexture.height);
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
        Player.GetComponentInChildren<PlayerAnimationActions>().StopAll();
        ResetPlayer();
        _playerAnimController.DeathAnim();
        StartCoroutine(DeathDelay());
    }
    private IEnumerator DeathDelay()
    {
        _playerCinematic.Freeze(1);
        yield return new WaitForSeconds(1);
        if (_spawnPoint == null)
            Player.transform.position = _entry.transform.position;
        else
            Player.transform.position = _spawnPoint.transform.position;

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
        _exit = dir;
        StartCoroutine(NextLvlDelay(lvl));
    }
    
    private IEnumerator NextLvlDelay( string lvl)
    {
        if (_exit == Direction.Left)
            _playerCinematic.MoveXSec(-1, _nextLvlDelay);
        else if (_exit == Direction.Right)
            _playerCinematic.MoveXSec(1, _nextLvlDelay);
        else
            _playerCinematic.MoveXSec(0, _nextLvlDelay);

        yield return new WaitForSeconds(_nextLvlDelay);
        SceneManager.LoadScene(lvl);

    }
    private void OnSceneChanged(Scene arg0, LoadSceneMode arg1)
    {
        SceneMainCam = Camera.main.gameObject;
        var cam = SceneMainCam.GetComponent<UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera>();
        _curScreenSize = cam.CorrectCinemachineOrthoSize(LVLManager.Instance.CamSize);
        _pixelatedTexture.Release();
        _pixelatedTexture.width = (int)((Screen.width / 16) * _curScreenSize);
        _pixelatedTexture.height = (int)((Screen.height / 16) * _curScreenSize);

        var player = ReloadPlayer();
        if (LVLManager.Instance.StartWeak)
            SetWeakenedState(true);

        if (_exit == null) return;

        _entry = GameObject.FindGameObjectsWithTag("Exit").FirstOrDefault(e => (int)e.GetComponent<Exit>().Direction == ((int)_exit + 6) % 4);
        if (_entry == null) return;

        player.transform.position = _entry.transform.position;
        if (_exit == Direction.Left)
            _playerCinematic.MoveXSec(-1, _nextLvlDelay);
        else if (_exit == Direction.Right)
            _playerCinematic.MoveXSec(1, _nextLvlDelay);

        if(_exit == Direction.Up)
        {
            _playerRb.AddForce(transform.up * 700);
        }
    }
    private GameObject ReloadPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if(Player == null)
        {
            Player = Instantiate(_playerPrefab).transform.GetChild(0).gameObject;
        }
        _playerVar = Player.GetComponent<PlayerVar>();
        _playerVar.ResizeHair(Collectibles);
        _playerAnimController = Player.GetComponent<PlayerAnimationController>();
        _playerCinematic = Player.GetComponent<CinematicActor>();
        _playerRb = Player.GetComponent<Rigidbody2D>();
        _playerController = Player.GetComponent<PlayerController>();
        return Player;
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

    public void SetWeakenedState(bool IsActive)
    {
        _playerController.WeakenedState(IsActive);
    }
}
