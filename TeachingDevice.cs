
using System;

namespace DeviceManagement
{
    public abstract class TeachingDevice : Device
    {
        protected int maxBorrowDays;

        public int MaxBorrowDays
        {
            get { return maxBorrowDays; }
            set { maxBorrowDays = value; }
        }

        public TeachingDevice(
            string deviceId,
            string name,
            int yearOfUse,
            decimal price,
            int maxBorrowDays = 1,
            DeviceStatus status = DeviceStatus.Available)
            : base(deviceId, name, yearOfUse, price, status)
        {
            if (maxBorrowDays <= 0)
            {
                throw new ArgumentException("Số ngày mượn tối đa phải lớn hơn 0.");
            }
            this.maxBorrowDays = maxBorrowDays;
        }

        public override string ToString()
        {
            return base.ToString() + " - Hạn mượn tối đa: " + maxBorrowDays + " ngày";
        }
    }
}
