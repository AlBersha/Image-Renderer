
using Raytracer.Scene;

namespace Renderer
{
    public class Startup
    {
        private ISceneCreator _sceneCreator;

        public Startup(ISceneCreator sceneCreator)
        {
            _sceneCreator = sceneCreator;
        }
        
        // todo the class supposed to be all required config setter

        public void ConfigureExecutor()
        {
            // 1. console management
            // 2. build tree
            // 3. create screen
            _sceneCreator.CreateScreen();
            
            
            
            
            
            // 4. execute tracing
        }
    }
}