
using System;

namespace DeviceManagement
{
    public class BorrowingSlip
    {
        private const decimal DAILY_LATE_FEE = 20000m;
        private const int MAX_ITEMS = 10;

        private string slipId;
        private IBorrower borrower;
        private string teachingRoom;
        private DateTime borrowedDate;
        private BorrowItem[] items;
        private int count;

        public string SlipId
        {
            get { return slipId; }
        }

        public IBorrower Borrower
        {
            get { return borrower; }
        }

        public string TeachingRoom
        {
            get { return teachingRoom; }
        }

        public DateTime BorrowedDate
        {
            get { return borrowedDate; }
        }

        public BorrowItem[] Items
        {
            get
            {
                BorrowItem[] result = new BorrowItem[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = items[i];
                }
                return result;
            }
        }

        public BorrowingSlip(string slipId, IBorrower borrower, string teachingRoom, DateTime borrowedDate)
        {
            if (slipId == null || slipId == "")
            {
                throw new ArgumentException("Mã phiếu mượn không được để trống.");
            }
            if (borrower == null)
            {
                throw new ArgumentNullException("Người mượn không được rỗng.");
            }
            if (teachingRoom == null || teachingRoom == "")
            {
                throw new ArgumentException("Phòng học không được để rỗng.");
            }

            this.slipId = slipId;
            this.borrower = borrower;
            this.teachingRoom = teachingRoom;
            this.borrowedDate = borrowedDate;
            this.items = new BorrowItem[MAX_ITEMS];
            this.count = 0;
        }

        public void AddDevice(TeachingDevice device, int borrowDays)
        {
            ValidateBorrowing(device, borrowDays);

            DateTime dueDate = borrowedDate.AddDays(borrowDays);
            BorrowItem item = new BorrowItem(device, borrowedDate, dueDate);

            device.MarkAsBorrowed();
            items[count] = item;
            count++;
        }

        private void ValidateBorrowing(TeachingDevice device, int borrowDays)
        {
            if (device == null)
            {
                throw new ArgumentNullException("Thiết bị không được rỗng.");
            }
            if (borrowDays <= 0 || borrowDays > device.MaxBorrowDays)
            {
                throw new InvalidBorrowPeriodException(device.DeviceId, borrowDays, device.MaxBorrowDays);
            }

            for (int i = 0; i < count; i++)
            {
                if (items[i].Device.DeviceId == device.DeviceId)
                {
                    throw new DuplicateDeviceException(device.DeviceId);
                }
            }

            if (count >= borrower.GetMaximumDevices())
            {
                throw new BorrowLimitExceededException(borrower.BorrowerId, borrower.GetMaximumDevices());
            }

            if (device.Status != DeviceStatus.Available)
            {
                throw new DeviceUnavailableException(device.DeviceId, device.Status);
            }
        }

        public decimal ReturnDevice(string deviceId, DateTime returnedDate)
        {
            if (deviceId == null || deviceId == "")
            {
                throw new ArgumentException("Mã thiết bị không được để trống.");
            }

           BorrowItem? item = null;
            for (int i = 0; i < count; i++)
            {
                if (items[i].Device.DeviceId == deviceId)
                {
                    item = items[i];
                    break;
                }
            }

            if (item == null)
            {
                throw new DeviceNotInSlipException(deviceId);
            }
            if (item.IsReturned)
            {
                throw new DeviceAlreadyReturnedException(deviceId);
            }

            item.Return(returnedDate);
            item.Device.MarkAsAvailable();

            decimal fee = 0m;
            if (returnedDate.Date > item.DueDate.Date)
            {
                int lateDays = (returnedDate.Date - item.DueDate.Date).Days;
                fee = lateDays * DAILY_LATE_FEE;
            }

            return fee;
        }

        public decimal CalculateTotalLateFee()
        {
            decimal totalFee = 0m;
            DateTime now = DateTime.Now;

            for (int i = 0; i < count; i++)
            {
                BorrowItem item = items[i];
                if (item.IsReturned && item.ReturnedDate.HasValue)
                {
                    if (item.ReturnedDate.Value.Date > item.DueDate.Date)
                    {
                        int lateDays = (item.ReturnedDate.Value.Date - item.DueDate.Date).Days;
                        totalFee += lateDays * DAILY_LATE_FEE;
                    }
                }
                else if (!item.IsReturned && now.Date > item.DueDate.Date)
                {
                    int overdueDays = (now.Date - item.DueDate.Date).Days;
                    totalFee += overdueDays * DAILY_LATE_FEE;
                }
            }

            return totalFee;
        }

        public override string ToString()
        {
            return "Phiếu mượn: " + slipId + " | Người mượn: " + borrower.FullName + " (" + borrower.BorrowerId +
                   ") | Phòng: " + teachingRoom + " | Ngày mượn: " + borrowedDate.ToString("dd/MM/yyyy") +
                   " | Số thiết bị: " + count;
        }
    }
}
