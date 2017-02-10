using IndDev.Domain.Abstract;

namespace IndDev.Infrastructure
{
    public class TrackingUsers
    {
        private readonly ITrack _repo;

        public TrackingUsers(ITrack repo)
        {
            _repo = repo;
        }
    }
}