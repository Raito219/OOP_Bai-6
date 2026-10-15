# BÀI TẬP TUẦN 6: HỆ THỐNG QUẢN LÝ THIẾT BỊ VÀ MƯỢN TRẢ THIẾT BỊ DẠY HỌC

- **Môn học:** Lập trình Hướng đối tượng (OOP)
- **Mã sinh viên:** 202418836
- **Họ và tên:** Nguyễn Vũ Quang Anh
- **Ngôn ngữ:** C# (.NET)

---

## 1. Mục tiêu kiến thức áp dụng

Dự án được xây dựng nhằm áp dụng và củng cố các nguyên lý Lập trình hướng đối tượng (OOP) cùng cơ chế xử lý ngoại lệ trong C#:
* **Kế thừa (Inheritance) & Lớp trừu tượng (Abstract Class):** Xây dựng cây phân cấp thiết bị từ lớp cơ sở trừu tượng `Device` -> `TeachingDevice` -> 3 lớp thiết bị cụ thể (`AirConditionerRemote`, `Microphone`, `ProjectorRemote`).
* **Giao diện (Interface):** Định nghĩa vai trò và hạn mức mượn thiết bị qua giao diện `IBorrower`, thực thi bởi `Lecturer` và `TeachingAssistant`.
* **Đa hình (Polymorphism):** Tính tổng chi phí bảo trì dự kiến hằng năm trong `ClassRoom.CalculateAnnualMaintenanceCost()` thông qua liên kết động mà không sử dụng `if-else` hay `switch-case` để kiểm tra kiểu thiết bị.
* **Kết tập (Aggregation), Hợp thành (Composition) & Liên kết (Association):**
  * **Kết tập:** `ClassRoom` quản lý danh sách `Device` (thiết bị tồn tại độc lập với phòng học).
  * **Hợp thành:** `BorrowingSlip` quản lý danh sách `BorrowItem` (mục mượn gắn liền vòng đời với phiếu mượn).
  * **Liên kết:** `BorrowingSlip` tham chiếu tới `IBorrower` và `BorrowItem` tham chiếu tới `TeachingDevice` (xóa phiếu không xóa người mượn hay thiết bị).
* **Xử lý ngoại lệ (Exception Handling) & Tính nhất quán trạng thái (State Consistency):** Thiết kế cây phân cấp ngoại lệ `BorrowingException` và đảm bảo khi thao tác mượn/trả thất bại, trạng thái của thiết bị và phiếu mượn được giữ nguyên vẹn như trước thao tác.

---

## 2. Kiến trúc hệ thống

1. **Phân cấp Thiết bị (`Device` -> `TeachingDevice`):**
   * `AirConditionerRemote` (Điều khiển điều hòa): Chi phí bảo trì/năm = `5% giá mua + 50,000 VNĐ`.
   * `Microphone` (Micro): Chi phí bảo trì/năm = `10% giá mua` (+ `80,000 VNĐ` nếu là micro không dây `IsWireless = true`).
   * `ProjectorRemote` (Điều khiển máy chiếu): Chi phí bảo trì/năm = `8% giá mua`.
   * Trạng thái thiết bị (`DeviceStatus`): `Available`, `Borrowed`, `UnderMaintenance`.
2. **Quản lý Phòng học (`ClassRoom`):**
   * Quản lý danh sách thiết bị theo sức chứa phòng (không nhận `null`, không trùng `DeviceId`).
   * Cung cấp các phương thức: `AddDevice()`, `RemoveDevice()`, `FindDevice()`, `CalculateAnnualMaintenanceCost()` và `GetDevicesRequiringMaintenance()` (lọc thiết bị đang `UnderMaintenance` hoặc có thời gian sử dụng `> 5` năm).
3. **Phân quyền Người mượn (`IBorrower`):**
   * `Lecturer` (Giảng viên): Được mượn tối đa **3 thiết bị** trên một phiếu.
   * `TeachingAssistant` (Trợ giảng): Được mượn tối đa **2 thiết bị** trên một phiếu.
4. **Nghiệp vụ Mượn – Trả (`BorrowingSlip` & `BorrowItem`):**
   * Kiểm tra toàn bộ điều kiện hợp lệ trong `ValidateBorrowing()` trước khi tạo `BorrowItem` và chuyển trạng thái thiết bị sang `Borrowed`.
   * Ghi nhận ngày trả thực tế, khôi phục trạng thái thiết bị về `Available` và tự động tính phí phạt trả muộn (`20,000 VNĐ/ngày`).
5. **Hệ phân cấp Ngoại lệ nghiệp vụ (`BorrowingException`):**
   * `DeviceUnavailableException`: Thiết bị không ở trạng thái `Available`.
   * `BorrowLimitExceededException`: Người mượn vượt quá số lượng thiết bị tối đa cho phép.
   * `InvalidBorrowPeriodException`: Số ngày mượn không dương hoặc vượt quá `MaxBorrowDays` của thiết bị.
   * `DuplicateDeviceException`: Thêm trùng một thiết bị hai lần vào cùng một phiếu mượn.
   * `DeviceNotInSlipException`: Trả thiết bị có mã không thuộc phiếu mượn.
   * `DeviceAlreadyReturnedException`: Trả thiết bị đã được trả trước đó.
   * `InvalidReturnDateException`: Ngày trả thiết bị trước ngày mượn.

---

## 3. Cấu trúc tệp mã nguồn

| Tệp mã nguồn | Mô tả vai trò |
| :--- | :--- |
| `DeviceStatus.cs` | Kiểu liệt kê (`enum`) trạng thái hoạt động của thiết bị. |
| `Device.cs` | Lớp cơ sở trừu tượng định nghĩa thuộc tính chung và phương thức trừu tượng tính phí bảo trì. |
| `TeachingDevice.cs` | Lớp trừu tượng kế thừa `Device`, bổ sung hạn mượn tối đa (`MaxBorrowDays`). |
| `AirConditionerRemote.cs` | Lớp dẫn xuất biểu diễn Điều khiển điều hòa. |
| `Microphone.cs` | Lớp dẫn xuất biểu diễn Micro (hỗ trợ phân biệt có dây / không dây). |
| `ProjectorRemote.cs` | Lớp dẫn xuất biểu diễn Điều khiển máy chiếu. |
| `ClassRoom.cs` | Lớp quản lý phòng học và danh sách thiết bị (quan hệ Kết tập). |
| `IBorrower.cs` | Giao diện định nghĩa người mượn và hạn mức mượn tối đa. |
| `Lecturer.cs` | Lớp Giảng viên thực thi `IBorrower` (tối đa 3 thiết bị). |
| `TeachingAssistant.cs` | Lớp Trợ giảng thực thi `IBorrower` (tối đa 2 thiết bị). |
| `BorrowItem.cs` | Lớp biểu diễn chi tiết một mục thiết bị được mượn trong phiếu. |
| `BorrowingSlip.cs` | Lớp quản lý phiếu mượn (quan hệ Hợp thành với `BorrowItem`), xử lý mượn/trả và tính phí trễ hạn. |
| `BorrowingExceptions.cs` | Định nghĩa lớp cơ sở `BorrowingException` và 7 lớp ngoại lệ dẫn xuất. |
| `Program.cs` | Chương trình chính thực thi các kịch bản kiểm thử bằng khối `try - catch - finally`. |

---

## 4. Sơ đồ lớp (Class Diagram)

```mermaid
classDiagram
    direction TB

    class DeviceStatus {
        <<enumeration>>
        Available
        Borrowed
        UnderMaintenance
    }

    class Device {
        <<abstract>>
        +string DeviceId
        +string Name
        +int YearOfUse
        +decimal Price
        +DeviceStatus Status
        +CalculateAnnualMaintenanceCost()* decimal
        +MarkAsBorrowed() void
        +MarkAsAvailable() void
        +MarkAsUnderMaintenance() void
        +ToString() string
    }

    class TeachingDevice {
        <<abstract>>
        +int MaxBorrowDays
        +ToString() string
    }

    class AirConditionerRemote {
        +CalculateAnnualMaintenanceCost() decimal
        +ToString() string
    }

    class Microphone {
        +bool IsWireless
        +CalculateAnnualMaintenanceCost() decimal
        +ToString() string
    }

    class ProjectorRemote {
        +CalculateAnnualMaintenanceCost() decimal
        +ToString() string
    }

    class ClassRoom {
        -string roomId
        -string roomName
        -int capacity
        -Device[] devices
        +AddDevice(Device) void
        +RemoveDevice(string) bool
        +FindDevice(string) Device
        +CalculateAnnualMaintenanceCost() decimal
        +GetDevicesRequiringMaintenance() Device[]
    }

    class IBorrower {
        <<interface>>
        +string BorrowerId
        +string FullName
        +GetMaximumDevices() int
    }

    class Lecturer {
        +string BorrowerId
        +string FullName
        +GetMaximumDevices() int
    }

    class TeachingAssistant {
        +string BorrowerId
        +string FullName
        +GetMaximumDevices() int
    }

    class BorrowingSlip {
        +string SlipId
        +IBorrower Borrower
        +string TeachingRoom
        +DateTime BorrowedDate
        +BorrowItem[] Items
        +AddDevice(TeachingDevice, int) void
        +ReturnDevice(string, DateTime) decimal
        +CalculateTotalLateFee() decimal
    }

    class BorrowItem {
        +TeachingDevice Device
        +DateTime BorrowedDate
        +DateTime DueDate
        +int FromPeriod
        +int ToPeriod
        +DateTime? ReturnedDate
        +bool IsReturned
        +Return(DateTime) void
    }

    Device <|-- TeachingDevice
    TeachingDevice <|-- AirConditionerRemote
    TeachingDevice <|-- Microphone
    TeachingDevice <|-- ProjectorRemote
    Device ..> DeviceStatus
    ClassRoom o-- "0..*" Device : devices
    IBorrower <|.. Lecturer
    IBorrower <|.. TeachingAssistant
    BorrowingSlip --> "1" IBorrower : borrower
    BorrowingSlip *-- "0..*" BorrowItem : items
    BorrowItem --> "1" TeachingDevice : device