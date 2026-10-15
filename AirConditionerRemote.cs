
namespace DeviceManagement
{
    public class AirConditionerRemote : TeachingDevice
    {
        public AirConditionerRemote(
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
            return (price * 0.05m) + 50000m;
        }

        public override string ToString()
        {
            return "[Điều Khiển Điều Hòa] " + base.ToString();
        }
    }
}
