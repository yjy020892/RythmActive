using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nuitrack;

public class EOSkeletonController : MonoBehaviour
{
    public delegate void SkeletonOnEvent();
    public static event SkeletonOnEvent SkeletonOn;

    [Range(0, 6)]
    public int skeletonCount = 6;         //Max number of skeletons tracked by Nuitrack
    [SerializeField] EOSkeletonAvatar skeletonAvatar;

    List<EOSkeletonAvatar> avatars = new List<EOSkeletonAvatar>();

    void OnEnable()
    {
        if (SkeletonOn != null)
        {
            SkeletonOn();
        }
    }

    void Start()
    {
        for (int i = 0; i < skeletonCount; i++)
        {
            GameObject newAvatar = Instantiate(skeletonAvatar.gameObject, transform);
            EOSkeletonAvatar simpleSkeleton = newAvatar.GetComponent<EOSkeletonAvatar>();
            simpleSkeleton.autoProcessing = false;
            avatars.Add(simpleSkeleton);

            RectTransform rectTransform = newAvatar.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;

            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }

        NuitrackManager.SkeletonTracker.SetNumActiveUsers(skeletonCount);

        NuitrackManager.onSkeletonTrackerUpdate += OnSkeletonUpdate;
    }

    void OnSkeletonUpdate(SkeletonData skeletonData)
    {
        for (int i = 0; i < avatars.Count; i++)
        {
            if (i < skeletonData.Skeletons.Length)
            {
                avatars[i].gameObject.SetActive(true);
                avatars[i].ProcessSkeleton(skeletonData.Skeletons[i]);
            }
            else
            {
                avatars[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        NuitrackManager.onSkeletonTrackerUpdate -= OnSkeletonUpdate;
    }
}
