using System;
using System.Linq;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.TrackingUser;

namespace IndDev.Domain.Context
{
    public class Tracking:ITrack
    {
        private readonly DataContext _context = new DataContext();

        public void CreateNewVisitor(Visitor visitor, UserRout route)
        {
            var visit =
                _context.Visitors.FirstOrDefault(
                    visitor1 => visitor1.Identifer.Equals(visitor.Identifer, StringComparison.CurrentCultureIgnoreCase));
            if (visit == null)
            {
                var v = CreateNew(visitor);
                route.Visitor = v;
                _context.UserRouts.Add(route);
            }
            else
            {
                if (string.IsNullOrEmpty(visit.StartUrl))
                {
                    visit.StartUrl = visitor.StartUrl;
                }
                visit.LastVisit = DateTime.Now;
                visit.UserRouts.Add(route);
            }
            _context.SaveChanges();
        }

        private Visitor CreateNew(Visitor visitor)
        {
            _context.Visitors.Add(visitor);
            _context.SaveChanges();
            return visitor;
        }
    }
}