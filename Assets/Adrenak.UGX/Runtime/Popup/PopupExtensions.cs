using Cysharp.Threading.Tasks;
using System.Collections.Generic;

using System;

namespace Adrenak.UGX {
    public static class PopupExtensions {
        /// <summary>
        /// The default ZoneID of popups
        /// </summary>
        public const string DEFAULT = "default";

        /// <summary>
        /// Maps zone ID to availability
        /// </summary>
        readonly static Dictionary<string, bool> zoneStateMap = 
            new Dictionary<string, bool>();

        /// <summary>
        /// Returns whether any popup is active in a given zone ID
        /// </summary>
        /// <param name="zoneID"></param>
        public static bool IsZoneFree(string zoneID = DEFAULT) {
            if (zoneStateMap.ContainsKey(zoneID))
                return zoneStateMap[zoneID];
            return false;
        }

        /// <summary>
        /// Displays a popup and returns the response as a task. 
        /// Optionally takes the zone ID too.
        /// </summary>
        public async static UniTask<K> Show<T, K>(
            this Popup<T, K> popup, 
            string zoneID = DEFAULT
        ) where T : State {
            await UniTask.WaitWhile(() => IsZoneFree(zoneID));
            zoneStateMap.Set(zoneID, true);
            var response = await popup.Show();
            zoneStateMap.Set(zoneID, false);
            return response;
        }

        /// <summary>
        /// Displays a popup and returns the response as a callback. 
        /// Optionally takes the zone ID too.
        /// </summary>
        public async static void Show<T, K>(
            this Popup<T, K> popup, 
            Action<K> response,
            string zoneID = DEFAULT
        ) where T : State {
            var r = await Show(popup, zoneID);
            response?.Invoke(r);
        }

        /// <summary>
        /// Sets view state and displays it. Returns the response as a task. 
        /// Optionally takes the zone ID too.
        /// </summary>
        public async static UniTask<K> SetStateAndShow<T, K>(
            this Popup<T, K> popup, 
            T state, 
            string zoneID = DEFAULT
        ) where T : State {
            await UniTask.WaitWhile(() => IsZoneFree(zoneID));
            zoneStateMap.Set(zoneID, true);
            var response = await popup.SetStateAndShow(state);
            zoneStateMap.Set(zoneID, false);
            return response;
        }

        /// <summary>
        /// Sets view state and displays it. returns response as a callback. 
        /// Optionally takes the zone ID too.
        /// </summary>
        public async static void SetStateAndShow<T, K>(
            this Popup<T, K> popup, 
            T state, 
            Action<K> response, 
            string zoneID = DEFAULT
        ) where T : State {
            var r = await SetStateAndShow(popup, state, zoneID);
            response?.Invoke(r);
        }

        /// <summary>
        /// Provides access to popup state for modification and displays it. 
        /// Returns the response as a task and takes an optional zone ID
        /// </summary>
        public async static UniTask<K> ModifyStateAndShow<T, K>(
            this Popup<T, K> popup, 
            Action<T> modification, 
            string zoneID = DEFAULT
        ) where T : State {
            await UniTask.WaitWhile(() => IsZoneFree(zoneID));
            zoneStateMap.Set(zoneID, true);
            var response = await popup.ModifyStateAndShow(modification);
            zoneStateMap.Set(zoneID, false);
            return response;
        }

        /// <summary>
        /// Provides access to popup state for modification and displays it. 
        /// Returns the response as a callback and takes an optional zone ID
        /// </summary>
        public async static void ModifyStateAndShow<T, K>(
            this Popup<T, K> popup, 
            Action<T> modification, 
            Action<K> response, 
            string zoneID = DEFAULT
        ) where T : State {
            var r = await ModifyStateAndShow(popup, modification, zoneID);
            response?.Invoke(r);
        }
    }
}