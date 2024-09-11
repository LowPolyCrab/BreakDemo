using UnityEngine;

[RequireComponent (typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SocketManager))]
[RequireComponent(typeof(InventoryComponent))]
[RequireComponent(typeof(HealthComponent))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameplayWidget gameplayWidgetPrefab;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float bodyRotSpeed = 10f;
    [SerializeField]private ViewCamera viewCameraPrefab;
    [SerializeField] private float animTurnLerpScale = 5.0f;

    private GameplayWidget _gameplayWidget;
    private CharacterController _characterController;
    private InventoryComponent _inventoryComponent;
    private ViewCamera _viewCamera;
    private HealthComponent _healthComponent;
    
    private Animator _animator;
    private float _animTurnSpeed;
    private Vector2 _moveInput;
    private Vector2 _aimInput;

    private static readonly int animFwdId = Animator.StringToHash("Forward");
    private static readonly int animRightId = Animator.StringToHash("Right");
    private static readonly int animTurnId = Animator.StringToHash("TurnAmt");
    private static readonly int switchWeaponId = Animator.StringToHash("SwitchWeapon");
    private static readonly int FireId = Animator.StringToHash("Firing");


    private void Awake()
    {
        _characterController = GetComponent<CharacterController> ();
        _animator = GetComponent<Animator> ();
        _inventoryComponent = GetComponent<InventoryComponent> ();
        _healthComponent = GetComponent<HealthComponent> ();
        _gameplayWidget = Instantiate(gameplayWidgetPrefab);
        _gameplayWidget.MoveStick.OnInputUpdated += MoveInputUpdated;
        _gameplayWidget.AimStick.OnInputUpdated += AimInputUpdated;
        _gameplayWidget.AimStick.OnInputClicked += SwitchWeapon;
        _viewCamera = Instantiate(viewCameraPrefab);
        _viewCamera.SetFollowParent(transform);
    }

    private void SwitchWeapon()
    {
        _animator.SetTrigger(switchWeaponId);
    }

    public void WeaponSwitchPoint()
    {
        _inventoryComponent.EquipNextWeapon();
    }

    private void MoveInputUpdated(Vector2 inputVal)
    {
        _moveInput = inputVal;
    }

    private void AimInputUpdated(Vector2 inputVal)
    {
        _aimInput = inputVal;
        _animator.SetBool(FireId, _aimInput != Vector2.zero);
       
    }
    public void AttackPoint()
    {
        _inventoryComponent.FireCurrentActiveWeapon();
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
            _viewCamera.AddYawInput(_moveInput.x);
        }

        float angleDelta = 0f;
        _viewCamera.AddYawInput(_moveInput.x);
        if (aimDir != Vector3.zero)
        {
            Vector3 prevDir = transform.forward;
            Quaternion goalRot = Quaternion.LookRotation(aimDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, goalRot, Time.deltaTime * (_moveSpeed * bodyRotSpeed));
            angleDelta = Vector3.SignedAngle(transform.forward,prevDir,Vector3.up);
        }

        _animTurnSpeed = Mathf.Lerp(_animTurnSpeed, angleDelta/Time.deltaTime, Time.deltaTime * animTurnLerpScale);
        _animator.SetFloat(animTurnId, angleDelta / Time.deltaTime);

        float animFwdAmt = Vector3.Dot(moveDir, transform.forward);
        float animRightAmt = Vector3.Dot(moveDir, transform.right);

        _animator.SetFloat(animFwdId, animFwdAmt);
        _animator.SetFloat(animRightId, animRightAmt);
    }
}
