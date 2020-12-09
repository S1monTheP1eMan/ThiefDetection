using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    [SerializeField] private float _duration;

    private AudioSource _audioSource;
    private bool _isInside;
    private bool _volumeIsMin;
    private bool _volumeIsMax;
    private float _runningTime;
    private IEnumerator _increaseVolume;
    private IEnumerator _decreaseVolume;

    private void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _isInside = false;
        _volumeIsMin = true;
        _volumeIsMax = false;
    }

    private void Update()
    {
        if (_isInside)
        {
            if (_volumeIsMin)
            {
                _volumeIsMin = false;

                StartIncreaseVolume();
            }

            if (_volumeIsMax)
            {
                _volumeIsMax = false;

                StartDecreaseVolume();
            }
        }
    }

    private void StartIncreaseVolume()
    {
        if (_decreaseVolume != null)
        {
            StopCoroutine(DecreaseVolume());
        }

        _increaseVolume = IncreaseVolume();
        StartCoroutine(_increaseVolume);
    }

    private void StartDecreaseVolume()
    {
        if (_increaseVolume != null)
        {
            StopCoroutine(IncreaseVolume());
        }

        _decreaseVolume = DecreaseVolume();
        StartCoroutine(_decreaseVolume);
    }

    private IEnumerator IncreaseVolume()
    {
        var waitUntil = new WaitUntil(() => _isInside == true);
        var volume = _audioSource.volume;

        for (_runningTime = 0; _runningTime <= _duration; _runningTime += Time.deltaTime)
        {
            volume = 0f + (1f / _duration * _runningTime);
            _audioSource.volume = volume;

            if (_audioSource.volume >= 0.99f)
            {
                _volumeIsMax = true;
                yield break;
            }
            else if (_isInside == false)
            {
                yield return waitUntil;
            }

            yield return null;
        }
    }

    private IEnumerator DecreaseVolume()
    {
        var waitUntil = new WaitUntil(() => _isInside == true);
        var volume = _audioSource.volume;

        for (_runningTime = 0; _runningTime <= _duration; _runningTime += Time.deltaTime)
        {
            volume = 1f - (1f / _duration * _runningTime);
            _audioSource.volume = volume;

            if (_audioSource.volume <= 0.01f)
            {
                _volumeIsMin = true;
                yield break;
            }
            else if (_isInside == false)
            {
                yield return waitUntil;
            }

            yield return null;
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
