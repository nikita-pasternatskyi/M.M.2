using Animancer;
using MykroFramework.Runtime.Objects.SOStateMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOAnimationMachine
{
    [System.Serializable]
    public class AnimationAction : StateAction
    {
        enum EntryType
        {
            StateContainer,
            AnimationClip
        }

        [System.Serializable]
        struct Entry
        {
            public EntryType EntryType;
            [HideIf("@this.EntryType == AnimationAction.EntryType.AnimationClip")] public StateContainer Container;
            [HideIf("@this.EntryType == AnimationAction.EntryType.StateContainer")] public AnimationClip AnimationClip;
            [SerializeReference] public ITransition ClipToPlay;
        }

        [SerializeReference] private ITransition _defaultClipToPlay;
        [SerializeField, ValidateInput("ValidateClips", "$_duplicateError", InfoMessageType.Error)] private Entry[] _clipVariantsWithTransitions;
        [SerializeField] private bool _resetTime;

        private Dictionary<StateContainer, ITransition> _variants;
        private Dictionary<AnimationClip, ITransition> _animationClipVariants;
        [SerializeField, HideInInspector] private string _duplicateError;

        private bool ValidateClips()
        {
            bool checkForErrors<T>(Func<Entry, T> func)
            {
                var hashSet = new HashSet<T>(_clipVariantsWithTransitions.Length);
                foreach (var item in _clipVariantsWithTransitions)
                {
                    var i = func(item);
                    if (i == null)
                        continue;
                    if (hashSet.Contains(i))
                    {
                        _duplicateError = item.Container.name;
                        return false;
                    }
                    hashSet.Add(i);
                }
                hashSet.Clear();
                return true;
            }

            if (!checkForErrors<StateContainer>((e) => e.Container))
                return false;
            if (!checkForErrors<AnimationClip>((e) => e.AnimationClip))
                return false;

            return true;
        }

        protected SOAnimator SOAnimator { get; private set; }

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            SOAnimator = stateMachine.GetComponent<SOAnimator>();
            if (ValidateClips() == false)
            {
                UnityEngine.Debug.LogError($"{this} has a duplicate with container of type {_duplicateError}");
                return;
            }

            _variants = new Dictionary<StateContainer, ITransition>();
            _animationClipVariants = new Dictionary<AnimationClip, ITransition>();
            foreach (var item in _clipVariantsWithTransitions)
            {
                switch (item.EntryType)
                {
                    case EntryType.StateContainer:
                        _variants.Add(item.Container, item.ClipToPlay);
                        break;
                    case EntryType.AnimationClip:
                        _animationClipVariants.Add(item.AnimationClip, item.ClipToPlay);
                        break;
                }
            }
        }

        public override void Enter()
        {
            if (SOAnimator.PreviousAnimationState == null || SOAnimator.PreviousAnimationClip == null)
            {
                PlayClip(_defaultClipToPlay);
                return;
            }

            if (_variants.TryGetValue(SOAnimator.PreviousAnimationState, out ITransition newClip) || 
                _animationClipVariants.TryGetValue(SOAnimator.PreviousAnimationClip, out newClip))
            {
                PlayClip(newClip);
                return;
            }

            PlayClip(_defaultClipToPlay);
        }

        private AnimancerState PlayClip(ITransition clip)
        {
            var state = SOAnimator.PlayClip(clip);
            if (_resetTime)
                state.Time = 0;
            return state;
        }
    }
}
