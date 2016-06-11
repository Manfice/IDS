namespace IndDev.Domain.Logic
{
    public class ExternalLogic
    {
        public string GetArt(int id)
        {
            const string vend = "IDS";
            var res = id.ToString().PadLeft(7, '0');
            return vend + "-" + res;
        }
    }
}