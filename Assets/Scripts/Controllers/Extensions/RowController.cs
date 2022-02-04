using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scripts.Controllers.Extensions
{
    public class RowController : MonoBehaviour
    {
        public TextMeshProUGUI _positionLabel;
        public TextMeshProUGUI _playerLabel;
        public TextMeshProUGUI _levelLabel;
        public TextMeshProUGUI _scoreLabel;

        public void SetLabels(string position, string player, string level, string score)
        {
            _positionLabel.text = position;
            _playerLabel.text = player;
            _levelLabel.text = level;
            _scoreLabel.text = score;
        }
    }
}
