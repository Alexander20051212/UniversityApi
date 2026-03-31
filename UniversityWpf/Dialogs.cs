using System.Windows;
using System.Windows.Controls;

namespace UniversityWpf;

// ——— Диалог студента ———
public class StudentDialog : Window
{
    public Student Result { get; private set; } = new();
    private TextBox _name = new(), _group = new(), _email = new();
    private int _id;

    public StudentDialog(Student? s = null)
    {
        Title = s == null ? "Добавить студента" : "Редактировать студента";
        Width = 380; Height = 280;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ResizeMode = ResizeMode.NoResize;
        _id = s?.Id ?? 0;

        var grid = new Grid { Margin = new Thickness(20) };
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        AddLabel(grid, "Имя", 0);
        _name = AddTextBox(grid, s?.Name ?? "", 1);
        AddLabel(grid, "Группа", 2);
        _group = AddTextBox(grid, s?.Group ?? "", 3);
        AddLabel(grid, "Email", 4);
        _email = AddTextBox(grid, s?.Email ?? "", 5);

        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 12, 0, 0) };
        var cancel = new Button { Content = "Отмена", Width = 90, Height = 32, Margin = new Thickness(0, 0, 8, 0), Background = System.Windows.Media.Brushes.Gray, Foreground = System.Windows.Media.Brushes.White, BorderThickness = new Thickness(0) };
        var save = new Button { Content = "Сохранить", Width = 90, Height = 32, Background = System.Windows.Media.Brushes.SteelBlue, Foreground = System.Windows.Media.Brushes.White, BorderThickness = new Thickness(0) };
        cancel.Click += (_, _) => { DialogResult = false; Close(); };
        save.Click += (_, _) => { Result = new Student { Id = _id, Name = _name.Text, Group = _group.Text, Email = _email.Text }; DialogResult = true; Close(); };
        buttons.Children.Add(cancel);
        buttons.Children.Add(save);
        Grid.SetRow(buttons, 7);
        grid.Children.Add(buttons);

        Content = grid;
    }

    private void AddLabel(Grid g, string text, int row)
    {
        var lbl = new TextBlock { Text = text, FontSize = 11, Foreground = System.Windows.Media.Brushes.Gray, Margin = new Thickness(0, row == 0 ? 0 : 8, 0, 3) };
        Grid.SetRow(lbl, row); g.Children.Add(lbl);
    }

    private TextBox AddTextBox(Grid g, string value, int row)
    {
        var tb = new TextBox { Text = value, Height = 30 };
        Grid.SetRow(tb, row); g.Children.Add(tb);
        return tb;
    }
}

// ——— Диалог преподавателя ———
public class TeacherDialog : Window
{
    public Teacher Result { get; private set; } = new();
    private TextBox _name = new(), _dept = new();
    private int _id;

    public TeacherDialog(Teacher? t = null)
    {
        Title = t == null ? "Добавить преподавателя" : "Редактировать преподавателя";
        Width = 380; Height = 230;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ResizeMode = ResizeMode.NoResize;
        _id = t?.Id ?? 0;

        var grid = new Grid { Margin = new Thickness(20) };
        for (int i = 0; i < 6; i++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        AddLabel(grid, "Имя", 0);
        _name = AddTextBox(grid, t?.Name ?? "", 1);
        AddLabel(grid, "Кафедра", 2);
        _dept = AddTextBox(grid, t?.Department ?? "", 3);

        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 12, 0, 0) };
        var cancel = new Button { Content = "Отмена", Width = 90, Height = 32, Margin = new Thickness(0, 0, 8, 0), Background = System.Windows.Media.Brushes.Gray, Foreground = System.Windows.Media.Brushes.White, BorderThickness = new Thickness(0) };
        var save = new Button { Content = "Сохранить", Width = 90, Height = 32, Background = System.Windows.Media.Brushes.SteelBlue, Foreground = System.Windows.Media.Brushes.White, BorderThickness = new Thickness(0) };
        cancel.Click += (_, _) => { DialogResult = false; Close(); };
        save.Click += (_, _) => { Result = new Teacher { Id = _id, Name = _name.Text, Department = _dept.Text }; DialogResult = true; Close(); };
        buttons.Children.Add(cancel); buttons.Children.Add(save);
        Grid.SetRow(buttons, 7); grid.Children.Add(buttons);
        Content = grid;
    }

    private void AddLabel(Grid g, string text, int row)
    {
        var lbl = new TextBlock { Text = text, FontSize = 11, Foreground = System.Windows.Media.Brushes.Gray, Margin = new Thickness(0, row == 0 ? 0 : 8, 0, 3) };
        Grid.SetRow(lbl, row); g.Children.Add(lbl);
    }

    private TextBox AddTextBox(Grid g, string value, int row)
    {
        var tb = new TextBox { Text = value, Height = 30 };
        Grid.SetRow(tb, row); g.Children.Add(tb);
        return tb;
    }
}

// ——— Диалог дисциплины ———
public class CourseDialog : Window
{
    public Course Result { get; private set; } = new();
    private TextBox _title = new();
    private TextBox _teacherId = new();
    private int _id;

    public CourseDialog(Course? c = null)
    {
        Title = c == null ? "Добавить дисциплину" : "Редактировать дисциплину";
        Width = 380; Height = 230;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ResizeMode = ResizeMode.NoResize;
        _id = c?.Id ?? 0;

        var grid = new Grid { Margin = new Thickness(20) };
        for (int i = 0; i < 6; i++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        AddLabel(grid, "Название", 0);
        _title = AddTextBox(grid, c?.Title ?? "", 1);
        AddLabel(grid, "ID преподавателя", 2);
        _teacherId = AddTextBox(grid, c?.TeacherId.ToString() ?? "", 3);

        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 12, 0, 0) };
        var cancel = new Button { Content = "Отмена", Width = 90, Height = 32, Margin = new Thickness(0, 0, 8, 0), Background = System.Windows.Media.Brushes.Gray, Foreground = System.Windows.Media.Brushes.White, BorderThickness = new Thickness(0) };
        var save = new Button { Content = "Сохранить", Width = 90, Height = 32, Background = System.Windows.Media.Brushes.SteelBlue, Foreground = System.Windows.Media.Brushes.White, BorderThickness = new Thickness(0) };
        cancel.Click += (_, _) => { DialogResult = false; Close(); };
        save.Click += (_, _) =>
        {
            if (!int.TryParse(_teacherId.Text, out int tid)) { MessageBox.Show("ID преподавателя должен быть числом"); return; }
            Result = new Course { Id = _id, Title = _title.Text, TeacherId = tid };
            DialogResult = true; Close();
        };
        buttons.Children.Add(cancel); buttons.Children.Add(save);
        Grid.SetRow(buttons, 7); grid.Children.Add(buttons);
        Content = grid;
    }

    private void AddLabel(Grid g, string text, int row)
    {
        var lbl = new TextBlock { Text = text, FontSize = 11, Foreground = System.Windows.Media.Brushes.Gray, Margin = new Thickness(0, row == 0 ? 0 : 8, 0, 3) };
        Grid.SetRow(lbl, row); g.Children.Add(lbl);
    }

    private TextBox AddTextBox(Grid g, string value, int row)
    {
        var tb = new TextBox { Text = value, Height = 30 };
        Grid.SetRow(tb, row); g.Children.Add(tb);
        return tb;
    }
}

// ——— Диалог оценки ———
public class GradeDialog : Window
{
    public Grade Result { get; private set; } = new();
    private TextBox _studentId = new(), _courseId = new(), _value = new();

    public GradeDialog()
    {
        Title = "Добавить оценку";
        Width = 380; Height = 270;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ResizeMode = ResizeMode.NoResize;

        var grid = new Grid { Margin = new Thickness(20) };
        for (int i = 0; i < 8; i++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        AddLabel(grid, "ID студента", 0);
        _studentId = AddTextBox(grid, "", 1);
        AddLabel(grid, "ID дисциплины", 2);
        _courseId = AddTextBox(grid, "", 3);
        AddLabel(grid, "Оценка (2-5)", 4);
        _value = AddTextBox(grid, "", 5);

        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 12, 0, 0) };
        var cancel = new Button { Content = "Отмена", Width = 90, Height = 32, Margin = new Thickness(0, 0, 8, 0), Background = System.Windows.Media.Brushes.Gray, Foreground = System.Windows.Media.Brushes.White, BorderThickness = new Thickness(0) };
        var save = new Button { Content = "Сохранить", Width = 90, Height = 32, Background = System.Windows.Media.Brushes.SteelBlue, Foreground = System.Windows.Media.Brushes.White, BorderThickness = new Thickness(0) };
        cancel.Click += (_, _) => { DialogResult = false; Close(); };
        save.Click += (_, _) =>
        {
            if (!int.TryParse(_studentId.Text, out int sid) ||
                !int.TryParse(_courseId.Text, out int cid) ||
                !int.TryParse(_value.Text, out int val) || val < 2 || val > 5)
            {
                MessageBox.Show("Проверьте правильность ввода. Оценка от 2 до 5.");
                return;
            }
            Result = new Grade { StudentId = sid, CourseId = cid, Value = val, Date = DateTime.UtcNow };
            DialogResult = true; Close();
        };
        buttons.Children.Add(cancel); buttons.Children.Add(save);
        Grid.SetRow(buttons, 9); grid.Children.Add(buttons);
        Content = grid;
    }

    private void AddLabel(Grid g, string text, int row)
    {
        var lbl = new TextBlock { Text = text, FontSize = 11, Foreground = System.Windows.Media.Brushes.Gray, Margin = new Thickness(0, row == 0 ? 0 : 8, 0, 3) };
        Grid.SetRow(lbl, row); g.Children.Add(lbl);
    }

    private TextBox AddTextBox(Grid g, string value, int row)
    {
        var tb = new TextBox { Text = value, Height = 30 };
        Grid.SetRow(tb, row); g.Children.Add(tb);
        return tb;
    }
}
