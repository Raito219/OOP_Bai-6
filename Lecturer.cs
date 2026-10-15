
using System;

namespace DeviceManagement
{
    public class Lecturer : IBorrower
    {
        private string borrowerId;
        private string fullName;

        public string BorrowerId
        {
            get { return borrowerId; }
        }

        public string FullName
        {
            get { return fullName; }
        }

        public Lecturer(string borrowerId, string fullName)
        {
            if (borrowerId == null || borrowerId == "")
            {
                throw new ArgumentException("Mã giảng viên không được để trống.");
            }
            if (fullName == null || fullName == "")
            {
                throw new ArgumentException("Họ tên giảng viên không được để trống.");
            }

            this.borrowerId = borrowerId;
            this.fullName = fullName;
        }

        // Trả về số thiết bị tối đa giảng viên được mượn là 3
        public int GetMaximumDevices()
        {
            return 3;
        }

        public override string ToString()
        {
            return "[Giảng viên] " + fullName + " (" + borrowerId + ") - Tối đa: " + GetMaximumDevices() + " thiết bị";
        }
    }
}