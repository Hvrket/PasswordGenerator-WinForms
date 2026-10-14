using System;
using System.Drawing;
using System.Windows.Forms;

namespace PassGen
{
    // Убираем слово partial, теперь это обычный цельный класс формы
    public class MainForm : Form
    {
        private readonly PasswordGenerator _generator = new PasswordGenerator();

        // Элементы интерфейса
        private NumericUpDown numericLength;
        private TextBox txtPassword;
        private Label lblStrength;
        private Button btnGenerate;
        private Button btnCopy;
        private CheckBox chkLower;
        private CheckBox chkUpper;
        private CheckBox chkDigits;
        private CheckBox chkSpecials;

        public MainForm()
        {
            // Настройки внешнего вида окна
            this.Text = "Генератор безопасных паролей";
            this.Size = new Size(350, 420);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;

            // Самостоятельно создаем и расставляем элементы на экране
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Выбор длины
            Label lblLength = new Label() { Text = "Длина пароля:", Location = new Point(20, 20), Size = new Size(100, 20) };
            numericLength = new NumericUpDown() { Location = new Point(130, 18), Size = new Size(60, 20), Minimum = 4, Maximum = 64, Value = 12 };

            // Галочки настроек
            chkLower = new CheckBox() { Text = "Строчные буквы (a-z)", Location = new Point(20, 55), Size = new Size(200, 20), Checked = true };
            chkUpper = new CheckBox() { Text = "Прописные буквы (A-Z)", Location = new Point(20, 85), Size = new Size(200, 20), Checked = true };
            chkDigits = new CheckBox() { Text = "Цифры (0-9)", Location = new Point(20, 115), Size = new Size(200, 20), Checked = true };
            chkSpecials = new CheckBox() { Text = "Спецсимволы (!@#$)", Location = new Point(20, 145), Size = new Size(200, 20) };

            // Поле для пароля
            txtPassword = new TextBox() { Location = new Point(20, 185), Size = new Size(290, 25), Font = new Font("Arial", 11), ReadOnly = true };

            // Индикатор надежности
            lblStrength = new Label() { Text = "Надежность: еще не сгенерирован", Location = new Point(20, 220), Size = new Size(290, 20), ForeColor = Color.Gray };

            // Кнопки управления
            btnGenerate = new Button() { Text = "Сгенерировать", Location = new Point(20, 260), Size = new Size(290, 35), BackColor = Color.LightBlue };
            btnCopy = new Button() { Text = "Копировать в буфер", Location = new Point(20, 310), Size = new Size(290, 35) };

            // Подвязываем клики к методам ниже
            btnGenerate.Click += btnGenerate_Click;
            btnCopy.Click += btnCopy_Click;

            // Добавляем элементы на форму
            this.Controls.AddRange(new Control[] { lblLength, numericLength, chkLower, chkUpper, chkDigits, chkSpecials, txtPassword, lblStrength, btnGenerate, btnCopy });
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string password = _generator.Generate((int)numericLength.Value, chkLower.Checked, chkUpper.Checked, chkDigits.Checked, chkSpecials.Checked);
                txtPassword.Text = password;
                UpdateStrengthIndicator(password);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                Clipboard.SetText(txtPassword.Text);
                MessageBox.Show("Пароль успешно скопирован!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateStrengthIndicator(string password)
        {
            int score = _generator.CheckStrength(password, chkLower.Checked, chkUpper.Checked, chkDigits.Checked, chkSpecials.Checked);
            if (score <= 1) { lblStrength.Text = "Надежность: Слабый"; lblStrength.ForeColor = Color.Red; }
            else if (score == 2) { lblStrength.Text = "Надежность: Средний"; lblStrength.ForeColor = Color.Orange; }
            else { lblStrength.Text = "Надежность: Надежный"; lblStrength.ForeColor = Color.Green; }
        }
    }
}
