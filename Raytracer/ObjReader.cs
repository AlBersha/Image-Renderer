using System.IO;
using OBJ3DWavefrontLoader;

namespace Raytracer
{
    public static class ObjReader
    {
        public static SimpleMesh ReadObjFile(string path)
        {
            SimpleMesh simpleMesh;
            using (var reader = new StreamReader(path))
            {
                simpleMesh = SimpleMesh.LoadFromObj(reader);
            }

            return simpleMesh;
        }
    }
}