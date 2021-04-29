using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] private Drive _car;
    [SerializeField] private Effects _effects;
    [SerializeField] private TrackingCamera _camera;
    [SerializeField] private GameObject _result;
    [SerializeField] private UiPanel _uiPanel;
    [SerializeField] private ParticleSystem _boost;
    [SerializeField] private ParticleSystem _boost2;
    [SerializeField] private Light _leftLight;
    [SerializeField] private Light _rightLight;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out JumpChecker jumper))
        {
            _uiPanel.ChangeUi();
            _car.Jumped = true;
            Time.timeScale = 1;
        }

        if (other.TryGetComponent(out Finish finish))
        {
            _car.Finished = true;
            _car.Brake(3000);
            _camera.StopRotate();
            _result.SetActive(true);
            _leftLight.intensity = 2;
            _rightLight.intensity = 2;
        }

        if (other.TryGetComponent(out CarObstacle car))
        {
            _car.CarSmashed++;
        }

        if (other.TryGetComponent(out CarObstacle car2) && _car.Jumped && _car.CarSpeed < 0.5f)
        {
            _result.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Finish finish))
        {
            _boost.startLifetime = Mathf.Lerp(_boost.startLifetime, 0, 0.001f);
            _boost2.startLifetime = Mathf.Lerp(_boost2.startLifetime, 0, 0.001f);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            _camera.StopRotate();
        }
    }
}
