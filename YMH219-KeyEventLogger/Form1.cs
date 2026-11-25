using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace YMH219_KeyEventLogger
{
    public partial class Form1 : Form
    {
        private readonly KeyLoggerManager _logger = new KeyLoggerManager();

        private Panel _pnlStatusBadge;
        private Label _lblStatusText;
        private Label _lblTotalCount;
        private Label _lblTopKeys;

        private Button _btnStart;
        private Button _btnStop;
        private Button _btnClear;
        private Button _btnSave;

        private CheckBox _chkEnableFilter;
        private DateTimePicker _dtpStart;
        private DateTimePicker _dtpEnd;
        private Button _btnApplyFilter;

        private ListBox _lstLogs;

        public Form1()
        {
            InitializeComponent();
            BuildModernUI();
            UpdateUIState();
        }

        private void BuildModernUI()
        {
            this.Text = "Key Event Logger - YMH219 (Eğitsel Uygulama)";
            this.Size = new Size(920, 620);
            this.MinimumSize = new Size(860, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Font = new Font("Segoe UI", 9.5f);
            this.KeyPreview = true; // Form aktifken tüm tuşları yakalamak için şart

            // 1. ÜST DURUM VE KONTROL PANELİ
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            _pnlStatusBadge = new Panel
            {
                Location = new Point(18, 16),
                Size = new Size(210, 42),
                BackColor = Color.FromArgb(220, 53, 69)
            };

            _lblStatusText = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "● KAYIT DURDURULDU"
            };
            _pnlStatusBadge.Controls.Add(_lblStatusText);

            _btnStart = CreateActionButton("▶ Başlat", new Point(250, 16), Color.FromArgb(25, 135, 84));
            _btnStop = CreateActionButton("■ Durdur", new Point(380, 16), Color.FromArgb(220, 53, 69));
            _btnClear = CreateActionButton("🗑 Temizle", new Point(510, 16), Color.FromArgb(108, 117, 125));
            _btnSave = CreateActionButton("💾 .TXT Kaydet", new Point(640, 16), Color.FromArgb(13, 110, 253), 145);

            _btnStart.Click += (s, e) => { _logger.Start(); UpdateUIState(); this.Focus(); };
            _btnStop.Click += (s, e) => { _logger.Stop(); UpdateUIState(); };
            _btnClear.Click += OnClearClick;
            _btnSave.Click += OnSaveClick;

            topPanel.Controls.AddRange(new Control[] { _pnlStatusBadge, _btnStart, _btnStop, _btnClear, _btnSave });

            // 2. FİLTRELEME PANELİ (Tarih/Saat Aralığı)
            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.FromArgb(233, 236, 239)
            };

            _chkEnableFilter = new CheckBox
            {
                Text = "Tarih/Saat Filtresi:",
                Location = new Point(18, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                TabStop = false
            };

            Label lblFrom = new Label { Text = "Başlangıç:", Location = new Point(175, 17), AutoSize = true };
            _dtpStart = new DateTimePicker
            {
                Location = new Point(245, 13),
                Width = 175,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm:ss",
                Enabled = false,
                TabStop = false
            };

            Label lblTo = new Label { Text = "Bitiş:", Location = new Point(435, 17), AutoSize = true };
            _dtpEnd = new DateTimePicker
            {
                Location = new Point(478, 13),
                Width = 175,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm:ss",
                Enabled = false,
                TabStop = false
            };

            _btnApplyFilter = new Button
            {
                Text = "Filtrele / Yenile",
                Location = new Point(668, 12),
                Size = new Size(125, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false,
                TabStop = false
            };

            _chkEnableFilter.CheckedChanged += (s, e) =>
            {
                bool active = _chkEnableFilter.Checked;
                _dtpStart.Enabled = active;
                _dtpEnd.Enabled = active;
                _btnApplyFilter.Enabled = active;
                RefreshLogList();
            };

            _btnApplyFilter.Click += (s, e) => RefreshLogList();

            filterPanel.Controls.AddRange(new Control[] { _chkEnableFilter, lblFrom, _dtpStart, lblTo, _dtpEnd, _btnApplyFilter });

            // 3. ALT İSTATİSTİK PANELİ
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 48,
                BackColor = Color.FromArgb(33, 37, 41)
            };

            _lblTotalCount = new Label
            {
                Location = new Point(18, 14),
                AutoSize = true,
                ForeColor = Color.Gold,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Text = "Toplam Tuş: 0"
            };

            _lblTopKeys = new Label
            {
                Location = new Point(180, 14),
                AutoSize = true,
                ForeColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                Text = "En Sık Kullanılanlar: Henüz kayıt yok"
            };

            bottomPanel.Controls.AddRange(new Control[] { _lblTotalCount, _lblTopKeys });

            // 4. ORTA KAYIT LİSTESİ (ListBox)
            _lstLogs = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10.5f),
                BackColor = Color.FromArgb(24, 26, 30),
                ForeColor = Color.FromArgb(144, 238, 144),
                BorderStyle = BorderStyle.None,
                IntegralHeight = false,
                TabStop = false
            };

            // Güvenlik bilgi notu
            Label lblNotice = new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = Color.FromArgb(255, 243, 205),
                ForeColor = Color.FromArgb(102, 77, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                Text = "Bilgi: Bu uygulama yalnızca kendi penceresi aktif (odakta) iken yapılan tuş vuruşlarını kaydeder."
            };

            this.Controls.Add(_lstLogs);
            this.Controls.Add(lblNotice);
            this.Controls.Add(filterPanel);
            this.Controls.Add(topPanel);
            this.Controls.Add(bottomPanel);

            this.KeyDown += OnFormKeyDown;
        }

        private Button CreateActionButton(string text, Point location, Color backColor, int width = 120)
        {
            return new Button
            {
                Text = text,
                Location = location,
                Size = new Size(width, 42),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
        }

        private void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (!_logger.IsRecording) return;

            var entry = new KeyLogEntry(e.KeyCode, e.Modifiers);
            _logger.AddEntry(entry);

            // Space/Enter gibi tuşların butonları yanlışlıkla tetiklemesini engelle
            e.SuppressKeyPress = true;

            RefreshLogList();
            UpdateStatistics();
        }

        private void RefreshLogList()
        {
            DateTime? start = _chkEnableFilter.Checked ? _dtpStart.Value : null;
            DateTime? end = _chkEnableFilter.Checked ? _dtpEnd.Value : null;

            var entries = _logger.GetFilteredEntries(start, end);

            _lstLogs.BeginUpdate();
            _lstLogs.Items.Clear();
            foreach (var item in entries)
            {
                _lstLogs.Items.Add(item.ToString());
            }
            if (_lstLogs.Items.Count > 0)
            {
                _lstLogs.TopIndex = _lstLogs.Items.Count - 1;
            }
            _lstLogs.EndUpdate();
        }

        private void UpdateStatistics()
        {
            _lblTotalCount.Text = $"Toplam Tuş: {_logger.TotalCount}";
            _lblTopKeys.Text = $"En Sık Kullanılanlar: {_logger.GetTopKeysSummary()}";
        }

        private void UpdateUIState()
        {
            if (_logger.IsRecording)
            {
                _pnlStatusBadge.BackColor = Color.FromArgb(25, 135, 84);
                _lblStatusText.Text = "● KAYIT AKTİF";
                _btnStart.Enabled = false;
                _btnStop.Enabled = true;
            }
            else
            {
                _pnlStatusBadge.BackColor = Color.FromArgb(220, 53, 69);
                _lblStatusText.Text = "● KAYIT DURDURULDU";
                _btnStart.Enabled = true;
                _btnStop.Enabled = false;
            }
        }

        private void OnClearClick(object sender, EventArgs e)
        {
            if (_logger.TotalCount == 0) return;

            var confirm = MessageBox.Show("Tüm tuş kayıtları silinsin mi?", "Temizle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _logger.Clear();
                RefreshLogList();
                UpdateStatistics();
            }
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            if (_logger.TotalCount == 0)
            {
                MessageBox.Show("Kaydedilecek herhangi bir tuş kaydı bulunmuyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Metin Dosyası (*.txt)|*.txt",
                FileName = $"KeyLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                Title = "Tuş Kayıtlarını Dışa Aktar"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DateTime? start = _chkEnableFilter.Checked ? _dtpStart.Value : null;
                    DateTime? end = _chkEnableFilter.Checked ? _dtpEnd.Value : null;
                    var entries = _logger.GetFilteredEntries(start, end);

                    _logger.SaveToFile(sfd.FileName, entries);
                    MessageBox.Show("Kayıtlar başarıyla .txt dosyasına aktarıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Dosya kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}