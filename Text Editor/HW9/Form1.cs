using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HW9
{
    //Якщо чесно, я не дуже в'їхав як зробити так, щоб друкувало PDF'ом у файл, хоча в мене є код з пари.
    //Зато просто надрукувати на папір виходить)

    public partial class Form1 : Form
    {
        private string SelectedTabTextBox { get; set; }

        private string TextToPrint { get; set; }

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            NewTabButton_Click(null, null);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewTabButton_Click(object sender, EventArgs e)
        {
            TabPage tabPage = new TabPage("НовийТекстовийДокумент" + (tabControl1.TabCount + 1));

            TextBox textBox = new TextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Both;

            tabPage.Controls.Add(textBox);

            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Текстовий Документ|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedTabTextBox = File.ReadAllText(openFileDialog.FileName, Encoding.Default);

                TabPage tabPage = new TabPage(Path.GetFileName(openFileDialog.FileName));

                foreach (TabPage tp in tabControl1.TabPages)
                    if (tp.Text == tabPage.Text)
                    {
                        MessageBox.Show("Вкладка з таким іменем вже відкрита");
                        return;
                    }

                TextBox textBox = new TextBox();
                textBox.Dock = DockStyle.Fill;
                textBox.Multiline = true;
                textBox.ScrollBars = ScrollBars.Both;
                textBox.Text = SelectedTabTextBox;
                tabPage.Controls.Add(textBox);
                tabControl1.TabPages.Add(tabPage);
                tabControl1.SelectedTab = tabPage;
            }
        }

        private void CloseTabButton_Click(object sender, EventArgs e)
        {
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textBox && textBox != null)
                    SelectedTabTextBox = textBox.Text;

            if (DialogResult.Yes == MessageBox.Show(
                 "Чи бажаєте ви зберегти файл перед закриттям?",
                 "Повідомлення",
                 MessageBoxButtons.YesNo))
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string fname = saveFileDialog1.FileName;
                File.WriteAllText(fname, SelectedTabTextBox);
                MessageBox.Show("Файл було збережено");
            }
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textBox && textBox != null)
                    SelectedTabTextBox = textBox.Text;

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string fname = saveFileDialog1.FileName;
            File.WriteAllText(fname, SelectedTabTextBox);
            MessageBox.Show("Файл було збережено");
        }

        private void збільшитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textBox && textBox != null)
                    textBox.Font = new Font(textBox.Font.Name, textBox.Font.Size + 3, textBox.Font.Style);
        }

        private void зменшитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textBox && textBox != null)
                    textBox.Font = new Font(textBox.Font.Name, textBox.Font.Size - 3, textBox.Font.Style);
        }
        private void заЗамовчуваннямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textBox && textBox != null)
                    textBox.Font = new Font(textBox.Font.Name, 9, textBox.Font.Style);
        }

        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var tb in tabControl1.SelectedTab.Controls)
                    if (tb is TextBox textBox && textBox != null)
                        textBox.Font = fontDialog.Font;
            }
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Програму зробив студент групи P01 для домашки. Котвіцький Ілля (2022)");
        }

        private void PrintTextFileButton_Click(object sender, EventArgs e)
        {
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textBox && textBox != null)
                    SelectedTabTextBox = textBox.Text;

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {

                TextToPrint = SelectedTabTextBox;
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            TextBox textBox = null;
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textbox && textbox != null)
                    textBox = textbox;

            Graphics gr = e.Graphics;

            gr.MeasureString(TextToPrint, textBox.Font, e.MarginBounds.Size,
            StringFormat.GenericTypographic,
            out int charOnPage, out int lineOnPage);

            Brush brush = new SolidBrush(textBox.ForeColor);
            gr.DrawString(TextToPrint.Substring(0, charOnPage), textBox.Font,
            brush, e.MarginBounds);

            TextToPrint = TextToPrint.Substring(charOnPage);

            e.HasMorePages = TextToPrint.Length > 0;

            if (!e.HasMorePages)
                TextToPrint = textBox.Text;
        }

        private void копіюватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox textBox = null;
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textbox && textbox != null)
                    textBox = textbox;

            if (textBox.SelectionLength > 0)
            {
                textBox.Copy();
            }
        }

        private void вставитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox textBox = null;
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textbox && textbox != null)
                    textBox = textbox;

            if (textBox.SelectionLength > 0)
            {

                if (MessageBox.Show("Ви дійсно хочете вставити в поточну позицію?", "Попередження", MessageBoxButtons.YesNo) == DialogResult.No)
                    textBox.SelectionStart += textBox.SelectionLength;
            }
            textBox.Paste();
        }

        private void вирізатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox textBox = null;
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textbox && textbox != null)
                    textBox = textbox;

            if(textBox.SelectionLength > 0)
            {
                textBox.Cut();
            }
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox textBox = null;
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textbox && textbox != null)
                    textBox = textbox;

            if (textBox.CanUndo == true)
            {
                textBox.Undo();
                textBox.ClearUndo();
            }
        }

        private void виділитиВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox textBox = null;
            foreach (var tb in tabControl1.SelectedTab.Controls)
                if (tb is TextBox textbox && textbox != null)
                    textBox = textbox;

            if (textBox.TextLength > 0)
            {
                textBox.SelectAll();
            }
        }
    }
}
