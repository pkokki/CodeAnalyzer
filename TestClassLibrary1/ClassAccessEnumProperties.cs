namespace TestClassLibrary1
{
    class ClassAccessEnumProperties
    {
        ClassWithEnumProperty1 classWithEnumProperty1 = new ClassWithEnumProperty1();
        ClassWithEnumProperty2 classWithEnumProperty2 = new ClassWithEnumProperty2();

        public void Dummy()
        {
            classWithEnumProperty1.ClientFlags = classWithEnumProperty2.ClientFlags;
        }
    }
}
