
namespace System.Reflection
{


    public static class Assembly2
    {

        public static System.Reflection.Assembly GetExecutingAssembly<T>()
        {
            return GetExecutingAssembly(typeof(T));
        }

        public static System.Reflection.Assembly GetExecutingAssembly(System.Type t)
        {
            return System.Reflection.IntrospectionExtensions.GetTypeInfo(t).Assembly;
        }

    }


}
