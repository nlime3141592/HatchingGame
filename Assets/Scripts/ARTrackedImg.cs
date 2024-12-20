using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTrackedImg : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject bugCylinder;

    void Awake()
    {
        
    }
    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage image in args.added)
        {
            UpdateImage(image);
        }

        foreach (ARTrackedImage image in args.updated)
        {
            UpdateImage(image);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector2 imageSize = trackedImage.size;

        bugCylinder.transform.position = trackedImage.transform.position;
        bugCylinder.transform.rotation = trackedImage.transform.rotation;
        // UIManager.s_ui_Debug.GetComponentInChildren<UnityEngine.UI.Text>().text = imageSize.ToString();
        bugCylinder.transform.localScale = new Vector3(imageSize.x, imageSize.x, imageSize.y);
        bugCylinder.SetActive(true);
    }
}
