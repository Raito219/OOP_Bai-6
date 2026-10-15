
using System;

namespace DeviceManagement
{
    public class BorrowingException : Exception
    {
        public BorrowingException(string message) : base(message) { }

        public BorrowingException(string message, Exception innerException)
            : base(message, innerException) { }
    }
    public class DeviceUnavailableException : BorrowingException
    {
        public string DeviceId { get; }
        public DeviceStatus CurrentStatus { get; }

        public DeviceUnavailableException(string deviceId, DeviceStatus currentStatus)
            : base($"Thiết bị [{deviceId}] hiện không sẵn sàng (Trạng thái: {currentStatus}).")
        {
            DeviceId = deviceId;
            CurrentStatus = currentStatus;
        }
    }
    public class BorrowLimitExceededException : BorrowingException
    {
        public string BorrowerId { get; }
        public int MaximumDevices { get; }

        public BorrowLimitExceededException(string borrowerId, int maximumDevices)
            : base($"Người mượn [{borrowerId}] đã đạt giới hạn mượn tối đa ({maximumDevices} thiết bị).")
        {
            BorrowerId = borrowerId;
            MaximumDevices = maximumDevices;
        }
    }
    public class InvalidBorrowPeriodException : BorrowingException
    {
        public string DeviceId { get; }
        public int RequestedDays { get; }
        public int MaxAllowedDays { get; }

        public InvalidBorrowPeriodException(string deviceId, int requestedDays, int maxAllowedDays)
            : base($"Thời hạn mượn {requestedDays} ngày cho thiết bị [{deviceId}] không hợp lệ (Tối đa: {maxAllowedDays} ngày).")
        {
            DeviceId = deviceId;
            RequestedDays = requestedDays;
            MaxAllowedDays = maxAllowedDays;
        }
    }

    public class DuplicateDeviceException : BorrowingException
    {
        public string DeviceId { get; }

        public DuplicateDeviceException(string deviceId)
            : base($"Thiết bị [{deviceId}] đã tồn tại trong phiếu mượn.")
        {
            DeviceId = deviceId;
        }
    }
    public class DeviceNotInSlipException : BorrowingException
    {
        public string DeviceId { get; }

        public DeviceNotInSlipException(string deviceId)
            : base($"Thiết bị [{deviceId}] không tồn tại trong phiếu mượn này.")
        {
            DeviceId = deviceId;
        }
    }

    public class DeviceAlreadyReturnedException : BorrowingException
    {
        public string DeviceId { get; }

        public DeviceAlreadyReturnedException(string deviceId)
            : base($"Thiết bị [{deviceId}] đã được trả trước đó.")
        {
            DeviceId = deviceId;
        }
    }

    public class InvalidReturnDateException : BorrowingException
    {
        public DateTime BorrowedDate { get; }
        public DateTime ReturnedDate { get; }

        public InvalidReturnDateException(DateTime borrowedDate, DateTime returnedDate)
            : base($"Ngày trả ({returnedDate:dd/MM/yyyy}) không được trước ngày mượn ({borrowedDate:dd/MM/yyyy}).")
        {
            BorrowedDate = borrowedDate;
            ReturnedDate = returnedDate;
        }
    }
}