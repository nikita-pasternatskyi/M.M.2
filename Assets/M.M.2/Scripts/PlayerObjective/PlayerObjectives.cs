using System;
using System.Collections.Generic;
using UnityEngine;

namespace MM2
{
    public class PlayerObjectives : MonoBehaviour
    {
        [SerializeField] private GameObject _objectivesUIRoot;
        [SerializeField] private PlayerObjectiveView _objectiveViewPrefab;
        private Dictionary<PlayerObjective, PlayerObjectiveView> _objectives = new Dictionary<PlayerObjective, PlayerObjectiveView>();

        public void RegisterObjective(PlayerObjective objective)
        {
            var view = Instantiate(_objectiveViewPrefab, _objectivesUIRoot.transform);
            view.GetComponent<PlayerObjectiveView>().Init(objective);
            _objectives.Add(objective, view);
            objective.Completed += Objective_Completed;
        }

        private void Objective_Completed(PlayerObjective obj)
        {
            _objectives[obj].Complete();
            obj.Completed -= Objective_Completed;
            _objectives.Remove(obj);
        }
    }
}
