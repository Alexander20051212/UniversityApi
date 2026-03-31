using System.Windows;
using System.Windows.Controls;

namespace UniversityWpf;

public partial class MainWindow : Window
{
    private readonly ApiService _api = new();
    private int _currentTab = 0;

    public MainWindow()
    {
        InitializeComponent();
        LoadStudents();

        // Автообновление каждые 10 секунд
        var timer = new System.Windows.Threading.DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(10);
        timer.Tick += (_, _) => LoadCurrentTab();
        timer.Start();
    }

    private void LoadCurrentTab()
    {
        switch (_currentTab)
        {
            case 0: LoadStudents(); break;
            case 1: LoadTeachers(); break;
            case 2: LoadCourses(); break;
            case 3: LoadGrades(); break;
        }
    }

    private void RefreshAll(object sender, RoutedEventArgs e) => LoadCurrentTab();

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.Source is not TabControl tc) return;
        _currentTab = tc.SelectedIndex;
        LoadCurrentTab();
    }

    // ——— Студенты ———
    private async void LoadStudents()
    {
        try { StudentsGrid.ItemsSource = await _api.GetStudents(); }
        catch { }
    }

    private void AddStudent(object sender, RoutedEventArgs e)
    {
        var dlg = new StudentDialog();
        if (dlg.ShowDialog() == true)
            _ = SaveAndRefresh(() => _api.CreateStudent(dlg.Result), LoadStudents);
    }

    private void EditStudent(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not Student s) return;
        var dlg = new StudentDialog(s);
        if (dlg.ShowDialog() == true)
            _ = SaveAndRefresh(() => _api.UpdateStudent(dlg.Result), LoadStudents);
    }

    private async void DeleteStudent(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not Student s) return;
        if (MessageBox.Show($"Удалить студента {s.Name}?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
        try { await _api.DeleteStudent(s.Id); LoadStudents(); }
        catch { MessageBox.Show("Ошибка удаления", "Ошибка"); }
    }

    // ——— Преподаватели ———
    private async void LoadTeachers()
    {
        try { TeachersGrid.ItemsSource = await _api.GetTeachers(); }
        catch { }
    }

    private void AddTeacher(object sender, RoutedEventArgs e)
    {
        var dlg = new TeacherDialog();
        if (dlg.ShowDialog() == true)
            _ = SaveAndRefresh(() => _api.CreateTeacher(dlg.Result), LoadTeachers);
    }

    private void EditTeacher(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not Teacher t) return;
        var dlg = new TeacherDialog(t);
        if (dlg.ShowDialog() == true)
            _ = SaveAndRefresh(() => _api.UpdateTeacher(dlg.Result), LoadTeachers);
    }

    private async void DeleteTeacher(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not Teacher t) return;
        if (MessageBox.Show($"Удалить преподавателя {t.Name}?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
        try { await _api.DeleteTeacher(t.Id); LoadTeachers(); }
        catch { MessageBox.Show("Ошибка удаления", "Ошибка"); }
    }

    // ——— Дисциплины ———
    private async void LoadCourses()
    {
        try { CoursesGrid.ItemsSource = await _api.GetCourses(); }
        catch { }
    }

    private void AddCourse(object sender, RoutedEventArgs e)
    {
        var dlg = new CourseDialog();
        if (dlg.ShowDialog() == true)
            _ = SaveAndRefresh(() => _api.CreateCourse(dlg.Result), LoadCourses);
    }

    private void EditCourse(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not Course c) return;
        var dlg = new CourseDialog(c);
        if (dlg.ShowDialog() == true)
            _ = SaveAndRefresh(() => _api.UpdateCourse(dlg.Result), LoadCourses);
    }

    private async void DeleteCourse(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not Course c) return;
        if (MessageBox.Show($"Удалить дисциплину {c.Title}?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
        try { await _api.DeleteCourse(c.Id); LoadCourses(); }
        catch { MessageBox.Show("Ошибка удаления", "Ошибка"); }
    }

    // ——— Оценки ———
    private async void LoadGrades()
    {
        try { GradesGrid.ItemsSource = await _api.GetGrades(); }
        catch { }
    }

    private void AddGrade(object sender, RoutedEventArgs e)
    {
        var dlg = new GradeDialog();
        if (dlg.ShowDialog() == true)
            _ = SaveAndRefresh(() => _api.CreateGrade(dlg.Result), LoadGrades);
    }

    private async void DeleteGrade(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not Grade g) return;
        if (MessageBox.Show("Удалить оценку?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
        try { await _api.DeleteGrade(g.Id); LoadGrades(); }
        catch { MessageBox.Show("Ошибка удаления", "Ошибка"); }
    }

    // ——— Хелпер ———
    private async Task SaveAndRefresh(Func<Task<System.Net.Http.HttpResponseMessage>> action, Action reload)
    {
        try { await action(); reload(); }
        catch { MessageBox.Show("Ошибка сохранения", "Ошибка"); }
    }
}
