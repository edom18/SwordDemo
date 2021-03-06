﻿using UnityEngine;
using System.Collections;

public class TargetSelector : MonoBehaviour
{
    [SerializeField]
    DaggerController _daggerController;

	// Use this for initialization
	void Start ()
    {
        //_daggerController = transform.parent.GetComponentInChildren<DaggerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_daggerController == null)
        {
            return;
        }

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);

        var radius = 1f;
        int layer = 1 << 8;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, float.MaxValue, layer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                _daggerController.SetTarget(hit.collider.gameObject);
            }
            else
            {
                _daggerController.SetTarget(null);
            }
        }
        else
        {
            _daggerController.SetTarget(null);
        }
	}
}
