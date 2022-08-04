using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCustomManager : SingletonMonoBehaviour<ARCustomManager>
{
    public ObjectController objectController;
    [SerializeField] private ARTrackedImageManager arTrackedImageManager;
    [SerializeField] public ARTrackedImage arTrackedImage;

    private void UpdateVisibilityObject(ARTrackedImagesChangedEventArgs args)
    {
        print(args);
        objectController.gameObject.SetActive(arTrackedImage.trackingState != TrackingState.None);
        objectController.audioSource.Play();
    }

    private void Start()
    {
        arTrackedImageManager.trackedImagesChanged += UpdateVisibilityObject;
    }
}