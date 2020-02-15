using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace FB2Renamer
{
    public partial class MainForm : Form
    {
        string bookPath;
        public MainForm()
        {
            InitializeComponent();
            string[] arg = Environment.GetCommandLineArgs();
            if (arg.Length == 1)
            {
                MessageBox.Show("Не указана книга для переименования!", "Ошибка");
                Environment.Exit(0);
            }
            else
            {
                bookPath = arg[1];
            }
            getBookInfo();
        }

        private void getBookInfo()
        {
            XmlDocument book = new XmlDocument();
            book.Load(bookPath);
            XmlNamespaceManager xmlnm = new XmlNamespaceManager(book.NameTable);
            xmlnm.AddNamespace("book", book.DocumentElement.NamespaceURI);
            textBoxName1.Text = book.SelectSingleNode("//book:first-name", xmlnm).InnerText;
            textBoxName2.Text = book.SelectSingleNode("//book:last-name", xmlnm).InnerText;
            textBoxTitle.Text = book.SelectSingleNode("//book:book-title", xmlnm).InnerText;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            string fn = "";
            string bin = (checkBox1.Checked?"1":"0")+(checkBox2.Checked?"1":"0")+(checkBox3.Checked?"1":"0");
            string[] str = new string[] { textBoxName1.Text, textBoxName2.Text, textBoxTitle.Text };
            switch (bin)
            {
                case "111": fn = str[0] + " " + str[1] + " - " + str[2]; break;
                case "110": fn = str[0] + " " + str[1]; break;
                case "101": fn = str[0] + " - " + str[2]; break;
                case "100": fn = str[0]; break;
                case "011": fn = str[1] + " - " + str[2]; break;
                case "010": fn = str[1]; break;
                case "001": fn = str[2]; break;
            }
            if (fn.Length == 0)
            {
                labelFilename.Text = "";
                buttonRename.Enabled = false;
            }
            else
            {
                labelFilename.Text = fn + ".fb2";
                buttonRename.Enabled = true;
            }
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(bookPath);
            string newPath = file.Directory.ToString() + "\\" + labelFilename.Text;
            try
            {
                File.Move(bookPath, newPath);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка переменования. Например, это может быть вызвано наличием недопустимых символов в новом имени файла.","Ошибка!");
                return;
            }
            Application.Exit();
        }

        private void checkBox_Click(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            switch (checkBox.Name)
            {
                case "checkBox1": textBoxName1.Enabled = !textBoxName1.Enabled; break;
                case "checkBox2": textBoxName2.Enabled = !textBoxName2.Enabled; break;
                case "checkBox3": textBoxTitle.Enabled = !textBoxTitle.Enabled; break;
            }
            textBox_TextChanged(sender, e);
        }
    }
}
