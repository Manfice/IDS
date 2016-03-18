namespace IndDev.Domain.Context
{
    public class ContextFabric
    {
        private static DataContext _context;

        public static DataContext Context
        {
            get {
                if (_context != null)
                {
                    return _context;
                }
                else
                {
                   return _context=new DataContext();
                    
                }
            }
            set { _context = value; }
        }

    }
}