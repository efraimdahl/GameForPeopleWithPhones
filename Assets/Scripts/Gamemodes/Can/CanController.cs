﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanController : MonoBehaviour
{
    /* Editor exposed attributes */
    [Header("Game variables")]
    public float Health = 100f;

    [Space()]
    [Header("Visual")]
    public GameObject RendererObject;
    public Sprite ExplodedSprite;
    public float WiggleDampening = 1f;

    /* Non-serialized public attributes */
    public Action OnCanBrokenEvent;


    private SpriteRenderer canRenderer;

    private void Start()
    {
        // add shake event listener
        CanGameManager.Instance.OnCanShakeEvent += _onShakeEvent;
        InputManager.Instance.OnAccelDataChanged += _onAccelDataChanged;

        // get can renderer
        canRenderer = RendererObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        // remove event listeners
        CanGameManager.Instance.OnCanShakeEvent -= _onShakeEvent;
        InputManager.Instance.OnAccelDataChanged -= _onAccelDataChanged;
    }

    private void _onShakeEvent(float intensity)
    {
        if (Health <= 0) return; // don't do anything if already broken

        // "hurt" can
        Health -= intensity + 1f;
        if(Health <= 0)
        {
            _onCanBroken();
        }
    }

    private void _onAccelDataChanged(AccelData data)
    {
        // wiggle
        RendererObject.transform.localPosition = data.Delta * WiggleDampening;
    }

    private void _onCanBroken()
    {
        // set sprite to open can
        canRenderer.sprite = ExplodedSprite;

        _dispatchOnCanBroken();
    }

    private void _dispatchOnCanBroken()
    {
        if (OnCanBrokenEvent != null)
            OnCanBrokenEvent();
    }
}
