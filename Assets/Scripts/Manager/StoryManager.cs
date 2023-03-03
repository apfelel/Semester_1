using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private string _nextLVL;
    [SerializeField]
    private Renderer _imageRenderer;
    [SerializeField]
    private List<StoryUnit> _storyUnits;
    [SerializeField]
    private TextMeshProUGUI _bottomText;

    [SerializeField]
    private ParticleSystem _particle;

    [SerializeField]
    private SpriteRenderer _image1;

    [SerializeField]
    private Image _skipImage;

    private float _skipTimer;

    private PlayerInputActions _playerInputActions;
    private InputAction _pauseAction;
    private InputAction _jumpAction;

    [System.Serializable]
    private class StoryUnit
    {
        [TextArea]
        public string Text;
        [HorizontalGroup("a")]
        public bool Fire;
        [ShowIf("Fire")]
        public float ParticleAmount;
        [HorizontalGroup("a")]
        public bool Smelt;
        public SpriteRenderer FadeIn;
        public SpriteRenderer FadeOut;
    }
    private void OnEnable()
    {
        if(GameManager.Instance.IsActive)
            GameManager.Instance.Deactivate();
        _playerInputActions = new PlayerInputActions();
        _pauseAction = _playerInputActions.Player.Pause;
        _pauseAction.Enable();
        _jumpAction = _playerInputActions.Player.Jump;
        _jumpAction.Enable();
    }

    private void OnDisable()
    {
        _pauseAction.Disable();
        _jumpAction.Disable();
    }
    void Start()
    {
        StartCoroutine(Sequence());
    }

    private void Update()
    {
        if(_jumpAction.ReadValue<float>() != 0 || _pauseAction.ReadValue<float>() != 0)
        {
            _skipTimer += Time.deltaTime;
            if(_skipTimer > 2)
            {
                if(_nextLVL == "L_0")
                    GameManager.Instance.Activate();
                else
                    GameManager.Instance.Deactivate();

                SceneManager.LoadScene(_nextLVL);
            }
        }
        else
        {
            if (_skipTimer > 0)
            {
                _skipTimer -= Time.deltaTime;
            }
        }
        _skipImage.fillAmount = _skipTimer / 2;
    }
    private IEnumerator Sequence()
    {
        for(int StoryIndex = 0; StoryIndex < _storyUnits.Count; StoryIndex++)
        {
            _bottomText.text = "";
            if (_storyUnits[StoryIndex].FadeIn)
                StartCoroutine(FadeIn(_storyUnits[StoryIndex].FadeIn));
            if (_storyUnits[StoryIndex].FadeOut)
                StartCoroutine(FadeOut(_storyUnits[StoryIndex].FadeOut));

            if (_storyUnits[StoryIndex].Fire)
            {
                if (!_particle.isPlaying)
                {
                    SoundManager.Instance.PlaySound("Torch", 0.5f);
                    var particleMain = _particle.emission;
                    particleMain.rateOverTime = _storyUnits[StoryIndex].ParticleAmount;
                    _particle.Play();
                }
            }
            else
            {
                if (_particle.isPlaying)
                {
                    _particle.Stop();
                }
            }
            if(_storyUnits[StoryIndex].Smelt)
                StartCoroutine(Smelt());

            for (int i = 0; i < _storyUnits[StoryIndex].Text.Length; i++)
            {
                _bottomText.text += _storyUnits[StoryIndex].Text[i];
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(4);
        }



        if (_nextLVL == "L_0")
            GameManager.Instance.Activate();
        else
            GameManager.Instance.Deactivate();

        SceneManager.LoadScene(_nextLVL);
    }

    private IEnumerator FadeIn(SpriteRenderer sprite)
    {
        for(int i = 0; i < 100; i++)
        {
            sprite.color = new Color(1, 1, 1, i / 100f);
            yield return new WaitForFixedUpdate();
        }
            sprite.color = new Color(1, 1, 1, 1);
    }
    private IEnumerator FadeOut(SpriteRenderer sprite)
    {
        for (int i = 100; i > 0; i--)
        {
            sprite.color = new Color(1, 1, 1, i / 100f);
            yield return new WaitForFixedUpdate();
        }
            sprite.color = new Color(1, 1, 1, 0);
    }
    private IEnumerator Smelt()
    {
        for(int i = 0; i < 1000; i++)
        {
            _imageRenderer.material.SetFloat("_Down", i / 1000f);
            yield return new WaitForSeconds(0.04f);
        }
    }
}
