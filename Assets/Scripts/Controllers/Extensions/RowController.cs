using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scripts.Controllers.Extensions
{
    public class RowController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _positionLabel;
        [SerializeField] private TextMeshProUGUI _playerLabel;
        [SerializeField] private TextMeshProUGUI _levelLabel;
        [SerializeField] private TextMeshProUGUI _scoreLabel;

        public void SetLabels(string position, string player, string level, string score)
        {
            _positionLabel.text = position;
            _playerLabel.text = player;
            _levelLabel.text = level;
            _scoreLabel.text = score;
        }
    }
}
