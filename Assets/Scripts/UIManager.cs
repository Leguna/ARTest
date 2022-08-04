using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public Button fireToggleButton;
    public Button blinkingToggleButton;
    public Button animateToggleButton;

    public ObjectController objectController;
    public Screenshot screenshot;

    protected override void Awake()
    {
        base.Awake();
        screenshot = gameObject.AddComponent<Screenshot>();
    }

    public void ToggleFire()
    {
        objectController.ToggleFireParticle();
    }

    public void Blinking()
    {
        objectController.Blinking();
    }

    public void AnimateToggle()
    {
        objectController.ToggleAnimator();
    }

    public void TakeScreenshot()
    {
        screenshot.TakeAShot();
    }
}