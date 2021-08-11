using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JumpJam
{
    public class RandomBotAppearance : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _bots;
        [SerializeField] private Material[] _materials;

        [SerializeField] private SpriteRenderer[] _icons;
        [SerializeField] private Sprite[] _images;

        [SerializeField] private Image[] _iconsUI;
        [SerializeField] private Sprite[] _imagesUI;

        [SerializeField] private TMP_Text[] _labels;

        private string[] _temporaryLabels;
        private int[] _indexes;
        private int _indexesNumber = 6;

        private void Awake()
        {
            _indexes = new int[_indexesNumber];

            for (int i = 0; i < _indexes.Length; i++)
            {
                _indexes[i] = i;
            }

            SetRandomIndexes();

            ApplyMaterials();
            ApplyIcons();
            ApplyImagesUI();
            ApplyLabelsUI();
        }

        private void SetRandomIndexes()
        {
            for (int i = 0; i < _indexes.Length; i++)
            {
                int randomIndex = Random.Range(0, _indexes.Length - 1);

                int temporary = _indexes[i];
                _indexes[i] = _indexes[randomIndex];
                _indexes[randomIndex] = temporary;
            }
        }

        private void ApplyMaterials()
        {
            for (int i = 0; i < _bots.Length; i++)
            {
                _bots[i].material = _materials[_indexes[i]];
            }
        }

        private void ApplyIcons()
        {
            for (int i = 0; i < _icons.Length; i++)
            {
                _icons[i].sprite = _images[_indexes[i]];
            }
        }

        private void ApplyImagesUI()
        {
            for (int i = 0; i < _iconsUI.Length; i++)
            {
                _iconsUI[i].sprite = _imagesUI[_indexes[i]];
            }
        }

        private void ApplyLabelsUI()
        {
            _temporaryLabels = new string[_labels.Length];

            for (int i = 0; i < _labels.Length; i++)
            {
                _temporaryLabels[i] = _labels[i].text;
            }

            for (int i = 0; i < _labels.Length; i++)
            {
                _labels[i].text = _temporaryLabels[_indexes[i]];
            }
        }
    }
}
