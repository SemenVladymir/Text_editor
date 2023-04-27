using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Text_editor
{
    public partial class Form1 : Form
    {
        string sample = "";
        ChangeMemory changes = new ChangeMemory(true, "", 0);
        MemoryStack MyStack = new MemoryStack();
        bool bUndo = false;
        bool bRedo = false;
        bool capSymbol = false;
        string buffer = "";
        int number = 1;
        ToolStripTextBox fontSizeItem = new ToolStripTextBox();
        ToolStripTextBox findText = new ToolStripTextBox();
        ToolStripButton tsUndo = new ToolStripButton();
        ToolStripButton tsRedo = new ToolStripButton();
        ToolStripMenuItem CapitalFirstLetter = new ToolStripMenuItem();
        ToolStripMenuItem AutoNumerationList = new ToolStripMenuItem();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            menu = new MenuStrip();
            menu.BackColor = SystemColors.Control;
            ToolStripMenuItem fileItem = new ToolStripMenuItem();
            fileItem.ShowShortcutKeys = true;
            fileItem.Text = "File";
            menu.Font = new Font("Calibri", 13);
            menu.LayoutStyle = ToolStripLayoutStyle.Flow;
            menu.ShowItemToolTips = true;
            fileItem.DropDownItems.Add(new ToolStripMenuItem("New", null, new EventHandler((s, ee) => NewText_Click(s, ee))) { ShortcutKeys = Keys.Control | Keys.N });
            fileItem.DropDownItems.Add(new ToolStripMenuItem("Open", null, new EventHandler((s, ee) => Load_Click(s, ee))) { ShortcutKeys = Keys.Control | Keys.O });
            fileItem.DropDownItems.Add(new ToolStripMenuItem("Save", null, new EventHandler((s, ee) => Save_Click(s, ee))) { ShortcutKeys = Keys.Control | Keys.S });
            fileItem.DropDownItems.Add(new ToolStripMenuItem("Print", null, new EventHandler((s, ee) => PrintBtn_Click(s, ee))) { ShortcutKeys = Keys.Control | Keys.P });
            fileItem.DropDownItems.Add(new ToolStripSeparator());
            fileItem.DropDownItems.Add(new ToolStripMenuItem("Exit", null, new EventHandler((s, ee) => this.Close())));

            CapitalFirstLetter.Text = "Set the capital first letter in a sentence";
            CapitalFirstLetter.CheckOnClick = true;
            CapitalFirstLetter.Click += CapitalFirstLetter_Click;
            
            AutoNumerationList.Text = "Set autonumeration after a colon";
            AutoNumerationList.CheckOnClick = true;

            ToolStripMenuItem mainItem = new ToolStripMenuItem();
            mainItem.ShowShortcutKeys = true;
            mainItem.Text = "Facilitations";
            mainItem.DropDownItems.Add(CapitalFirstLetter);
            mainItem.DropDownItems.Add(AutoNumerationList);
            
            ToolStripMenuItem editItem = new ToolStripMenuItem();
            editItem.ShowShortcutKeys = true;
            editItem.Text = "Edit";
            editItem.DropDownItems.Add(new ToolStripMenuItem("Copy", null, new EventHandler((s, ee) => Copy_Click(s, ee))) { ShortcutKeys = Keys.Control | Keys.C });
            editItem.DropDownItems.Add(new ToolStripMenuItem("Cut", null, new EventHandler((s, ee) => Cut_Click(s, ee))) { ShortcutKeys = Keys.Control | Keys.X });
            editItem.DropDownItems.Add(new ToolStripMenuItem("Paste", null, new EventHandler((s, ee) => Paste_Click(s, ee))) { ShortcutKeys = Keys.Control | Keys.V });

            tsUndo.Text = "Undo";
            tsUndo.Enabled = false;
            tsUndo.ForeColor = Color.Blue;
            tsUndo.Image = Bitmap.FromFile("C:\\Users\\User\\OneDrive\\Documents\\GitHub\\Text_editor\\obj\\Debug\\undo.jpg");
            tsUndo.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            tsUndo.Click += Undo_Click;

            tsRedo.Text = "Redo";
            tsRedo.Enabled = false;
            tsRedo.ForeColor = Color.Blue;
            tsRedo.Image = Bitmap.FromFile("C:\\Users\\User\\OneDrive\\Documents\\GitHub\\Text_editor\\obj\\Debug\\redo.jpg");
            tsRedo.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            tsRedo.Click += Redo_Click;

            ToolStripMenuItem aboutItem = new ToolStripMenuItem();
            aboutItem.Click +=  About_Click;
            aboutItem.ShortcutKeys = Keys.Control | Keys.A;
            aboutItem.Text = "About";

            fontSizeItem.Size = new Size(30, fontSizeItem.Height);
            fontSizeItem.Text = "12";
            fontSizeItem.Font = new Font("Calibri", 13);
            fontSizeItem.TextChanged += FontSize_TextChanged;

            findText.Font = new Font("Calibri", 13);
            findText.KeyDown += Find_Click;

            cmbBox = new ToolStripComboBox();
            cmbBox.Size = new Size(180, cmbBox.Height);
            cmbBox.Font = new Font("Calibri", 13);
            FontFamily[] fontFamilies;
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();
            fontFamilies = installedFontCollection.Families;
            foreach (var tmp in fontFamilies)
                cmbBox.Items.Add(tmp.Name.ToString());
            cmbBox.SelectedIndex = 0;
            cmbBox.TextChanged += CmbBox_TextChanged;

            alignmText = new ToolStripComboBox();
            alignmText.Font = new Font("Calibri", 13);
            alignmText.Text = "Alignment";
            alignmText.Items.Add("Left");
            alignmText.Items.Add("Center");
            alignmText.Items.Add("Right");
            alignmText.SelectedIndex = 0;
            alignmText.TextChanged += AlignmText_TextChanged;



            menu.Items.Add(fileItem);
            menu.Items.Add(editItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripLabel("Search"));
            menu.Items.Add(findText);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(tsUndo);
            menu.Items.Add(tsRedo);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(fontSizeItem);
            menu.Items.Add(cmbBox);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(alignmText);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(mainItem);
            menu.Items.Add(aboutItem);

            this.Controls.Add(menu);

            textBox = new TextBox();
            textBox.Width = Width - 35;
            textBox.Height = Height - 130;
            textBox.Location = new Point(10, 50);
            textBox.Font = new Font(textBox.Font.FontFamily, 12);
            textBox.ScrollBars = ScrollBars.Both;
            textBox.WordWrap = true;
            textBox.Multiline = true;
            textBox.AcceptsTab = true;
            textBox.TextChanged += TextBox_TextChanged;
            Controls.Add(textBox);
            this.Resize += Form1_Resize;
        }

        private void NewText_Click(object s, EventArgs ee)
        {
            if (textBox.Text.Length > 0)
            {
                if (MessageBox.Show("Do you want to save this text?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "text|*.txt";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(dialog.FileName, textBox.Text);
                        MyStack.MainStackClear();
                        MyStack.BackStackClear();
                        tsUndo.Enabled = false;
                        tsRedo.Enabled = false;
                    }
                }
                else
                {
                    textBox.Text = "";
                    MyStack.MainStackClear();
                    MyStack.BackStackClear();
                    tsUndo.Enabled = false;
                    tsRedo.Enabled = false;
                }
            }
            this.Text = "Text editor [New]";
        }

        private void Find_Click(object s, System.Windows.Forms.KeyEventArgs ee)
        {
            if (ee.KeyCode == Keys.Enter && findText.Text.Length > 0)
            {
                textBox.Focus();
                int index;
                int findStart = 0;
                int length = findText.Text.Length;
                if (textBox.Text.IndexOf(findText.Text, findStart) != -1)
                {
                    do
                    {
                        index = textBox.Text.IndexOf(findText.Text, findStart);

                        if (index != -1)
                        {
                            textBox.SelectionStart = index;
                            textBox.SelectionLength = length;
                            findStart = index + length;
                            if (MessageBox.Show("Do you want to continue searching?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                                index = -2;
                        }
                    } while (index > -1);
                    if (index == -1)
                        MessageBox.Show("The end of the text!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                    MessageBox.Show("Anything didn`t find!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void Paste_Click(object s, EventArgs ee)
        {
            int start = textBox.SelectionStart;
            if (!string.IsNullOrEmpty(buffer))
                textBox.Text = textBox.Text.Insert(textBox.SelectionStart, buffer);
            textBox.SelectionStart = start + buffer.Length;
        }

        private void Cut_Click(object s, EventArgs ee)
        {
            int start = textBox.SelectionStart;
            int length = textBox.SelectionLength;
            int txtLength = textBox.Text.Length;
            buffer = string.Copy(textBox.Text.Substring(start, length));
            textBox.Text = textBox.Text.Substring(0, start) + textBox.Text.Substring(start + length, txtLength - (start + length));

        }

        private void Copy_Click(object s, EventArgs ee)
        {
            buffer = string.Copy(textBox.Text.Substring(textBox.SelectionStart, textBox.SelectionLength));
        }

        private void CapitalFirstLetter_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tTmp = sender as ToolStripMenuItem;
            if (CapitalFirstLetter.Checked)
            {
                string tmp = "";
                string[] strArr = textBox.Text.Split('.');
                for (int i = 0; i < strArr.Length - 1; i++)
                {
                    if (!strArr[i][0].Equals(" "))
                        strArr[i] = strArr[i].Substring(0, 1) + char.ToUpper(strArr[i][1]) + strArr[i].Substring(2) + ".";
                    else
                        strArr[i] = char.ToUpper(strArr[i][0]) + strArr[i].Substring(1) + ".";
                    tmp += strArr[i];
                }
                textBox.Text = tmp;
            }
        }

        private void AlignmText_TextChanged(object sender, EventArgs e)
        {
            switch (alignmText.SelectedIndex)
            {
                case 0:
                    textBox.TextAlign = HorizontalAlignment.Left;
                    break;
                case 1:
                    textBox.TextAlign = HorizontalAlignment.Center;
                    break;
                case 2:
                    textBox.TextAlign = HorizontalAlignment.Right;
                    break;
            }
            
        }

        private void CmbBox_TextChanged(object sender, EventArgs e)
        {
            string txtStile = cmbBox.SelectedItem.ToString();
            textBox.Font = new Font(txtStile, textBox.Font.Size);
        }

        private void About_Click(object s, EventArgs ee)
        {
            string tmp = textBox.Text.ToString();
            int num = 0;
            int symb = 0;
            int letters = 0;
            int sepsr = 0;
            char[] separ = new char[100];
            foreach (char ch in tmp)
            {
                if (Char.IsNumber(ch))
                    num++;
                if (Char.IsPunctuation(ch))
                    symb++;
                if (Char.IsLetter(ch))
                    letters++;
                if (Char.IsSeparator(ch))
                {
                    separ[sepsr] = ch;
                    sepsr++;
                }
            }
            int words = tmp.Split(separ).Length;
            //File.WriteAllText("Statistic", $"Words - {words};\nSymbols - {symb};\nDigits - {num};\nLetters - {letters}");
            MessageBox.Show(
               $"Words - {words};\nPunctuations - {symb};\nDigits - {num};\nLetters - {letters}",
               "Text statistic",
               MessageBoxButtons.OK,
               MessageBoxIcon.Information,
               MessageBoxDefaultButton.Button1,
               MessageBoxOptions.DefaultDesktopOnly);
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //dialog.Document;
            }
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            bUndo = true;
            if (MyStack.MainStack != null)
            {
                changes = MyStack.MainStackPull();
                if (changes!=null && !changes.TypeOfChange)
                {
                    if (string.IsNullOrEmpty(sample))
                        sample = changes.ChangeText;
                    textBox.Text = sample.Insert(changes.Position, changes.ChangeText);
                    changes.TypeOfChange = !changes.TypeOfChange;
                    MyStack.BackStackAdd(changes);
                    tsRedo.Enabled = true;
                }
                else if (changes!=null)
                {
                    textBox.Text = sample.Remove(changes.Position, changes.ChangeText.Length);
                    changes.TypeOfChange = !changes.TypeOfChange;
                    MyStack.BackStackAdd(changes);
                    tsRedo.Enabled = true;
                }
            }
            if (MyStack.MainStack.Count == 0)
            {
                tsUndo.Enabled = false;
            }
            bUndo = false;
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            bRedo = true;
            if (MyStack.BackStack != null) 
            {
                changes = MyStack.BackStackPull();
                if (changes != null && !changes.TypeOfChange)
                {
                    textBox.Text = sample.Insert(changes.Position, changes.ChangeText);
                    MyStack.MainStackAdd(!changes.TypeOfChange, changes.ChangeText, changes.Position);
                    tsUndo.Enabled = true;
                }
                else if (changes!=null)
                {

                    textBox.Text = sample.Remove(changes.Position, changes.ChangeText.Length);
                    MyStack.MainStackAdd(!changes.TypeOfChange, changes.ChangeText, changes.Position);
                    tsUndo.Enabled = true;
                }
            }
            if (MyStack.BackStack.Count == 0)
                {
                tsRedo.Enabled = false;
            }
            bRedo = false;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (CapitalFirstLetter.Checked && textBox.Text.Length == 1 && Convert.ToInt32(textBox.Text[textBox.Text.Length - 1]) > 64)
            {
                textBox.Text = char.ToUpper(textBox.Text[textBox.Text.Length - 1]).ToString();
                textBox.SelectionStart = textBox.Text.Length;
            }
            else if (CapitalFirstLetter.Checked && textBox.Text.Length == 1)
                capSymbol = true;
            bool tmp = false;
            if (!string.IsNullOrEmpty(sample) && !bUndo && !bRedo)
            {
                if (textBox.Text.Length > 0)
                {
                    char symb = textBox.Text[textBox.Text.Length - 1];
                    if (CapitalFirstLetter.Checked && (symb == '\n' || symb == '.' || symb == '?' || symb == '!'))
                        capSymbol = true;
                    if (Convert.ToInt32(textBox.Text[textBox.Text.Length - 1]) > 64 && capSymbol)
                    {
                        capSymbol = false;
                        string text1 = textBox.Text.Substring(0, textBox.Text.Length - 1);
                        textBox.Text = text1 + char.ToUpper(textBox.Text[textBox.Text.Length - 1]);
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                    if (symb == ':' && AutoNumerationList.Checked)
                    {
                        textBox.Text = textBox.Text + "\r\n" + $"{number++}) ";
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                    if (symb == '\n' && AutoNumerationList.Checked)
                    {
                        textBox.Text = textBox.Text + "\n" + $"{number++}) ";
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                }
                int length = textBox.Text.Length - sample.Length;
                if (length > 0)
                {
                    for (int i = 0; i < sample.Length; i++)
                    {
                        if (textBox.Text[i] != sample[i])
                        {
                            MyStack.MainStackAdd(true, textBox.Text.Substring(i, length), i);
                            tsUndo.Enabled = true;
                            i = textBox.Text.Length;
                            tmp = true;
                        }
                        if (i == (sample.Length - 1) && !tmp)
                        {
                            MyStack.MainStackAdd(true, textBox.Text.Substring(i+1, length), i+1);
                            tsUndo.Enabled = true;
                        }
                    }
                }
                if (length < 0)
                {
                    for (int i = 0; i < textBox.Text.Length; i++)
                    {
                        if (textBox.Text[i] != sample[i])
                        {
                            MyStack.MainStackAdd(false, sample.Substring(i, length * -1), i);
                            tsUndo.Enabled = true;
                            i = textBox.Text.Length;
                            tmp = true;
                        }
                        if (i == (textBox.Text.Length - 1) && !tmp)
                        {
                            MyStack.MainStackAdd(false, sample.Substring(i + 1, length * -1), i + 1);
                            tsUndo.Enabled = true;
                        }
                    }
                }
            }      
            sample = textBox.Text;      
        }

        private void FontSize_TextChanged(object sender, EventArgs e)
        {
            if (fontSizeItem.Text != "")
            {
                int size = Convert.ToInt32(fontSizeItem.Text);
                if (size > 2)
                    textBox.Font = new Font(textBox.Font.FontFamily, size);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "text|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, textBox.Text);
                this.Text = $"Text editor [{Path.GetFileNameWithoutExtension(dialog.FileName)}]";
                MyStack.MainStackClear();
                MyStack.BackStackClear();
                tsUndo.Enabled = false;
                tsRedo.Enabled = false;
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "text|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = File.ReadAllText(dialog.FileName);
                this.Text = $"Text editor [{Path.GetFileNameWithoutExtension(dialog.FileName)}]";
                sample = textBox.Text;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            textBox.Width = this.Width - 35;
            textBox.Height = this.Height - 130;
        }

        MenuStrip menu;

        ToolStripComboBox cmbBox;
        ToolStripComboBox alignmText;

        TextBox textBox;
    }
}
