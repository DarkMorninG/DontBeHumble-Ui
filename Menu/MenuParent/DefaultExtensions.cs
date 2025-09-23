namespace DBH.UI.Menu.MenuParent {
    public class DefaultExtensions : ExecutableMenu {
        public override Cover Cover => null;
        
        public ItemHolder ItemHolderOverride { get; set; }
        protected override ItemHolder ItemHolder() {
            return ItemHolderOverride ?? base.ItemHolder();
        }
    }
}