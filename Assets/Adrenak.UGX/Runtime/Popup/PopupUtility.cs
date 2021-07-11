using Cysharp.Threading.Tasks;
using System.Collections.Generic;

using System;

namespace Adrenak.UGX {
    public static class PopupUtility {
        /// <summary>
        /// The default ZoneID of popups
        /// </summary>
        public const string DEFAULT = "default";

        /// <summary>
        /// Maps zone ID to availability
        /// </summary>
        readonly static Dictionary<string, bool> zoneStateMap = new Dictionary<string, bool>();

        /// <summary>
        /// Returns whether any popup is active in a given zone ID
        /// </summary>
        /// <param name="zoneID"></param>
        /// <returns></returns>
        public static bool IsZoneFree(string zoneID = DEFAULT) {
            if (zoneStateMap.ContainsKey(zoneID))
                return zoneStateMap[zoneID];
            return false;
        }

        /// <summary>
        /// Displays a popup and returns the response as a task. Optionally takes the zone ID too.
        /// </summary>
        public async static UniTask<PopupResponse<K>> Show<T, K>(Popup<T, K> popup, string zoneID = DEFAULT) where T : ViewModel {
            await UniTask.WaitWhile(() => IsZoneFree(zoneID));
            zoneStateMap.SetPair(zoneID, true);
            var response = await popup.Show();
            zoneStateMap.SetPair(zoneID, false);
            return response;
        }

        /// <summary>
        /// Displays a popup and returns the response as a callback. Optionally takes the zone ID too.
        /// </summary>
        public async static void Show<T, K>(Popup<T, K> popup, Action<K> resultCallback, string zoneID = DEFAULT) where T : ViewModel =>
            resultCallback?.Invoke((await Show(popup, zoneID)).Data);

        /// <summary>
        /// Takes a popup, it's state and displays it before returning the response as a task. Optionally takes the zone ID too.
        /// </summary>
        public async static UniTask<PopupResponse<K>> SetModelAndShow<T, K>(Popup<T, K> popup, T model, string zoneID = DEFAULT) where T : ViewModel {
            await UniTask.WaitWhile(() => IsZoneFree(zoneID));
            zoneStateMap.SetPair(zoneID, true);
            var response = await popup.SetModelAndShow(model);
            zoneStateMap.SetPair(zoneID, false);
            return response;
        }

        /// <summary>
        /// Takes a popup, its model and displays it before returning the response as a callback. Optionally takes the zone ID too.
        /// </summary>
        public async static void SetModelAndShow<T, K>(Popup<T, K> popup, T model, Action<K> resultCallback, string zoneID = DEFAULT) where T : ViewModel =>
            resultCallback?.Invoke((await SetModelAndShow(popup, model, zoneID)).Data);

        /// <summary>
        /// Takes a popup and an Action for model change before displaying. Returns the response as a task and takes an optional zone ID
        /// </summary>
        public async static UniTask<PopupResponse<K>> ModifyModelAndShow<T, K>(Popup<T, K> popup, Action<T> modification, string zoneID = DEFAULT) where T : ViewModel {
            await UniTask.WaitWhile(() => IsZoneFree(zoneID));
            zoneStateMap.SetPair(zoneID, true);
            var response = await popup.ModifyModelAndShow(modification);
            zoneStateMap.SetPair(zoneID, false);
            return response;
        }

        /// <summary>
        /// Takes a popup and an Action for model change before displaying. Returns the response as a callback and takes an optional zone ID
        /// </summary>
        public async static void ModifyModelAndShow<T, K>(Popup<T, K> popup, Action<T> modification, Action<K> resultCallback, string zoneID = DEFAULT) where T : ViewModel =>
            resultCallback?.Invoke((await ModifyModelAndShow(popup, modification, zoneID)).Data);
    }
}