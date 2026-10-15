using System;

namespace DeviceManagement
{
    public class ClassRoom
    {
        private string roomId;
        private string roomName;
        private int capacity;
        private Device[] devices;
        private int count;

        public string RoomId
        {
            get { return roomId; }
        }

        public string RoomName
        {
            get { return roomName; }
            set { roomName = value; }
        }

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public Device[] Devices
        {
            get
            {
                Device[] result = new Device[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = devices[i];
                }
                return result;
            }
        }

        public ClassRoom(string roomId, string roomName, int capacity)
        {
            if (roomId == null || roomId == "")
            {
                throw new ArgumentException("Mã phòng không được để trống.");
            }
            if (capacity <= 0)
            {
                throw new ArgumentException("Sức chứa phòng phải lớn hơn 0.");
            }

            this.roomId = roomId;
            this.roomName = roomName;
            this.capacity = capacity;
            this.devices = new Device[capacity];
            this.count = 0;
        }

public Device? FindDevice(string deviceId)        {
            if (deviceId == null || deviceId == "")
            {
                return null;
            }
            for (int i = 0; i < count; i++)
            {
                if (devices[i].DeviceId == deviceId)
                {
                    return devices[i];
                }
            }
            return null;
        }

        public void AddDevice(Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("Không chấp nhận đối tượng thiết bị null.");
            }
            if (FindDevice(device.DeviceId) != null)
            {
                throw new InvalidOperationException("Thiết bị mã " + device.DeviceId + " đã tồn tại trong phòng " + roomId + ".");
            }
            if (count >= devices.Length)
            {
                throw new InvalidOperationException("Phòng đã chứa tối đa " + devices.Length + " thiết bị.");
            }

            devices[count] = device;
            count++;
        }

        public bool RemoveDevice(string deviceId)
        {
            for (int i = 0; i < count; i++)
            {
                if (devices[i].DeviceId == deviceId)
                {
                    for (int j = i; j < count - 1; j++)
                    {
                        devices[j] = devices[j + 1];
                    }
                    devices[count - 1] = null!;
                    count--;
                    return true;
                }
            }
            return false;
        }

        public decimal CalculateAnnualMaintenanceCost()
        {
            decimal totalCost = 0;
            for (int i = 0; i < count; i++)
            {
                totalCost += devices[i].CalculateAnnualMaintenanceCost();
            }
            return totalCost;
        }

        public Device[] GetDevicesRequiringMaintenance()
        {
            int currentYear = DateTime.Now.Year;
            int matchCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (devices[i].Status == DeviceStatus.UnderMaintenance || (currentYear - devices[i].YearOfUse) > 5)
                {
                    matchCount++;
                }
            }

            Device[] result = new Device[matchCount];
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (devices[i].Status == DeviceStatus.UnderMaintenance || (currentYear - devices[i].YearOfUse) > 5)
                {
                    result[index] = devices[i];
                    index++;
                }
            }
            return result;
        }

        public override string ToString()
        {
            return "Phòng " + roomId + " - " + roomName + " (Sức chứa: " + capacity + ") - Số thiết bị: " + count;
        }
    }
}
