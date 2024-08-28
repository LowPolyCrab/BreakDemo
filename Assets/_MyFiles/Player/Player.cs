using UnityEngine;

[RequireComponent (typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameplayWidget gameplayWidgetPrefab;
    private GameplayWidget _gameplayWidget;
    private CharacterController _characterController;
    
    private Animator _animator;
    private Vector2 _moveInput;

    [SerializeField] private float _moveSpeed = 3f;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController> ();
        _animator = GetComponent<Animator> ();
        _gameplayWidget = Instantiate(gameplayWidgetPrefab);
        _gameplayWidget.MoveStick.OnInputUpdated += InputUpdated;
    }

    private void InputUpdated(Vector2 inputVal)
    {
        _moveInput = inputVal;
    }
    void Start()
    {
        
    }

    void Update()
    {
        _characterController.Move( new Vector3(_moveInput.x, 0, _moveInput.y) * (_moveSpeed * Time.deltaTime));
    }
}
