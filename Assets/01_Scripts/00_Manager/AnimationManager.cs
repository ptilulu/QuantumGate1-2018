using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    private static AnimationManager _instance;

    public static AnimationManager Instance
    {
        get
        {
            if (!_instance)
                _instance = new GameObject("AnimationManager").AddComponent<AnimationManager>();

            return _instance;
        }
    }

    private static IEnumerator _Move(GameObject go, Vector3 newPosition, float timeToMove, System.Action callback)
    {
        Vector3 currentPos = go.transform.localPosition;

        for (float elapsedTime = 0f; elapsedTime < timeToMove; elapsedTime += Time.deltaTime)
        {
            go.transform.localPosition = Vector3.Lerp(currentPos, newPosition, elapsedTime / timeToMove);
            yield return new WaitForEndOfFrame();
        }

        go.transform.localPosition = newPosition;

        callback?.Invoke();
    }
    public static void Move(GameObject go, Vector3 newPosition, float timeToMove)
    {
        Instance.StartCoroutine(_Move(go, newPosition, timeToMove, null));
    }
    public static void Move(GameObject go, Vector3 newPosition, float timeToMove, System.Action callback)
    {
        Instance.StartCoroutine(_Move(go, newPosition, timeToMove, callback));
    }

    private static IEnumerator _Fade(GameObject go, float targetAlpha, float timeToFade, System.Action callback)
    {
        //Debug.Log(go.GetComponentsInChildren<MeshRenderer>().Length);
        for (float elapsedTime = 0f; elapsedTime < timeToFade; elapsedTime += Time.deltaTime)
        {
            foreach (MeshRenderer mr in go.GetComponentsInChildren<Renderer>())
            {
                Color c = mr.material.color;
                float newAlpha = Mathf.Lerp(c.a, targetAlpha, elapsedTime / timeToFade);
                mr.material.color = new Color(c.r, c.g, c.b, newAlpha);
            }
            yield return new WaitForEndOfFrame();
        }

        foreach (MeshRenderer mr in go.GetComponentsInChildren<Renderer>())
        {
            Color c = mr.material.color;
            mr.material.color = new Color(c.r, c.g, c.b, targetAlpha);
        }

        callback?.Invoke();
    }
    public static void Fade(GameObject go, float alpha, float timeToFade)
    {
        Instance.StartCoroutine(_Fade(go, alpha, timeToFade, null));
    }
    public static void Fade(GameObject go, float alpha, float timeToFade, System.Action callback)
    {
        Instance.StartCoroutine(_Fade(go, alpha, timeToFade, callback));
    }
}
