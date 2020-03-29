using StackExchange.Redis;

namespace TestClassLibrary1
{
    class ClassAccessEnumValue
    {
        public bool Method()
        {
            var x = new ClassWithEnumProperty1();
            return x.ClientFlags == ClientFlags.Master;
        }
    }
}
