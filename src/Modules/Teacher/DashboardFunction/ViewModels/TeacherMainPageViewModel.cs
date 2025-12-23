using LaboratoryApp.src.Core.ViewModels;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace LaboratoryApp.src.Modules.Teacher.DashboardFunction.ViewModels
{
    // ViewModel chịu trách nhiệm chuẩn bị dữ liệu cho View hiển thị.
    // Kế thừa INotifyPropertyChanged để thông báo cho giao diện khi dữ liệu thay đổi.
    public class TeacherMainPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        // --- 1. CÁC THUỘC TÍNH DỮ LIỆU (PROPERTIES) ---

        // Số liệu thống kê (Sử dụng backing field để kích hoạt OnPropertyChanged khi set)
        private int _pendingGradingCount;
        public int PendingGradingCount
        {
            get => _pendingGradingCount;
            set { _pendingGradingCount = value; OnPropertyChanged(); }
        }

        private int _submittedTodayCount;
        public int SubmittedTodayCount
        {
            get => _submittedTodayCount;
            set { _submittedTodayCount = value; OnPropertyChanged(); }
        }

        private int _assignmentsDueCount;
        public int AssignmentsDueCount
        {
            get => _assignmentsDueCount;
            set { _assignmentsDueCount = value; OnPropertyChanged(); }
        }

        // Dữ liệu cho biểu đồ LiveCharts
        public SeriesCollection ActivitySeries { get; set; }
        public string[] ActivityLabels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        // Danh sách hoạt động gần đây
        // ObservableCollection giúp giao diện tự cập nhật khi thêm/xóa item
        public ObservableCollection<ActivityItem> RecentActivities { get; set; }

        // --- 2. HÀM KHỞI TẠO (CONSTRUCTOR) ---
        public TeacherMainPageViewModel()
        {
            // Khởi tạo các danh sách
            RecentActivities = new ObservableCollection<ActivityItem>();

            // Load dữ liệu giả lập (Sau này có thể thay bằng gọi Database/API)
            LoadDashboardData();
        }

        // --- 3. LOGIC XỬ LÝ DỮ LIỆU ---
        private void LoadDashboardData()
        {
            // A. Gán số liệu thống kê thẻ
            PendingGradingCount = 15;
            SubmittedTodayCount = 42;
            AssignmentsDueCount = 3;

            // B. Cấu hình biểu đồ (Chart)
            SetupChartData();

            // C. Load danh sách hoạt động
            RecentActivities.Add(new ActivityItem { Description = "Nguyen Van A submitted Lab Report 1", SubDetail = "Physics Class - Grade 10", TimeAgo = "17 min ago" });
            RecentActivities.Add(new ActivityItem { Description = "Le Thi B completed Quiz 3", SubDetail = "Chemistry Class - Grade 11", TimeAgo = "3 hours ago" });
            RecentActivities.Add(new ActivityItem { Description = "Tran Van C requested help", SubDetail = "Project X - Biology", TimeAgo = "5 hours ago" });
            RecentActivities.Add(new ActivityItem { Description = "Nguyen Van A submitted Lab Report 1", SubDetail = "Physics Class - Grade 10", TimeAgo = "1 day ago" });
            RecentActivities.Add(new ActivityItem { Description = "System Maintenance", SubDetail = "Server Update", TimeAgo = "2 days ago" });
        }

        private void SetupChartData()
        {
            ActivitySeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Submissions",
                    // Dữ liệu mẫu cho biểu đồ
                    Values = new ChartValues<double> { 5, 15, 28, 10, 0, 58, 0, 20, 58, 10, 38, 5 },
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 10,
                    LineSmoothness = 1, // 1 = Đường cong mượt, 0 = Đường gấp khúc
                    StrokeThickness = 3,
                    Stroke = new SolidColorBrush(Color.FromRgb(46, 134, 222)), // Màu xanh dương (#2E86DE)
                    // Tạo hiệu ứng màu loang dần (Gradient) bên dưới đường biểu đồ
                    Fill = new LinearGradientBrush
                    {
                        StartPoint = new System.Windows.Point(0, 0),
                        EndPoint = new System.Windows.Point(0, 1),
                        GradientStops = new GradientStopCollection
                        {
                            new GradientStop(Color.FromArgb(100, 46, 134, 222), 0), // Màu nhạt ở trên
                            new GradientStop(Color.FromArgb(10, 46, 134, 222), 1)   // Gần như trong suốt ở dưới
                        }
                    }
                }
            };

            // Nhãn trục X (Thứ trong tuần)
            ActivityLabels = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun", "Mon", "Tue", "Wed", "Thu" };
            // Format trục Y (số nguyên)
            YFormatter = value => value.ToString("N0");
        }

        // --- 4. IMPLEMENT INotifyPropertyChanged (BOILERPLATE CODE) ---
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Class Model đơn giản cho từng dòng hoạt động
    public class ActivityItem
    {
        public string Description { get; set; }
        public string SubDetail { get; set; }
        public string TimeAgo { get; set; }
    }
}