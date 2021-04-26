using Cysharp.Threading.Tasks;
using System.Collections.Generic;

using System;

namespace Adrenak.UGX {
    public static class PopupController {
        const string DEFAULT = "default";

        readonly static Dictionary<string, bool> isShowingMap = new Dictionary<string, bool>();

        /// <summary>
        /// Returns whether any popup is active in a given zone ID
        /// </summary>
        /// <param name="zoneID"></param>
        /// <returns></returns>
        public static bool IsPopupActive(string zoneID = DEFAULT) {
            if (isShowingMap.ContainsKey(zoneID))
                return isShowingMap[zoneID];
            return false;
        }

        /// <summary>
        /// Displays a popup and returns the response as a task. Optionally takes the zone ID too.
        /// </summary>
        public async static UniTask<K> Display<T, K>(Popup<T, K> popup, string zoneID = DEFAULT) where T : PopupState where K : PopupResponse {
            await UniTask.WaitWhile(() => IsPopupActive(zoneID));
            isShowingMap.SetPair(zoneID, true);
            var response = await popup.Display();
            isShowingMap.SetPair(zoneID, false);
            return response;
        }

        /// <summary>
        /// Displays a popup and returns the response as a callback. Optionally takes the zone ID too.
        /// </summary>
        public async static void Display<T, K>(Popup<T, K> popup, Action<K> resultCallback, string zoneID = DEFAULT) where T : PopupState where K : PopupResponse =>
            resultCallback?.Invoke(await Display(popup, zoneID));

        /// <summary>
        /// Takes a popup, it's state and displays it before returning the response as a task. Optionally takes the zone ID too.
        /// </summary>
        public async static UniTask<K> SetStateAndDisplay<T, K>(Popup<T, K> popup, T state, string zoneID = DEFAULT) where T : PopupState where K : PopupResponse {
            await UniTask.WaitWhile(() => IsPopupActive(zoneID));
            isShowingMap.SetPair(zoneID, true);
            var response = await popup.SetStateAndDisplay(state);
            isShowingMap.SetPair(zoneID, false);
            return response;
        }

        /// <summary>
        /// Takes a popup, it's state and displays it before returning the response as a callback. Optionally takes the zone ID too.
        /// </summary>
        public async static void SetStateAndDisplay<T, K>(Popup<T, K> popup, T state, Action<K> resultCallback, string zoneID = DEFAULT) where T : PopupState where K : PopupResponse =>
            resultCallback?.Invoke(await SetStateAndDisplay(popup, state, zoneID));

        /// <summary>
        /// Takes a popup and a method for state change before displaying. Returns the response as a task and takes an optional zone ID
        /// </summary>
        public async static UniTask<K> ModifyStateAndDisplay<T, K>(Popup<T, K> popup, Action<T> modification, string zoneID = DEFAULT) where T : PopupState where K : PopupResponse {
            await UniTask.WaitWhile(() => IsPopupActive(zoneID));
            isShowingMap.SetPair(zoneID, true);
            var response = await popup.ModifyStateAndDisplay(modification);
            isShowingMap.SetPair(zoneID, false);
            return response;
        }

        /// <summary>
        /// Takes a popup and a method for state change before displaying. Returns the response as a callback and takes an optional zone ID
        /// </summary>
        public async static void ModifyStateAndDisplay<T, K>(Popup<T, K> popup, Action<T> modification, Action<K> resultCallback, string zoneID = DEFAULT) where T : PopupState where K : PopupResponse =>
            resultCallback?.Invoke(await ModifyStateAndDisplay(popup, modification, zoneID));
    }
}
