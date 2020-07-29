using System.Collections.Generic;
using UnityEngine;
using Adrenak.Unex;
using NaughtyAttributes;
using System;
using System.Linq.Expressions;

namespace Adrenak.UPF {
    [Serializable]
    public class NavigationStack {
        [ReadOnly] [SerializeField] Page current = null;
        public Page Current => current;
        [ReadOnly] [SerializeField] int currentIndex = -1;

        [ReorderableList] [ReadOnly] [SerializeField] List<Page> history = new List<Page>();
        public List<Page> History => history;

        bool IsMidHistory => currentIndex != history.Count - 1;
        bool IsLast(Page view) => history.Last() == view;

        public void Push(Page view) {
            if (history.Count == 0) {
                history.Add(view);
                SetAsCurrent(view);
                return;
            }

            if (history.Last() == view)
                return;

            if (history.Count > 1 && history.FromLast(1) == view) {
                history.RemoveAt(history.Count - 1);
                SetAsCurrent(history.Last());
                return;
            }

            history.Add(view);
            SetAsCurrent(view);
        }

        public bool Pop() {
            if (history.Count > 1) {
                history.RemoveAt(history.Count - 1);
                SetAsCurrent(history.Last());
            }
            return true;
            //currentIndex--;
            //return ValidateMove();
        }

        public bool Back() {
            currentIndex--;
            return ValidateMove();
        }

        public bool Forward() {
            currentIndex++;
            return ValidateMove();
        }

        void SetAsCurrent(Page view) {
            view.OpenPage();
            current?.ClosePage();

            currentIndex = history.IndexOf(view);
            current = history[currentIndex];
        }

        bool ValidateMove() {
            var clamped = Mathf.Clamp(currentIndex, 0, history.Count - 1);
            if (currentIndex != clamped) {
                currentIndex = clamped;
                return false;
            }

            SetAsCurrent(history[currentIndex]);
            return true;
        }
    }
}