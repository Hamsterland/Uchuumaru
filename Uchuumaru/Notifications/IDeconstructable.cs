namespace Uchuumaru.Notifications
{
    public interface IDeconstructable<T>
    {
        T Deconstruct();
    }
}