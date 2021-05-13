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
    [SerializeField] private Light _leftLight;
    [SerializeField] private Light _rightLight;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out JumpChecker jumper) && !_car.Bot)
        {
            Time.timeScale = 0.4f;
            StartCoroutine(_uiPanel.ChangeUi());
            _car.Jumped = true;
        }

        if (other.TryGetComponent(out JumpChecker jumper1) && _car.Bot)
        {
            _car.BotBoost.SetActive(true);
            _car.BoostValue = 2;
            _car.JumpNitro();
        }

        if (other.TryGetComponent(out Finish finish))
        {            
            _car.BotBoost.SetActive(false);
            _car.Finished = true;
            _car.Brake(30000);
            _camera.StopRotate();
            //_result.SetActive(true);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            _camera.StopRotate();
        }
    }
}
