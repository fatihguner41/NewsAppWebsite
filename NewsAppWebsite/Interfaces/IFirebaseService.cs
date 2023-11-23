using FireSharp.Interfaces;

namespace NewsAppWebsite.Interfaces
{
    public interface IFirebaseService
    {
        IFirebaseClient FirebaseClient { get; }
    }
}
