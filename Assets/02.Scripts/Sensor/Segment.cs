using UnityEngine;
using UnityEngine.UI;

public class Segment : MonoBehaviour
{
    [SerializeField]
    Color32[] colorsList;

    Rect imageRect;

    [SerializeField]
    Image segmentOut;

    Texture2D segmentTexture;
    Sprite segmentSprite;
    byte[] outSegment;

    [SerializeField]
    SegmentCollider segmentCollider;

    int cols = 0;
    int rows = 0;

    void Start()
    {
        //Debug.Log("Segment Start()");
        SkeletonController.SkeletonOn += SegInit;
        NuitrackManager.onUserTrackerUpdate += ColorizeUser;

        nuitrack.OutputMode mode = NuitrackManager.DepthSensor.GetOutputMode();

        cols = mode.XRes;
        rows = mode.YRes;

        //Debug.Log(string.Format("{0}, {1}", cols, rows));

        imageRect = new Rect(0, 0, cols, rows);

        segmentTexture = new Texture2D(cols, rows, TextureFormat.ARGB32, false);

        outSegment = new byte[cols * rows * 4];

        segmentOut.type = Image.Type.Simple;
        segmentOut.preserveAspect = false;

        //segmentCollider.CreateColliders(cols, rows);
    }

    private void SegInit()
    {
        NuitrackManager.onUserTrackerUpdate += ColorizeUser;

        nuitrack.OutputMode mode = NuitrackManager.DepthSensor.GetOutputMode();

        cols = mode.XRes;
        rows = mode.YRes;

        Debug.Log(string.Format("{0}, {1}", cols, rows));

        imageRect = new Rect(0, 0, cols, rows);
        
        segmentTexture = new Texture2D(cols, rows, TextureFormat.ARGB32, false);

        outSegment = new byte[cols * rows * 4];

        segmentOut.type = Image.Type.Simple;
        segmentOut.preserveAspect = false;
    }

    void OnDestroy()
    {
        NuitrackManager.onUserTrackerUpdate -= ColorizeUser;
        SkeletonController.SkeletonOn -= SegInit;
    }

    void ColorizeUser(nuitrack.UserFrame frame)
    {
        for (int i = 0; i < (cols * rows); i++)
        {
            Color32 currentColor = colorsList[frame[i]];
            
            int ptr = i * 4;
            outSegment[ptr] = currentColor.a;
            outSegment[ptr + 1] = currentColor.r;
            outSegment[ptr + 2] = currentColor.g;
            outSegment[ptr + 3] = currentColor.b;
        }

        segmentTexture.LoadRawTextureData(outSegment);
        segmentTexture.Apply();

        segmentSprite = Sprite.Create(segmentTexture, imageRect, Vector3.one * 0.5f, 100f, 0, SpriteMeshType.FullRect);

        segmentOut.sprite = segmentSprite;

        //segmentCollider.UpdateFrame(frame);
    }
}