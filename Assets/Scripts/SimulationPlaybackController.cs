using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationPlaybackController : MonoBehaviour
{
    #region Fields

    [SerializeField] private CustomRenderTexture targetRenderTexture;
    [SerializeField] private float playbackSpeed = 1f;

    private bool _isRunning = false;
    private float _timeCounter = 0f;

    #endregion

    #region Properties

    #endregion

    #region BehaviourMethods

    private void Awake()
    {
        targetRenderTexture.initializationMode = CustomRenderTextureUpdateMode.OnDemand;
        targetRenderTexture.updateMode = CustomRenderTextureUpdateMode.OnDemand;

        targetRenderTexture.Initialize();
    }

    public void Update()
    {
        if (_isRunning)
        {
            _timeCounter += Time.deltaTime * playbackSpeed * 60f;

            while (_timeCounter >= 1)
            {
                targetRenderTexture.Update();
                _timeCounter -= 1f;
            }
        }
    }

    private void OnDestroy()
    {
        Stop();
    }

    #endregion

    #region Methods

    public void Run()
    {
        _isRunning = true;
    }

    public void Pause()
    {
        _isRunning = false;
    }

    public void Stop()
    {
        targetRenderTexture.Initialize();
        Pause();
    }

    public void SetSpeed(float speed)
    {
        playbackSpeed = speed;
    }

    #endregion
}