using UnityEngine;
using System.Collections;

public class CameraMvr : MonoBehaviour
{
    public Vector3 L;
    public Vector3 R;
    private Vector3 target;
    // Use this for initialization
    public void MoveCamera(bool isLeft)
    {
        StartCoroutine(MoveCameraCoroutine(isLeft));
    }
    IEnumerator MoveCameraCoroutine(bool isLeft)
    {
        target = isLeft ? L : R;
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 5);
            yield return null;
        }
        transform.position = target;
    }
    private void Awake()
    {
        AAPSingleton.cameraMvr = this;
    }
    private void Start()
    {
        Vector3 bucketL = AAPSingleton.catPodL.GetBackgroundPos();
        Vector3 bucketR = AAPSingleton.catPodR.GetBackgroundPos();
        bucketL.z = this.transform.position.z;
        bucketR.z = this.transform.position.z;
        L = bucketL;
        R = bucketR;
    }


}