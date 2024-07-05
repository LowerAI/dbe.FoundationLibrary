namespace dbe.FoundationLibrary.Windows.Mvvm
{
    public delegate T Notification<T, E>(E e);
    public delegate void Notification<E>(E e);
    public delegate void Notification();
}
