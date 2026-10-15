
using System;

namespace DeviceManagement
{
    public abstract class Device
    {
        protected string deviceId;
        protected string name;
        protected int yearOfUse;
        protected decimal price;
        protected DeviceStatus status;

        public Device(string deviceId, string name, int yearOfUse, decimal price, DeviceStatus status = DeviceStatus.Available)
        {
            if (deviceId == null || deviceId == "")
            {
                throw new ArgumentException("Mã thiết bị không được để trống.");
            }
            if (name == null || name == "")
            {
                throw new ArgumentException("Tên thiết bị không được để trống.");
            }
            if (yearOfUse > DateTime.Now.Year)
            {
                throw new ArgumentException("Năm đưa vào sử dụng không được lớn hơn năm hiện tại.");
            }
            if (price <= 0)
            {
                throw new ArgumentException("Giá mua phải lớn hơn 0.");
            }

            this.deviceId = deviceId;
            this.name = name;
            this.yearOfUse = yearOfUse;
            this.price = price;
            this.status = status;
        }

        public string DeviceId
        {
            get { return deviceId; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int YearOfUse
        {
            get { return yearOfUse; }
            set { yearOfUse = value; }
        }

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public DeviceStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public abstract decimal CalculateAnnualMaintenanceCost();

        public virtual void MarkAsBorrowed()
        {
            this.status = DeviceStatus.Borrowed;
        }

        public virtual void MarkAsAvailable()
        {
            this.status = DeviceStatus.Available;
        }

        public virtual void MarkAsUnderMaintenance()
        {
            this.status = DeviceStatus.UnderMaintenance;
        }

        public override string ToString()
        {
            return "[" + deviceId + "] " + name + " - Năm SD: " + yearOfUse + " - Giá: " + price.ToString("N0") + " VNĐ - Trạng thái: " + status;
        }
    }
}