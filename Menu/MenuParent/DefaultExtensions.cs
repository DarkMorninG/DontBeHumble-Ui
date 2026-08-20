namespace DBH.UI.Menu.MenuParent {
    public class DefaultExtensions : ExecutableMenu {
        public override ICover Cover => null;
        
        public ItemHolder ItemHolderOverride { get; set; }
        protected override ItemHolder ItemHolder() {
            return ItemHolderOverride ?? base.ItemHolder();
        }
    }
}