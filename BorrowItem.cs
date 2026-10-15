
using System;

namespace DeviceManagement
{
    public class BorrowItem
    {
        private TeachingDevice device;
        private DateTime borrowedDate;
        private DateTime dueDate;
        private int fromPeriod;
        private int toPeriod;
        private DateTime? returnedDate;

        public TeachingDevice Device
        {
            get { return device; }
        }

        public DateTime BorrowedDate
        {
            get { return borrowedDate; }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        public int FromPeriod
        {
            get { return fromPeriod; }
            set { fromPeriod = value; }
        }

        public int ToPeriod
        {
            get { return toPeriod; }
            set { toPeriod = value; }
        }

        public DateTime? ReturnedDate
        {
            get { return returnedDate; }
        }

        public bool IsReturned
        {
            get { return returnedDate.HasValue; }
        }

        public BorrowItem(
            TeachingDevice device,
            DateTime borrowedDate,
            DateTime dueDate,
            int fromPeriod = 1,
            int toPeriod = 5)
        {
            if (device == null)
            {
                throw new ArgumentNullException("Thiết bị không được null.");
            }
            if (dueDate < borrowedDate)
            {
                throw new ArgumentException("Hạn trả phải sau hoặc bằng ngày mượn.");
            }
            if (fromPeriod > toPeriod)
            {
                throw new ArgumentException("Tiết bắt đầu không được lớn hơn tiết kết thúc.");
            }

            this.device = device;
            this.borrowedDate = borrowedDate;
            this.dueDate = dueDate;
            this.fromPeriod = fromPeriod;
            this.toPeriod = toPeriod;
            this.returnedDate = null;
        }

        public void Return(DateTime returnedDate)
        {
            if (IsReturned)
            {
                throw new DeviceAlreadyReturnedException(device.DeviceId);
            }
            if (returnedDate < borrowedDate)
            {
                throw new InvalidReturnDateException(borrowedDate, returnedDate);
            }

            this.returnedDate = returnedDate;
        }

        public override string ToString()
{
    string returnStatus = returnedDate.HasValue
        ? ("Đã trả lúc: " + returnedDate.Value.ToString("dd/MM/yyyy HH:mm"))
        : "Chưa trả";
    return "- " + device.Name + " [" + device.DeviceId + "] | Mượn: " + borrowedDate.ToString("dd/MM/yyyy") +
           " -> Hạn: " + dueDate.ToString("dd/MM/yyyy") + " | Tiết " + fromPeriod + "-" + toPeriod + " | " + returnStatus;
}
    }
}
