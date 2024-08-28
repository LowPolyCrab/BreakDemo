using UnityEngine;

[RequireComponent (typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameplayWidget gameplayWidgetPrefab;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField]private ViewCamera viewCameraPrefab;
    private GameplayWidget _gameplayWidget;
    private CharacterController _characterController;
    private ViewCamera _viewCamera;
    
    private Animator _animator;
    private Vector2 _moveInput;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController> ();
        _animator = GetComponent<Animator> ();
        _gameplayWidget = Instantiate(gameplayWidgetPrefab);
        _gameplayWidget.MoveStick.OnInputUpdated += InputUpdated;
        _viewCamera = Instantiate(viewCameraPrefab);
        _viewCamera.SetFollowParent(transform);
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
