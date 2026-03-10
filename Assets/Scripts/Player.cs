using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    private bool isInit = false;
    private SpriteRenderer sr;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (!isInit)
        {
            sr = GetComponent<SpriteRenderer>();

            sr.enabled = false;
            isInit = true;
        }
        
    }

    private void Start()
    {
        RunManager.Instance.OnRunStart += InstanceOnOnRunStart;
        RunManager.Instance.OnRunEnd += InstanceOnOnRunEnd;
        
    }

    private void OnDisable()
    {
        RunManager.Instance.OnRunStart -= InstanceOnOnRunStart;
        RunManager.Instance.OnRunEnd -= InstanceOnOnRunEnd;
    }

    private void InstanceOnOnRunEnd(float obj)
    {
        sr.enabled = false;
    }

    private void InstanceOnOnRunStart()
    {
        gameObject.transform.position = new Vector3(0f, -4f, 0f);
        sr.enabled = true;
    }
}
