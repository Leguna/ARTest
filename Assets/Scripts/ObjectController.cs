using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class ObjectController : MonoBehaviour
{
    private TouchInput _touchInput;
    public float swipeVerticalSpeedModifier = 0.01f;
    public float swipeHorizontalSpeedModifier = 0.01f;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator parentAnimator;
    public GameObject fireParticle;
    private static readonly int Blink = Animator.StringToHash("Blink");
    public ARTrackedImage arTrackedImage;
    public AudioSource audioSource;

    public bool isSecondFingerTouch;
    private Coroutine _zoomCoroutine;

    public Vector3 zoomInMultiplier = new Vector3(1.2f, 1.2f, 1.2f);
    public Vector3 zoomOutMultiplier = new Vector3(0.8f, 0.8f, 0.8f);
    public float zoomSpeed = 1f;

    private void Awake()
    {
        _touchInput = new TouchInput();
    }

    private void OnEnable()
    {
        _touchInput.Enable();
    }

    private void OnDisable()
    {
        _touchInput.Disable();
    }

    private void Start()
    {
        arTrackedImage = GetComponentInParent<ARTrackedImage>();

        UIManager.Instance.objectController = this;
        ARCustomManager.Instance.arTrackedImage = arTrackedImage;

        _touchInput.Touch.SwipeHorizontal.performed += SwipeHorizontal;
        _touchInput.Touch.SwipeVertical.performed += SwipeVertical;
        _touchInput.Touch.SecondaryTouch.started += _ => SecondaryTouchStart();
        _touchInput.Touch.SecondaryTouch.canceled += _ => SecondaryTouchEnd();
    }

    private void SecondaryTouchStart()
    {
        isSecondFingerTouch = true;
        _zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void SecondaryTouchEnd()
    {
        isSecondFingerTouch = false;
        StopCoroutine(_zoomCoroutine);
    }

    private IEnumerator ZoomDetection()
    {
        float previousDistance = 0f;
        while (true)
        {
            var distance = Vector2.Distance(_touchInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>(),
                _touchInput.Touch.SecondaryTouchPosition.ReadValue<Vector2>());

            if (distance > previousDistance)
            {
                var localScale = transform.localScale;
                localScale = Vector3.Slerp(localScale, localScale * 2f, Time.deltaTime * zoomSpeed);
                transform.localScale = localScale;
            }
            else if (distance < previousDistance)
            {
                var localScale = transform.localScale;
                localScale = Vector3.Slerp(localScale, localScale * 0f, Time.deltaTime * zoomSpeed);
                transform.localScale = localScale;
            }

            previousDistance = distance;
            yield return null;
        }
    }

    private void SwipeHorizontal(InputAction.CallbackContext ctx)
    {
        print(isSecondFingerTouch);
        if (isSecondFingerTouch) return;

        var value = ctx.ReadValue<float>();

        var rotation = transform.rotation;
        var rotationY = Quaternion.Euler(0.0f, -value * swipeHorizontalSpeedModifier, 0f);
        transform.rotation = rotationY * rotation;
    }

    private void SwipeVertical(InputAction.CallbackContext ctx)
    {
        print(isSecondFingerTouch);
        if (isSecondFingerTouch) return;

        var value = ctx.ReadValue<float>();

        var rotation = transform.rotation;
        var rotationX = Quaternion.Euler(value * swipeVerticalSpeedModifier, 0f, 0f);
        transform.rotation = rotationX * rotation;
    }


    public void ToggleAnimator()
    {
        animator.enabled = !animator.enabled;
    }

    public void Blinking()
    {
        parentAnimator.SetTrigger(Blink);
    }

    public void ToggleFireParticle()
    {
        fireParticle.SetActive(!fireParticle.activeSelf);
    }
}