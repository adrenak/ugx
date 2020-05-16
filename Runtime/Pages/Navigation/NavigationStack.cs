using System.Collections.Generic;
using UnityEngine;
using Adrenak.Unex;
using NaughtyAttributes;
using System;

namespace Adrenak.UPF {
    [Serializable]
    public class NavigationStack {
        [ReadOnly] [SerializeField] PageView current = null;
        public PageView Current => current;
        [ReadOnly] [SerializeField] int currentIndex = -1;

        [ReorderableList] [ReadOnly] [SerializeField] List<PageView> history = new List<PageView>();
        public List<PageView> History => history;

        bool IsMidHistory => currentIndex != history.Count - 1;
        bool IsLast(PageView view) => history.Last() == view;

        public void Push(PageView view) {
            if (history.Count == 0) {
                history.Add(view);
                SetAsCurrent(view);
                return;
            }

            // If the current pointer was in the middle
            // and we try to open the last page again, we
            // simply set the last page to current
            if (IsLast(view)) {
                if (IsMidHistory)
                    SetAsCurrent(history.Last());
            }
            else {
                // If we're not repushing the last view and we're 
                // mid history, we "slice" the history and add the 
                // view to that, making it the last element of history
                // and then we set it as current
                if (IsMidHistory) {
                    history = history.GetRange(0, currentIndex + 1);
                    history.Add(view);
                    SetAsCurrent(view);
                }
                // If we're not repushing the last view and we're 
                // at the last view, we simply add it and make it current
                else {
                    history.Add(view);
                    SetAsCurrent(view);
                }
            }
        }

        public bool Pop() {
            currentIndex--;
            return ValidateMove();
        }

        public bool Back() {
            currentIndex--;
            return ValidateMove();
        }

        public bool Forward() {
            currentIndex++;
            return ValidateMove();
        }

        void SetAsCurrent(PageView view) {
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