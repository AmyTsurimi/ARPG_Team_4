﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExplodeBox : MonoBehaviour
{

    public float minForce;
    public float maxForce;
    public float radius;
    public float fadeSpeed;
    private List<Transform> _explodeObject = new List<Transform>();

    private void Start() {
        Explode();
    }

    public void Explode() {
        foreach (Transform t in transform) {
            _explodeObject.Add(t);
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null) rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
        }
        
    }

    public void MakeTransparent() {
        foreach (Transform obj in _explodeObject) {
            Color color = obj.GetComponent<Renderer>().material.color;
            color.a -= Time.deltaTime * fadeSpeed;
            obj.GetComponent<Renderer>().material.color = color;
            if (color.a <= 0) Destroy(gameObject);
        }
    }

    private void Update() {
        MakeTransparent();
    }
}