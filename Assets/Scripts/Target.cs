using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float _minX;
    [SerializeField] float _maxX;

	private void Start()
	{
		SetNewPos();
	}

	public void SetNewPos()
	{
		transform.position = new Vector3(Random.Range(_minX, _maxX), transform.position.y, 0);
	}
}
