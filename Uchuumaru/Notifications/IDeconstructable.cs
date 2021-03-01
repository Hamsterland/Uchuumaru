namespace Uchuumaru.Notifications
{
    public interface IDeconstructable<out T>
    {
        T Deconstruct();
    }
}