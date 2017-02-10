using IndDev.Domain.Entity.TrackingUser;

namespace IndDev.Domain.Abstract
{
    public interface ITrack
    {
        void CreateNewVisitor(Visitor visitor, UserRout route);
    }
}