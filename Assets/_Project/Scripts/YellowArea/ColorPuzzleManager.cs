using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleManager : MonoBehaviour
{
    [Header("Chest to Unlock")]
    [SerializeField] private ChestController _chest;

    [Header("Sequence Settings")]
    [SerializeField] private List<string> _correctSequence = new List<string> { "Yellow", "Yellow", "Yellow", "Blue", "Blue", "Red", "Red", "Red", "Red" };

    [SerializeField] private List<string> _playerSequence = new List<string>();

    public void PressColor(string color)
    {
        _playerSequence.Add(color);

        // Se la sequenza è ancora in corso
        if (_playerSequence.Count <= _correctSequence.Count)
        {
            for (int i = 0; i < _playerSequence.Count; i++)
            {
                if (_playerSequence[i] != _correctSequence[i])
                {
                    ResetSequence();
                    AudioManager.Instance.Play("WrongCombination"); 
                    return;
                }
            }

            // Se ha completato correttamente tutta la sequenza
            if (_playerSequence.Count == _correctSequence.Count)
            {
                AudioManager.Instance.Play("CorrectCombination");
                _chest?.Unlock();
            }
        }
    }

    private void ResetSequence()
    {
        _playerSequence.Clear();
    }
}
