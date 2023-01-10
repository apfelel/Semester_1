using Cinemachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public bool IsActive;
    private bool _hasGrapple;
    private bool _hasGloves;
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
            PlayerVar.ResizeHair(value);
        } 
    }
    private int _collectibles;

    [HideInInspector]
    public GameObject Player;
    [HideInInspector]
    public PlayerVar PlayerVar;
    [HideInInspector]
    public CinematicActor PlayerCinematic;
    [HideInInspector]
    public PlayerController PlayerController;
    [HideInInspector]
    public PlayerAnimationController PlayerAnimController;
    private Rigidbody2D _playerRb;
    [SerializeField]
    private GameObject _playerPrefab;

    public string CurControlScheme;
    [HideInInspector]
    public GameObject SceneMainCam;
    [HideInInspector]
    public CinemachineVirtualCamera SceneVCam;
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


    private bool _firstLVLZoom = false;
    public Action<float> OnScreensizeChange;

    public void ChangeScreensize(float size)
    {
        OnScreensizeChange?.Invoke(size);
        RecalculatePixelTexture(size);
        Debug.Log("test");
    }
    private void RecalculatePixelTexture(float camSize)
    {
        Debug.Log(camSize +"--------");
        var cam = SceneMainCam.GetComponent<UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera>();
        _curScreenSize = cam.CorrectCinemachineOrthoSize(camSize);
        _pixelatedTexture.Release();
        _pixelatedTexture.width = (int)(_curScreenSize * 32 * (16f / 9));
        _pixelatedTexture.height = (int)(_curScreenSize * 32);
    }
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
    private void LateUpdate()
    {
        if (Player == null) return;
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
        PlayerAnimController.DeathAnim();
        StartCoroutine(DeathDelay());
    }
    private IEnumerator DeathDelay()
    {
        PlayerCinematic.Freeze(1);
        yield return new WaitForSeconds(1);
        if (_spawnPoint == null)
        {
            WalkOutExit();
            PlayerAnimController.Idle();
        }
        else
        {
            PlayerCinematic.MoveXSec(0, 2);
            Player.transform.position = _spawnPoint.transform.position;

            yield return new WaitForSeconds(1);
            PlayerAnimController.RespawnAnim();
            yield return new WaitForSeconds(1);
        }
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
            PlayerCinematic.MoveXSec(-1, _nextLvlDelay);
        else if (_exit == Direction.Right)
            PlayerCinematic.MoveXSec(1, _nextLvlDelay);
        else
            PlayerCinematic.MoveXSec(0, _nextLvlDelay);

        yield return new WaitForSeconds(_nextLvlDelay);
        SceneManager.LoadScene(lvl);

    }
    private void OnSceneChanged(Scene arg0, LoadSceneMode arg1)
    {
        _spawnPoint = null;
        ReloadPlayer();
        if (LVLManager.Instance != null)
        {
            ChangeScreensize(LVLManager.Instance.CamSize);
        }
        Debug.Log(SceneManager.GetActiveScene().name);
        Debug.Log(_firstLVLZoom);
        if (SceneManager.GetActiveScene().name == "L_0" & !_firstLVLZoom)
        {
            _firstLVLZoom = true;
            ChangeScreensize(5);
        }

        if (_exit == null) return;
        _entry = GameObject.FindGameObjectsWithTag("Exit").FirstOrDefault(e => (int)e.GetComponent<Exit>().Direction == ((int)_exit + 6) % 4);
        if (_entry == null) return;

        WalkOutExit();

        
    }
    public void WalkOutExit()
    {
        Player.transform.position = _entry.transform.position;
        if (_exit == Direction.Left)
        {
            PlayerCinematic.MoveXSec(-1, _nextLvlDelay);
            Player.transform.position -= new Vector3(0, _entry.transform.localScale.y / 2);
        }
        else if (_exit == Direction.Right)
        {
            PlayerCinematic.MoveXSec(1, _nextLvlDelay);
            Player.transform.position -= new Vector3(0, _entry.transform.localScale.y / 2);
        }

        if (_exit == Direction.Up)
        {
            _playerRb.AddForce(transform.up * 700);
        }
    }

    private void ReloadPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if(Player == null)
        {
            Player = Instantiate(_playerPrefab).transform.GetChild(0).gameObject;
        }
        PlayerVar = Player.GetComponent<PlayerVar>();
        PlayerVar.ResizeHair(Collectibles);
        PlayerAnimController = Player.GetComponent<PlayerAnimationController>();
        PlayerCinematic = Player.GetComponent<CinematicActor>();
        _playerRb = Player.GetComponent<Rigidbody2D>();
        PlayerController = Player.GetComponent<PlayerController>();
        SceneMainCam = Camera.main.gameObject;
        SceneVCam = Player.transform.parent.GetComponentInChildren<CinemachineVirtualCamera>();
        PlayerVar.HasGloves = _hasGloves;
        PlayerVar.HasGrapple = _hasGrapple;
        PlayerController.WeakenedState(Weakend);
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
        if (spawnPoint == _spawnPoint) return;
        _spawnPoint?.ChangeState(false);
        _spawnPoint = spawnPoint;
        _spawnPoint.ChangeState(true);
    }

    public void SetWeakenedState(bool IsActive)
    {
        Weakend = IsActive;
        PlayerController.WeakenedState(IsActive);
    }
    [Button("Unlock Grapple")]
    public void UnlockGrapple()
    {
        _hasGrapple = true;
        PlayerVar.HasGrapple = true;
    }
    [Button("Unlock Gloves")]
    public void UnlockGloves()
    {
        _hasGloves = true;
        PlayerVar.HasGloves = true;
    }
}
