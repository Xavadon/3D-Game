using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OK
{
    public class PlayerCharacteristics : MonoBehaviour
    {
        [SerializeField] private GameObject _characteristicsPanel;
        [SerializeField] private Text _characteristicsText;

        bool _isPanelActive;

        private float _health;
        private float _endurance;
        private float _mana;
        private float _damage;
        private float _defence;
        private float _experience;
        private float _level;
        private float _expToLevelUp = 100;

        public float Health => _health;
        public float Endurance => _endurance;
        public float Mana => _mana;
        public float Damage => _damage;
        public float Defence => _defence;
        public float Experience => _experience;
        public float Level => _level;



        private void Awake()
        {
            UpdateValues();
        }

        private void Update()
        {
            EnablePanel();
        }

        private void UpdateValues()
        {
            _health = PlayerPrefs.GetFloat("health");
            if (_health == 0)
            {
                SetDefaultValues();
                return;
            }
            _endurance = PlayerPrefs.GetFloat("endurance");
            _mana = PlayerPrefs.GetFloat("mana");
            _damage = PlayerPrefs.GetFloat("damage");
            _defence = PlayerPrefs.GetFloat("defence");
            _experience = PlayerPrefs.GetFloat("experience");
            _level = PlayerPrefs.GetFloat("level");
            _expToLevelUp = 100 + 50 * (_level - 1);

            SetUI();
        }


        private void SetDefaultValues()
        {
            PlayerPrefs.SetFloat("health", 100);
            PlayerPrefs.SetFloat("endurance", 100);
            PlayerPrefs.SetFloat("mana", 10);
            PlayerPrefs.SetFloat("damage", 20);
            PlayerPrefs.SetFloat("defence", 5);
            PlayerPrefs.SetFloat("experience", 0);
            PlayerPrefs.SetFloat("level", 1);
            UpdateValues();
        }

        public void AddExp(float value)
        {
            _experience += value;
            if (_experience >= _expToLevelUp)
            {
                _experience -= _expToLevelUp;
                AddCharacteristicValue("level", 1);
                LevelUp();
            }

            PlayerPrefs.SetFloat("experience", _experience);
            UpdateValues();
        }

        private void LevelUp()
        {
            AddCharacteristicValue("health",  20);
            AddCharacteristicValue("mana",  5);
            AddCharacteristicValue("damage", 5);
        }

        public void AddCharacteristicValue(string name, float value)
        {
            var currentValue = PlayerPrefs.GetFloat(name);
            PlayerPrefs.SetFloat(name, currentValue + value);
        }

        private void SetUI()
        {
            _characteristicsText.text =
                _health + "\n"
                + _endurance + "\n"
                + _mana + "\n"
                + _damage + "\n"
                + _defence + "\n\n"
                + _level + "\n"
                + _experience + "/" + _expToLevelUp;
        }

        private void EnablePanel()
        {

            if(Input.GetButtonDown("ShowCharacteristicsPanel"))
            {
                if (_isPanelActive)
                {
                    _characteristicsPanel.SetActive(false);
                    _isPanelActive = false;
                }
                else
                {
                    _characteristicsPanel.SetActive(true);
                    _isPanelActive = true;
                }
            }
        }
    }
}
