// =====================================================================
//  Mã sinh viên: 202418836
//  Họ tên      : Nguyễn Vũ Quang Anh
// =====================================================================
using System;
using System.Text;

namespace DeviceManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("==================================================");
            Console.WriteLine("   KIỂM THỬ HỆ THỐNG QUẢN LÝ & MƯỢN TRẢ THIẾT BỊ");
            Console.WriteLine("==================================================\n");

            // --- KIỂM THỬ PHÒNG HỌC (CLASSROOM) & TÍNH ĐA HÌNH ---
            Console.WriteLine("--- [1] KIỂM THỬ PHÒNG HỌC (CLASSROOM) ---");
            ClassRoom room = new ClassRoom("P301", "Phòng học lý thuyết 301", 40);
            room.AddDevice(new ProjectorRemote("PR01", "ĐK Máy chiếu Sony", 2019, 500000m, 2));
            room.AddDevice(new AirConditionerRemote("AC01", "ĐK Điều hòa Daikin", 2023, 400000m, 1, DeviceStatus.UnderMaintenance));
            room.AddDevice(new Microphone("MC01", "Micro Shure Không dây", 2024, 1500000m, 1, true));

            Console.WriteLine(room);
            Console.WriteLine($"Tổng chi phí bảo trì dự kiến/năm: {room.CalculateAnnualMaintenanceCost():N0} VNĐ");
            Console.WriteLine("Danh sách thiết bị cần bảo trì (UnderMaintenance hoặc > 5 năm):");
            foreach (var d in room.GetDevicesRequiringMaintenance())
            {
                Console.WriteLine("  " + d);
            }

            // --- MỤC 11.1: LUỒNG THÀNH CÔNG ---
            Console.WriteLine("\n--- [2] MỤC 11.1: KIỂM THỬ LUỒNG THÀNH CÔNG ---");
            DateTime borrowDate = new DateTime(2026, 10, 10);

            // 1. Tạo một giảng viên
            IBorrower lecturer = new Lecturer("GV001", "TS. Nguyễn Văn A");

            // 2. Tạo 1 ĐK máy chiếu, 1 ĐK điều hòa, 1 micro không dây
            var projRemote = new ProjectorRemote("PR101", "ĐK Máy chiếu Panasonic", 2024, 450000m, maxBorrowDays: 2);
            var acRemote = new AirConditionerRemote("AC101", "ĐK Điều hòa Casper", 2024, 350000m, maxBorrowDays: 1);
            var wirelessMic = new Microphone("MC101", "Micro Sennheiser", 2025, 1800000m, maxBorrowDays: 1, isWireless: true);

            // 3. Tạo phiếu mượn cho một phòng dạy
            BorrowingSlip slip = new BorrowingSlip("SLIP001", lecturer, "P301", borrowDate);

            try
            {
                // 4. Thêm ĐK máy chiếu và micro vào phiếu
                slip.AddDevice(projRemote, 1);
                slip.AddDevice(wirelessMic, 1);
                Console.WriteLine("Đã tạo phiếu và mượn thiết bị thành công:");
                Console.WriteLine(slip);

                // 5. Trả ĐK máy chiếu đúng hạn (mượn 1 ngày, trả sau 1 ngày)
                decimal fee1 = slip.ReturnDevice("PR101", borrowDate.AddDays(1));
                Console.WriteLine($"Trả PR101 đúng hạn -> Phí phạt: {fee1:N0} VNĐ");

                // 6. Trả micro muộn 2 ngày (hạn 1 ngày -> trả sau 3 ngày)
                decimal fee2 = slip.ReturnDevice("MC101", borrowDate.AddDays(3));
                Console.WriteLine($"Trả MC101 muộn 2 ngày -> Phí phạt: {fee2:N0} VNĐ");

                // 7. Hiển thị các thiết bị trong phiếu (trả muộn, chưa trả...)
                Console.WriteLine("\nChi tiết các thiết bị trong phiếu:");
                foreach (var item in slip.Items)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine($"Tổng phí phạt của phiếu: {slip.CalculateTotalLateFee():N0} VNĐ");
            }
            catch (BorrowingException ex)
            {
                Console.WriteLine("[Lỗi nghiệp vụ] " + ex.Message);
            }
            finally
            {
                Console.WriteLine("-> Kết thúc kiểm thử luồng thành công.\n");
            }

            // --- MỤC 11.2: LUỒNG PHÁT SINH NGOẠI LỆ (7 TÌNH HUỐNG) ---
            Console.WriteLine("--- [3] MỤC 11.2: KIỂM THỬ 7 TÌNH HUỐNG NGOẠI LỆ ---");

            // Tình huống 1: Mượn ĐK điều hòa đang UnderMaintenance
            RunExceptionTest("TH1: Mượn điều khiển điều hòa đang UnderMaintenance", () =>
            {
                acRemote.MarkAsUnderMaintenance();
                BorrowingSlip s = new BorrowingSlip("S_ERR1", lecturer, "P301", borrowDate);
                s.AddDevice(acRemote, 1);
            });

            // Tình huống 2: Trợ giảng mượn thiết bị thứ 3 (tối đa 2)
            RunExceptionTest("TH2: Trợ giảng mượn thiết bị thứ ba", () =>
            {
                IBorrower ta = new TeachingAssistant("TA001", "Trần Thị B");
                BorrowingSlip s = new BorrowingSlip("S_ERR2", ta, "P302", borrowDate);
                s.AddDevice(new ProjectorRemote("PR_A", "ĐK Chiếu A", 2024, 300000m), 1);
                s.AddDevice(new AirConditionerRemote("AC_A", "ĐK ĐH A", 2024, 300000m), 1);
                s.AddDevice(new Microphone("MC_A", "Micro A", 2024, 900000m), 1);
            });

            // Tình huống 3: Thêm cùng một micro hai lần vào phiếu
            RunExceptionTest("TH3: Thêm cùng một micro hai lần", () =>
            {
                BorrowingSlip s = new BorrowingSlip("S_ERR3", lecturer, "P301", borrowDate);
                var mic = new Microphone("MC_DUP", "Micro B", 2024, 900000m, 1);
                s.AddDevice(mic, 1);
                s.AddDevice(mic, 1);
            });

            // Tình huống 4: Mượn micro trong 3 ngày dù giới hạn là 1 ngày
            RunExceptionTest("TH4: Mượn micro 3 ngày vượt giới hạn 1 ngày", () =>
            {
                BorrowingSlip s = new BorrowingSlip("S_ERR4", lecturer, "P301", borrowDate);
                var mic = new Microphone("MC_LIM", "Micro C", 2024, 900000m, maxBorrowDays: 1);
                s.AddDevice(mic, 3);
            });

            // Tình huống 5: Trả một điều khiển máy chiếu không thuộc phiếu
            RunExceptionTest("TH5: Trả ĐK máy chiếu không thuộc phiếu", () =>
            {
                BorrowingSlip s = new BorrowingSlip("S_ERR5", lecturer, "P301", borrowDate);
                s.ReturnDevice("PR_NOT_EXIST", borrowDate.AddDays(1));
            });

            // Tình huống 6: Trả cùng một thiết bị hai lần
            RunExceptionTest("TH6: Trả cùng một thiết bị hai lần", () =>
            {
                BorrowingSlip s = new BorrowingSlip("S_ERR6", lecturer, "P301", borrowDate);
                var pr = new ProjectorRemote("PR_RET2", "ĐK Chiếu D", 2024, 400000m, 1);
                s.AddDevice(pr, 1);
                s.ReturnDevice("PR_RET2", borrowDate.AddDays(1));
                s.ReturnDevice("PR_RET2", borrowDate.AddDays(1));
            });

            // Tình huống 7: Trả thiết bị với ngày trả trước ngày mượn
            RunExceptionTest("TH7: Trả thiết bị với ngày trả trước ngày mượn", () =>
            {
                BorrowingSlip s = new BorrowingSlip("S_ERR7", lecturer, "P301", borrowDate);
                var pr = new ProjectorRemote("PR_DATE", "ĐK Chiếu E", 2024, 400000m, 1);
                s.AddDevice(pr, 1);
                s.ReturnDevice("PR_DATE", borrowDate.AddDays(-1));
            });
        }

        static void RunExceptionTest(string testName, Action action)
        {
            Console.WriteLine($"* {testName}:");
            try
            {
                action();
            }
            catch (BorrowingException ex)
            {
                Console.WriteLine($"  -> Bắt ngoại lệ thành công [{ex.GetType().Name}]: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("  -> Đã kiểm tra xong tình huống (Trạng thái hệ thống được giữ toàn vẹn).");
            }
        }
    }
}