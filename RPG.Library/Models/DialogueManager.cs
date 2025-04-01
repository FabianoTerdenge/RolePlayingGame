using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Library.Models
{
    public class DialogueState
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public List<DialogueOption> Options { get; set; } = new();
        public Action<Character> OnEnter { get; set; }
        public Action OnExit { get; set; }
    }

    public class DialogueOption
    {
        public string Text { get; set; }
        public string NextStateId { get; set; }
        public Func<Character, bool> IsAvailable { get; set; } = (Character player) => true;
        public Action OnSelect { get; set; }
    }
    public class DialogueManager
    {
        private Dictionary<string, DialogueState> _states;
        private DialogueState _currentState; // Private field

        // Public property for read-only access
        public DialogueState CurrentState => _currentState;
        private Stack<DialogueState> _stateHistory = new();
        private Character _player;

        public DialogueManager(Dictionary<string, DialogueState> states, string initialStateId, Character player)
        {
            _states = states;
            TransitionTo(initialStateId);
            _player = player;
        }

        public int DisplayCurrentState()
        {
            Console.Clear();
            Console.WriteLine(_currentState.Text);
            Console.WriteLine();

            var availableOptions = _currentState.Options
                .Where(o => o.IsAvailable(_player))
                .ToList();

            for (int i = 0; i < availableOptions.Count; i++)
            {
                Console.WriteLine(availableOptions.Count > 1 ? $"{i + 1}. {availableOptions[i].Text}"
                : availableOptions[i].Text);
            }
            return availableOptions.Count;
        }

        public void SelectOption(int index)
        {
            var availableOptions = _currentState.Options
                .Where(o => o.IsAvailable(_player))
                .ToList();

            if (index < 0 || index >= availableOptions.Count)
                return;

            var selectedOption = availableOptions[index];
            selectedOption.OnSelect?.Invoke();
            TransitionTo(selectedOption.NextStateId);
        }

        public void GoBack()
        {
            if (_stateHistory.Count > 0)
            {
                TransitionTo(_stateHistory.Pop().Id, false);
            }
        }

        private void TransitionTo(string stateId, bool rememberCurrent = true)
        {
            if (!_states.TryGetValue(stateId, out var newState))
                return;

            if (_currentState != null && rememberCurrent)
            {
                _stateHistory.Push(_currentState);
                _currentState.OnExit?.Invoke();
            }

            _currentState = newState;
            _currentState.OnEnter?.Invoke(_player);
        }
    }
}
