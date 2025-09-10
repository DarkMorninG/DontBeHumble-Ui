namespace DBH.UI.Controller {
    public interface IMenuInteractionChanged {

        int Order() {
            return 10;
        }
        
        void Enabled() {
            
        }

        void BeforeEnabled() {
            
        }

        void Disabled() {
        }
        
    }
}