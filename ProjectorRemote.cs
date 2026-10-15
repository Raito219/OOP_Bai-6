
namespace DeviceManagement
{
    public class ProjectorRemote : TeachingDevice
    {
        public ProjectorRemote(
            string deviceId,
            string name,
            int yearOfUse,
            decimal price,
            int maxBorrowDays = 1,
            DeviceStatus status = DeviceStatus.Available)
            : base(deviceId, name, yearOfUse, price, maxBorrowDays, status)
        {
        }

        public override decimal CalculateAnnualMaintenanceCost()
        {
            return price * 0.08m;
        }

        public override string ToString()
        {
            return "[Điều Khiển Máy Chiếu] " + base.ToString();
        }
    }
}
