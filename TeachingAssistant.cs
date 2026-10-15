
using System;

namespace DeviceManagement
{
    public class TeachingAssistant : IBorrower
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

        public TeachingAssistant(string borrowerId, string fullName)
        {
            if (borrowerId == null || borrowerId == "")
            {
                throw new ArgumentException("Mã trợ giảng không được để trống.");
            }
            if (fullName == null || fullName == "")
            {
                throw new ArgumentException("Họ tên trợ giảng không được để trống.");
            }

            this.borrowerId = borrowerId;
            this.fullName = fullName;
        }

        // Trả về số thiết bị tối đa trợ giảng được mượn là 2
        public int GetMaximumDevices()
        {
            return 2;
        }

        public override string ToString()
        {
            return "[Trợ giảng] " + fullName + " (" + borrowerId + ") - Tối đa: " + GetMaximumDevices() + " thiết bị";
        }
    }
}