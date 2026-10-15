
namespace DeviceManagement
{
    public class Microphone : TeachingDevice
    {
        private bool isWireless;

        public bool IsWireless
        {
            get { return isWireless; }
            set { isWireless = value; }
        }

        public Microphone(
            string deviceId,
            string name,
            int yearOfUse,
            decimal price,
            int maxBorrowDays = 1,
            bool isWireless = true,
            DeviceStatus status = DeviceStatus.Available)
            : base(deviceId, name, yearOfUse, price, maxBorrowDays, status)
        {
            this.isWireless = isWireless;
        }

        public override decimal CalculateAnnualMaintenanceCost()
        {
            decimal baseCost = price * 0.10m;
            return isWireless ? (baseCost + 80000m) : baseCost;
        }

        public override string ToString()
        {
            string type = isWireless ? "Không dây" : "Có dây";
            return "[Micro (" + type + ")] " + base.ToString();
        }
    }
}
