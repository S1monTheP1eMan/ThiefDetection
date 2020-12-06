using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    [SerializeField] private float _duration;

    private AudioSource _audioSource;
    private bool _isInside;
    private bool _volumeIsMin;

    private void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _isInside = false;
    }

    private void Update()
    {
        if (_isInside)
        {
            if (_volumeIsMin)
            {
                IncreaseVolume();
            }

            if (!_volumeIsMin)
            {
                DecreaseVolume();
            }
        }
    }

    private void IncreaseVolume()
    {
        _audioSource.volume += Time.deltaTime / _duration;

        if (_audioSource.volume == 1)
        {
            _volumeIsMin = false;
        }
    }

    private void DecreaseVolume()
    {
        _audioSource.volume -= Time.deltaTime / _duration;

        if (_audioSource.volume == 0)
        {
            _volumeIsMin = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            _isInside = true;
            _audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            _isInside = false;
            _audioSource.Pause();
        }
    }
}
