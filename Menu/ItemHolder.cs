using System.Collections.Generic;
using System.Linq;
using DBH.Base;
using Sirenix.OdinInspector;
using Vault;

namespace DBH.UI.Menu {
    public class ItemHolder : DBHMono {
        [ReadOnly]
        [ShowInInspector]
        private HashSet<object> toHold = new();

        public void AddItem(object toAdd) {
            toHold.Add(toAdd);
        }

        public void AddOrUpdateItem<T>(T toUpdate) {
            toHold.Where(o => o is T)
                .Cast<T>()
                .FirstOptional()
                .IfPresent(obj => toHold.Remove(obj));
            toHold.Add(toUpdate);
        }

        public T Item<T>() {
            return toHold.Where(o => o is T)
                .Cast<T>()
                .FirstOrDefault();
        }

        public bool Contains<T>() {
            return toHold.Any(o => o is T);
        }

        public void Clear() {
            toHold.Clear();
        }

        public void TransferTo(ItemHolder itemHolder) {
            toHold.ForEach(itemHolder.AddItem);
        }
    }
}