using UnityEngine;

[RequireComponent (typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameplayWidget gameplayWidgetPrefab;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float bodyRotSpeed = 10f;
    [SerializeField]private ViewCamera viewCameraPrefab;
    private GameplayWidget _gameplayWidget;
    private CharacterController _characterController;
    private ViewCamera _viewCamera;
    
    private Animator _animator;
    private Vector2 _moveInput;
    private Vector2 _aimInput;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController> ();
        _animator = GetComponent<Animator> ();
        _gameplayWidget = Instantiate(gameplayWidgetPrefab);
        _gameplayWidget.MoveStick.OnInputUpdated += MoveInputUpdated;
        _gameplayWidget.AimStick.OnInputUpdated += AimInputUpdated;
        _viewCamera = Instantiate(viewCameraPrefab);
        _viewCamera.SetFollowParent(transform);
    }

    private void MoveInputUpdated(Vector2 inputVal)
    {
        _moveInput = inputVal;
    }

    private void AimInputUpdated(Vector2 inputVal)
    {
        _aimInput = inputVal;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 moveDir = _viewCamera.InputToWorldDir(_moveInput);
        _characterController.Move(moveDir * (_moveSpeed * Time.deltaTime));

        Vector3 aimDir = _viewCamera.InputToWorldDir(_aimInput);
        if (aimDir == Vector3.zero)
        {
            aimDir = moveDir;
        }

        _viewCamera.AddYawInput(_moveInput.x);
        if (aimDir != Vector3.zero)
        {
            Quaternion goalRot = Quaternion.LookRotation(aimDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, goalRot, Time.deltaTime * (_moveSpeed * bodyRotSpeed));
        }
    }
}
