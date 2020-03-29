using SR = StackExchange.Redis;

namespace TestClassLibrary1
{
    class ClassAccessEnumValueWithUsing
    {
        public bool Method()
        {
            var x = new ClassWithEnumProperty2();
            return x.ClientFlags == SR.ClientFlags.Slave;
        }
    }
}
